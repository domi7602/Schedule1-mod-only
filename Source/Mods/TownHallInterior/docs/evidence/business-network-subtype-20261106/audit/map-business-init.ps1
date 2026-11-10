[CmdletBinding()]
param(
    [string]$SelectionFile,
    [string]$OutputTag = 'inventory',
    [string]$ReferencesFile = 'world-exit-reference-candidates.json'
)
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$inputManifest = Get-Content -LiteralPath (Join-Path $root 'input-manifest.json') -Raw | ConvertFrom-Json
$bundle = $inputManifest.analysisToolDirectory
$game = $inputManifest.gameDirectory
foreach ($entry in $inputManifest.analysisTools) {
    $path = Join-Path $bundle $entry.path
    if ((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant() -ne $entry.sha256) { throw 'Analysis-tool hash changed.' }
    [void][Reflection.Assembly]::LoadFrom($path)
}
# Target binaries remain data. Only the separately hashed existing analysis libraries are loaded.
[LibCpp2IL.LibCpp2IlMain]::Settings.AllowManualMetadataAndCodeRegInput = $false
[LibCpp2IL.LibCpp2IlMain]::Settings.DisableMethodPointerMapping = $false
$version = [AssetRipper.Primitives.UnityVersion]::Parse('2022.3.62f2')
if (-not [LibCpp2IL.LibCpp2IlMain]::LoadFromFile((Join-Path $game 'GameAssembly.dll'), (Join-Path $game 'Schedule I_Data/il2cpp_data/Metadata/global-metadata.dat'), $version)) { throw 'Native metadata parse failed.' }
$metadata = [LibCpp2IL.LibCpp2IlMain]::TheMetadata
$methodType = [LibCpp2IL.Metadata.Il2CppMethodDefinition]
$pointerProperty = $methodType.GetProperty('MethodPointer')
$nameProperty = $methodType.GetProperty('Name')
$declaringTypeProperty = $methodType.GetProperty('DeclaringType')
$tokenField = $methodType.GetField('token')
$parameterCountField = $methodType.GetField('parameterCount')
$slotField = $methodType.GetField('slot')
$native = [IO.File]::ReadAllBytes((Join-Path $game 'GameAssembly.dll'))
$nativeHash = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($native)).ToLowerInvariant()
if ($nativeHash -ne $inputManifest.inputs[0].sha256) { throw 'Native input drift.' }
$imageBase = [Convert]::ToUInt64($inputManifest.pe.imageBase.Substring(2), 16)
$sections = @($inputManifest.pe.sections | ForEach-Object {
    [pscustomobject]@{ Rva = [Convert]::ToUInt32($_.rva.Substring(2), 16); RawOffset = [Convert]::ToUInt32($_.rawOffset.Substring(2), 16); RawBytes = [uint32]$_.rawBytes }
})
function RawOffset([uint32]$rva, [int]$length) {
    $matches = @($sections | Where-Object { $rva -ge $_.Rva -and ([uint64]$rva + $length) -le ([uint64]$_.Rva + $_.RawBytes) })
    if ($matches.Count -ne 1) { throw ('RVA outside a unique file-backed section: 0x{0:X}' -f $rva) }
    return [int]($matches[0].RawOffset + $rva - $matches[0].Rva)
}
$pe = [BitConverter]::ToUInt32($native, 0x3c)
$optional = $pe + 24
$exceptionRva = [BitConverter]::ToUInt32($native, $optional + 112 + 3 * 8)
$exceptionBytes = [BitConverter]::ToUInt32($native, $optional + 112 + 3 * 8 + 4)
$exceptionRaw = RawOffset $exceptionRva $exceptionBytes
$exceptionCount = [int]($exceptionBytes / 12)
function RuntimeFunction([uint32]$rva) {
    $lo = 0; $hi = $exceptionCount - 1
    while ($lo -le $hi) {
        $middle = [int][Math]::Floor(($lo + $hi) / 2)
        $start = [BitConverter]::ToUInt32($native, $exceptionRaw + $middle * 12)
        $end = [BitConverter]::ToUInt32($native, $exceptionRaw + $middle * 12 + 4)
        if ($rva -lt $start) { $hi = $middle - 1 }
        elseif ($rva -ge $end) { $lo = $middle + 1 }
        else { return [pscustomobject]@{ BeginRva = $start; EndRva = $end; UnwindRva = [BitConverter]::ToUInt32($native, $exceptionRaw + $middle * 12 + 8) } }
    }
    return $null
}
$allPointers = [uint64[]]@([LibCpp2IL.LibCpp2IlMain]::MethodsByPtr.Keys)
[Array]::Sort($allPointers)
function NextPointerIndex([uint64]$pointer) {
    $lo = 0; $hi = $allPointers.Length - 1
    while ($lo -le $hi) {
        $middle = [int][Math]::Floor(($lo + $hi) / 2)
        if ($allPointers[$middle] -le $pointer) { $lo = $middle + 1 } else { $hi = $middle - 1 }
    }
    return $lo
}
function MethodEntry($method) {
    $type = $declaringTypeProperty.GetValue($method)
    $pointer = [uint64]$pointerProperty.GetValue($method)
    [pscustomobject]@{ Type = $type.FullName; Name = $nameProperty.GetValue($method); Token = ('0x{0:X8}' -f $tokenField.GetValue($method)); ParameterCount = $parameterCountField.GetValue($method); InterfaceOrVirtualSlot = $slotField.GetValue($method); Va = ('0x{0:X}' -f $pointer); Rva = $(if ($pointer) { '0x{0:X}' -f ($pointer - $imageBase) } else { '0x0' }); NativePointerPresent = [bool]$pointer }
}
function MethodNames([uint64]$address) {
    $map = [LibCpp2IL.LibCpp2IlMain]::MethodsByPtr
    if (-not $map.ContainsKey($address)) { return @() }
    @($map[$address] | ForEach-Object { $declaringTypeProperty.GetValue($_).FullName + '::' + $nameProperty.GetValue($_) + '/' + $parameterCountField.GetValue($_) })
}
function GlobalName([uint64]$address) {
    try {
        $usage = [LibCpp2IL.LibCpp2IlMain]::GetAnyGlobalByAddress($address)
        if ($null -eq $usage) { return $null }
        $kind = $usage.Type.ToString()
        $value = switch ($kind) {
            'StringLiteral' { $usage.AsLiteral(); break }
            'MethodDef' { $method = $usage.AsMethod(); $declaringTypeProperty.GetValue($method).FullName + '::' + $nameProperty.GetValue($method) + '/' + $parameterCountField.GetValue($method); break }
            'MethodRef' { $usage.AsGenericMethodRef().ToString(); break }
            'TypeInfo' { $usage.AsType().ToString(); break }
            'Type' { $usage.AsType().ToString(); break }
            default { $usage.ToString() }
        }
        return [pscustomobject]@{ Kind = $kind; Value = $value }
    }
    catch { return [pscustomobject]@{ ResolutionError = $_.Exception.Message } }
}
$selection = @()
if ($SelectionFile) { $selection = @(Get-Content -LiteralPath $SelectionFile -Raw | ConvertFrom-Json) }
$selectedKeys = @{}
foreach ($entry in $selection) { $selectedKeys[$entry.Type + '|' + $entry.Token] = $true }
$xrefInput = Get-Content -LiteralPath (Join-Path $root $ReferencesFile) -Raw | ConvertFrom-Json
$xrefRows = [Collections.Generic.List[object]]::new()
$extraTypes = @{}
$extraTypes['ScheduleOne.DevUtilities.PersistentSingleton`1'] = $true
$extraTypes['ScheduleOne.DevUtilities.Singleton`1'] = $true
foreach ($candidate in $xrefInput.candidates) {
    $site = [Convert]::ToUInt64($candidate.va.Substring(2), 16)
    $next = NextPointerIndex $site
    $ownerPointer = $allPointers[$next - 1]
    $owners = @([LibCpp2IL.LibCpp2IlMain]::MethodsByPtr[$ownerPointer] | ForEach-Object { MethodEntry $_ })
    foreach ($owner in $owners) { $extraTypes[$owner.Type] = $true }
    $xrefRows.Add([pscustomobject]@{ Candidate = $candidate; NearestPreviousMethodPointer = ('0x{0:X}' -f $ownerPointer); OwnerCandidates = $owners; Limit = 'Nearest parser pointer is only a candidate owner until a decoded containing window confirms the transfer.' })
}
foreach ($entry in $selection) { $extraTypes[$entry.Type] = $true }
$types = @($metadata.typeDefs | Where-Object {
    $_.FullName -in @('ScheduleOne.Property.Business','ScheduleOne.Property.Property','ScheduleOne.Property.PropertyContentsContainer','ScheduleOne.Persistence.Loaders.BusinessLoader','ScheduleOne.Persistence.Loaders.BusinessesLoader','ScheduleOne.Property.BusinessManager','ScheduleOne.Property.PropertyManager','FishNet.Object.NetworkBehaviour','ScheduleOne.GameTime.TimeManager','ScheduleOne.Money.MoneyManager')
})
$bodyNames = @('Awake','Start','OnDestroy','InitializeSaveable','NetworkInitialize___Early','NetworkInitialize__Late','NetworkInitializeIfDisabled','Awake_UserLogic','AwakeUserLogic','Method_Protected_Virtual_Void_0','.ctor','Load','SetProperty','get_currentLaunderTotal','get_appliedLaunderLimit','GetSaveString','DoBoundsContainPoint','IsPointInsideBox','GetBusiness','LoadBusiness')
$fieldMap = [Collections.Generic.List[object]]::new()
$methodMap = [Collections.Generic.List[object]]::new()
$windows = [Collections.Generic.List[object]]::new()
$outputDir = Join-Path $root ('native-disassembly-' + $OutputTag)
if ($true) {
    if (Test-Path -LiteralPath $outputDir) { throw 'Output must be new; preserve previous evidence.' }
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}
foreach ($type in $types) {
    $number = 0
    foreach ($field in $type.Fields) {
        $isStatic = [bool]($field.RawFieldType.Attrs -band 0x10)
        $offset = [LibCpp2IL.LibCpp2IlMain]::Binary.GetFieldOffsetFromIndex($type.TypeIndex, $number, $field.FieldIndex, $type.IsValueType, $isStatic)
        $fieldMap.Add([pscustomobject]@{ Type = $type.FullName; Name = $field.Name; Static = $isStatic; Offset = ('0x{0:X}' -f $offset) })
        $number++
    }
    foreach ($method in $type.Methods) {
        $entry = MethodEntry $method
        $methodMap.Add($entry)
        if ($entry.Name -notin $bodyNames -and $entry.Name -notlike '*Awake*') { continue }
        if (-not $entry.NativePointerPresent) { throw 'Selected method has no native pointer.' }
        $pointer = [Convert]::ToUInt64($entry.Va.Substring(2), 16)
        $rva = [uint32]($pointer - $imageBase)
        $next = NextPointerIndex $pointer
        $end = $allPointers[$next]
        $length = [int]($end - $pointer)
        if ($length -lt 1 -or $length -gt 65536) { throw 'Unexpected pointer-window length.' }
        $raw = RawOffset $rva $length
        $bytes = [byte[]]::new($length)
        [Array]::Copy($native, $raw, $bytes, 0, $length)
        $decoder = [Iced.Intel.Decoder]::Create(64, $bytes, $pointer, [Iced.Intel.DecoderOptions]::None)
        $instructions = [Collections.Generic.List[object]]::new()
        $text = [Collections.Generic.List[string]]::new()
        $undecodedTail = $null
        $text.Add(('; {0}::{1}/{2}, token {3}, RVA {4}, next-pointer window 0x{5:X} bytes' -f $entry.Type, $entry.Name, $entry.ParameterCount, $entry.Token, $entry.Rva, $length))
        $text.Add('; No target execution. Parser-pointer window can contain padding/cold fragments and is not exact/full function, EH or transitive coverage. Shared direct aliases are truncated to eight names.')
        while ($decoder.IP -lt $end) {
            $instruction = $decoder.Decode()
            if ($decoder.LastError -ne [Iced.Intel.DecoderError]::None -or $instruction.Length -le 0) {
                # A pointer window may include jump-table/data bytes. Do not invent instructions or silently drop them.
                $tailStart = [uint64]$instruction.IP
                $tailLength = [int]($end - $tailStart)
                $undecodedTail = [pscustomobject]@{ Va = ('0x{0:X}' -f $tailStart); Rva = ('0x{0:X}' -f ($tailStart - $imageBase)); Bytes = [BitConverter]::ToString($bytes, [int]($tailStart - $pointer), $tailLength).Replace('-', ' '); Length = $tailLength; Reason = 'Linear decode stopped at invalid bytes; suffix may contain data/jump tables or unanalysed code. No instruction/coverage claim for this suffix.' }
                $text.Add(('; UNDECODED SUFFIX at {0}, {1} bytes: {2}' -f $undecodedTail.Rva, $tailLength, $undecodedTail.Reason))
                $text.Add('; ' + $undecodedTail.Bytes)
                break
            }
            $flow = $instruction.FlowControl.ToString()
            $target = $null; $aliases = @(); $aliasCount = 0
            if ($flow -in @('Call', 'UnconditionalBranch', 'ConditionalBranch')) {
                $target = [uint64]$instruction.NearBranchTarget
                $aliases = @(MethodNames $target)
                $aliasCount = $aliases.Count
                if ($aliasCount -gt 8) { $aliases = $aliases[0..7] }
            }
            $candidate = $instruction.IsIPRelativeMemoryOperand -and (($instruction.IPRelativeMemoryAddress % 8) -eq 0) -and ($instruction.Mnemonic -eq [Iced.Intel.Mnemonic]::Lea -or $instruction.MemorySize -eq [Iced.Intel.MemorySize]::UInt64)
            $global = if ($candidate) { GlobalName $instruction.IPRelativeMemoryAddress } else { $null }
            $row = [pscustomobject]@{ Va = ('0x{0:X}' -f $instruction.IP); Rva = ('0x{0:X}' -f ($instruction.IP - $imageBase)); Bytes = [BitConverter]::ToString($bytes, [int]($instruction.IP - $pointer), $instruction.Length).Replace('-', ' '); Text = $instruction.ToString(); FlowControl = $flow; DirectTarget = $(if ($null -ne $target) { '0x{0:X}' -f $target } else { $null }); TargetMethodCount = $aliasCount; TargetMethods = $aliases; RipAddress = $(if ($instruction.IsIPRelativeMemoryOperand) { '0x{0:X}' -f $instruction.IPRelativeMemoryAddress } else { $null }); MetadataGlobal = $global }
            $instructions.Add($row)
            $suffix = if ($aliases.Count) { ' ; ' + ($aliases -join ' | ') } elseif ($global -and $global.Value) { ' ; ' + $global.Kind + ': ' + $global.Value } else { '' }
            if ($aliasCount -gt 8) { $suffix += (' | ... SHARED TARGET: {0} aliases' -f $aliasCount) }
            $text.Add(('{0,-12} {1,-31} {2}{3}' -f $row.Rva, $row.Bytes, $row.Text, $suffix))
        }
        $shortType = ($type.Name -replace '[^A-Za-z0-9_-]', '_')
        if ($shortType.Length -gt 36) { $shortType = $shortType.Substring(0, 36) }
        $shortMethod = ($entry.Name -replace '[^A-Za-z0-9_-]', '_')
        if ($shortMethod.Length -gt 42) { $shortMethod = $shortMethod.Substring(0, 42) }
        $filename = $entry.Token + '-' + $shortType + '-' + $shortMethod
        $window = [pscustomobject]@{ Method = $entry; PrimaryPdataBounds = RuntimeFunction $rva; WindowEndVa = ('0x{0:X}' -f $end); WindowKind = 'Next distinct parser method pointer, not an exact/full function extent'; RawOffset = ('0x{0:X}' -f $raw); ByteLength = $length; BytesSha256 = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant(); Instructions = $instructions.ToArray(); UndecodedTail = $undecodedTail }
        [IO.File]::WriteAllText((Join-Path $outputDir ($filename + '.json')), ($window | ConvertTo-Json -Depth 14), [Text.UTF8Encoding]::new($false))
        [IO.File]::WriteAllLines((Join-Path $outputDir ($filename + '.txt')), $text, [Text.UTF8Encoding]::new($false))
        $windows.Add([pscustomobject]@{ Method = $entry; ByteLength = $length; BytesSha256 = $window.BytesSha256; File = $filename; Instructions = $instructions.Count; HasUndecodedTail = ($null -ne $undecodedTail) })
    }
}

$record = [pscustomobject]@{ CapturedUtc = [DateTime]::UtcNow.ToString('o'); NativeSha256 = $nativeHash; MetadataVersion = [LibCpp2IL.LibCpp2IlMain]::MetadataVersion; GlobalMethodCount = $metadata.methodDefs.Length; Method = 'Fresh LibCpp2IL mapping + Iced native-byte decode, target assemblies never loaded/invoked'; FieldMap = $fieldMap.ToArray(); MethodMap = $methodMap.ToArray(); RawReferenceOwnerCandidates = $xrefRows.ToArray(); Disassembled = $windows.ToArray(); NativeSaveWriteAllowed = $false; GameInvoked = $false; NativeSaveFilesRead = $false }
$output = Join-Path $root ('native-map-' + $OutputTag + '.json')
if (Test-Path -LiteralPath $output) { throw 'Mapping output must be new.' }
[IO.File]::WriteAllText($output, ($record | ConvertTo-Json -Depth 15), [Text.UTF8Encoding]::new($false))
[pscustomobject]@{ MappedMethods = $methodMap.Count; MappedFields = $fieldMap.Count; NativeWindows = $windows.Count; RawReferenceCandidates = $xrefRows.Count; NativeSaveWriteAllowed = $false } | ConvertTo-Json
