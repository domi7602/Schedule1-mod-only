$ErrorActionPreference = 'Stop'
$root = (Resolve-Path '.scratch/townhall-native-save-audit-20261105').Path
$bundle = Join-Path $root 'tool-bundle'
$inputManifest = Get-Content -LiteralPath (Join-Path $root 'input-manifest.json') -Raw | ConvertFrom-Json
$game = $inputManifest.gameDirectory
$loadedTools = @()
foreach ($name in @('AssetRipper.Primitives.dll', 'WasmDisassembler.dll', 'Disarm.dll', 'Iced.dll', 'LibCpp2IL.dll')) {
    $file = Join-Path $bundle $name
    $assembly = [System.Reflection.Assembly]::LoadFrom($file)
    $loadedTools += [pscustomobject]@{ Name = $name; Identity = $assembly.FullName; Sha256 = (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant() }
}
# Only trusted analysis libraries are loaded. GameAssembly is read as bytes, never Assembly.Load/LoadLibrary/invoked.
[LibCpp2IL.LibCpp2IlMain]::Settings.AllowManualMetadataAndCodeRegInput = $false
[LibCpp2IL.LibCpp2IlMain]::Settings.DisableMethodPointerMapping = $false
$version = [AssetRipper.Primitives.UnityVersion]::Parse('2022.3.62f2')
$ok = [LibCpp2IL.LibCpp2IlMain]::LoadFromFile((Join-Path $game 'GameAssembly.dll'), (Join-Path $game 'Schedule I_Data/il2cpp_data/Metadata/global-metadata.dat'), $version)
if (-not $ok) { throw 'Native metadata mapping failed.' }
$metadata = [LibCpp2IL.LibCpp2IlMain]::TheMetadata
$methodDefinitionType = [LibCpp2IL.Metadata.Il2CppMethodDefinition]
$pointerProperty = $methodDefinitionType.GetProperty('MethodPointer')
$nameProperty = $methodDefinitionType.GetProperty('Name')
$declaringTypeProperty = $methodDefinitionType.GetProperty('DeclaringType')
$tokenField = $methodDefinitionType.GetField('token')
$parameterCountField = $methodDefinitionType.GetField('parameterCount')
$slotField = $methodDefinitionType.GetField('slot')
$fieldOffsetMethod = [LibCpp2IL.LibCpp2IlMain].GetField('Binary').FieldType.GetMethods() | Where-Object { $_.Name -eq 'GetFieldOffsetFromIndex' }
Write-Output ('Field-offset API parameters: ' + (($fieldOffsetMethod.GetParameters() | ForEach-Object { $_.Name }) -join ', '))
$allPointers = [uint64[]]@([LibCpp2IL.LibCpp2IlMain]::MethodsByPtr.Keys)
[Array]::Sort($allPointers)
$native = [IO.File]::ReadAllBytes((Join-Path $game 'GameAssembly.dll'))
$imageBase = [Convert]::ToUInt64($inputManifest.pe.imageBase.Substring(2), 16)
$sections = @($inputManifest.pe.sections | ForEach-Object {
    [pscustomobject]@{ Rva = [Convert]::ToUInt32($_.rva.Substring(2), 16); RawOffset = [Convert]::ToUInt32($_.rawOffset.Substring(2), 16); RawBytes = [uint32]$_.rawBytes }
})
function RawOffset([uint32]$rva, [int]$length) {
    $matches = @($sections | Where-Object { $rva -ge $_.Rva -and ([uint64]$rva + $length) -le ([uint64]$_.Rva + $_.RawBytes) })
    if ($matches.Count -ne 1) { throw ('RVA outside unique file-backed section: 0x{0:X}' -f $rva) }
    return [int]($matches[0].RawOffset + $rva - $matches[0].Rva)
}
$pe = [BitConverter]::ToUInt32($native, 0x3c)
$optional = $pe + 24
$exceptionRva = [BitConverter]::ToUInt32($native, $optional + 112 + 3 * 8)
$exceptionBytes = [BitConverter]::ToUInt32($native, $optional + 112 + 3 * 8 + 4)
if ($exceptionBytes % 12) { throw 'Unexpected x64 exception-directory size.' }
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
function MethodNames([uint64]$address) {
    $map = [LibCpp2IL.LibCpp2IlMain]::MethodsByPtr
    if (-not $map.ContainsKey($address)) { return @() }
    return @($map[$address] | ForEach-Object { $declaringTypeProperty.GetValue($_).FullName + '::' + $nameProperty.GetValue($_) + '/' + $parameterCountField.GetValue($_) })
}
function GlobalName([uint64]$address) {
    try {
        $usage = [LibCpp2IL.LibCpp2IlMain]::GetAnyGlobalByAddress($address)
        if ($null -eq $usage) { return $null }
        $kind = $usage.Type.ToString()
        $value = switch ($kind) {
            'StringLiteral' { $usage.AsLiteral(); break }
            'MethodDef' {
                $definition = $usage.AsMethod()
                $declaringTypeProperty.GetValue($definition).FullName + '::' + $nameProperty.GetValue($definition) + '/' + $parameterCountField.GetValue($definition)
                break
            }
            'MethodRef' { $usage.AsGenericMethodRef().ToString(); break }
            'TypeInfo' { $usage.AsType().ToString(); break }
            'Type' { $usage.AsType().ToString(); break }
            'FieldInfo' { $usage.ToString(); break }
            default { $usage.ToString() }
        }
        return [pscustomobject]@{ Kind = $kind; Value = $value }
    }
    catch { return [pscustomobject]@{ ResolutionError = $_.Exception.Message } }
}
$types = @($metadata.typeDefs | Where-Object {
    $_.FullName.StartsWith('ScheduleOne.Persistence.SaveManager') -or $_.FullName -eq 'ScheduleOne.Persistence.SaveRequest' -or $_.FullName -eq 'ScheduleOne.Persistence.ISaveable'
})
$methodMap = [Collections.Generic.List[object]]::new()
$nativeMethods = [Collections.Generic.List[object]]::new()
$fieldMap = [Collections.Generic.List[object]]::new()
$bodyNames = @('Save', 'MoveNext', 'Complete', 'CompleteSaveable', 'ClearCompletedSaveable', 'QueueSaveRequest', 'DequeueSaveRequest', 'RegisterSaveable', 'Clean', 'ReportSaveError', 'WriteBaseData', 'CompleteSave', 'WriteData', 'WriteFolder', 'WriteSubfile', 'DeleteUnapprovedFiles', 'Awake', 'Start', 'Update', '.ctor')
$outputDir = Join-Path $root 'native-disassembly-v4'
if (Test-Path -LiteralPath $outputDir) { throw 'New analysis output directory required.' }
New-Item -ItemType Directory -Path $outputDir | Out-Null
foreach ($type in $types) {
    $fieldNumber = 0
    foreach ($field in $type.Fields) {
        $isStatic = [bool]($field.RawFieldType.Attrs -band 0x10)
        $offset = [LibCpp2IL.LibCpp2IlMain]::Binary.GetFieldOffsetFromIndex($type.TypeIndex, $fieldNumber, $field.FieldIndex, $type.IsValueType, $isStatic)
        $fieldMap.Add([pscustomobject]@{ Type = $type.FullName; Name = $field.Name; IndexInType = $fieldNumber; Static = $isStatic; Offset = ('0x{0:X}' -f $offset) })
        $fieldNumber++
    }
    foreach ($method in $type.Methods) {
        # Exact reflection avoids LibCpp2IL's methodIndex/MethodIndex casing collision in PowerShell's adapter.
        $pointer = [uint64]$pointerProperty.GetValue($method)
        $methodName = $nameProperty.GetValue($method)
        $rva = if ($pointer) { [uint32]($pointer - $imageBase) } else { 0 }
        $entry = [pscustomobject]@{ Type = $type.FullName; Name = $methodName; Token = ('0x{0:X8}' -f $tokenField.GetValue($method)); ParameterCount = $parameterCountField.GetValue($method); InterfaceOrVirtualSlot = $slotField.GetValue($method); Va = ('0x{0:X}' -f $pointer); Rva = ('0x{0:X}' -f $rva); NativePointerPresent = [bool]$pointer }
        $methodMap.Add($entry)
        if (-not $pointer -or $methodName -notin $bodyNames) { continue }
        $bounds = RuntimeFunction $rva
        if ($null -eq $bounds -or $bounds.BeginRva -ne $rva) { continue }
        $lo = 0; $hi = $allPointers.Length - 1
        while ($lo -le $hi) {
            $middle = [int][Math]::Floor(($lo + $hi) / 2)
            if ($allPointers[$middle] -le $pointer) { $lo = $middle + 1 } else { $hi = $middle - 1 }
        }
        if ($lo -ge $allPointers.Length) { throw 'No upper metadata-pointer boundary.' }
        $nextPointer = $allPointers[$lo]
        $length = [int]($nextPointer - $pointer)
        if ($length -le 0 -or $length -gt 65536) { throw 'Unexpected method extent.' }
        $raw = RawOffset $rva $length
        $bytes = [byte[]]::new($length)
        [Array]::Copy($native, $raw, $bytes, 0, $length)
        $decoder = [Iced.Intel.Decoder]::Create(64, $bytes, $pointer, [Iced.Intel.DecoderOptions]::None)
        $instructions = [Collections.Generic.List[object]]::new()
        $text = [Collections.Generic.List[string]]::new()
        $text.Add(('; {0}::{1}/{2}, token {3}, RVA {4}, metadata-pointer window 0x{5:X} bytes' -f $entry.Type, $entry.Name, $entry.ParameterCount, $entry.Token, $entry.Rva, $length))
        $text.Add('; Raw native bytes only. Window ends at next distinct managed-method pointer, includes padding/cold fragments; not proof of full transitive/exception coverage. Shared target names show at most eight aliases; total count is recorded.')
        while ($decoder.IP -lt ($pointer + $length)) {
            $instruction = $decoder.Decode()
            if ($decoder.LastError -ne [Iced.Intel.DecoderError]::None -or $instruction.Length -le 0) { throw ('Decode failed in {0} at 0x{1:X}' -f $entry.Token, $instruction.IP) }
            $flow = $instruction.FlowControl.ToString()
            $targets = @()
            $targetCount = 0
            $target = $null
            if ($flow -in @('Call', 'UnconditionalBranch', 'ConditionalBranch')) {
                $target = [uint64]$instruction.NearBranchTarget
                $targets = @(MethodNames $target)
                $targetCount = $targets.Count
                if ($targetCount -gt 8) { $targets = $targets[0..7] }
            }
            # A random byte/flag can accidentally decode as a valid metadata usage. Annotate only aligned pointer-sized references or LEA.
            $metadataCandidate = $instruction.IsIPRelativeMemoryOperand -and (($instruction.IPRelativeMemoryAddress % 8) -eq 0) -and ($instruction.Mnemonic -eq [Iced.Intel.Mnemonic]::Lea -or $instruction.MemorySize -eq [Iced.Intel.MemorySize]::UInt64)
            $global = if ($metadataCandidate) { GlobalName $instruction.IPRelativeMemoryAddress } else { $null }
            $row = [pscustomobject]@{ Va = ('0x{0:X}' -f $instruction.IP); Rva = ('0x{0:X}' -f ($instruction.IP - $imageBase)); Bytes = [BitConverter]::ToString($bytes, [int]($instruction.IP - $pointer), $instruction.Length).Replace('-', ' '); Text = $instruction.ToString(); FlowControl = $flow; DirectTarget = $(if ($null -ne $target) { '0x{0:X}' -f $target } else { $null }); TargetMethodCount = $targetCount; TargetMethods = $targets; RipAddress = $(if ($instruction.IsIPRelativeMemoryOperand) { '0x{0:X}' -f $instruction.IPRelativeMemoryAddress } else { $null }); MetadataGlobal = $global }
            $instructions.Add($row)
            $suffix = if ($targets.Count) { ' ; ' + ($targets -join ' | ') } elseif ($null -ne $global -and $global.Value) { ' ; ' + $global.Kind + ': ' + $global.Value } else { '' }
            if ($targetCount -gt 8) { $suffix += (' | ... SHARED TARGET: {0} method aliases total' -f $targetCount) }
            $text.Add(('{0,-12} {1,-31} {2}{3}' -f $row.Rva, $row.Bytes, $row.Text, $suffix))
        }
        $filename = $entry.Token + '-' + ($entry.Name -replace '[^A-Za-z0-9_-]', '_')
        [IO.File]::WriteAllLines((Join-Path $outputDir ($filename + '.txt')), $text, [Text.UTF8Encoding]::new($false))
        $methodOutput = [pscustomobject]@{ Method = $entry; PrimaryPdataBounds = $bounds; WindowEndVa = ('0x{0:X}' -f $nextPointer); WindowKind = 'Next distinct managed-method pointer; not claimed to be exact function extent'; RawOffset = ('0x{0:X}' -f $raw); ByteLength = $length; BytesSha256 = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant(); Instructions = $instructions.ToArray() }
        [IO.File]::WriteAllText((Join-Path $outputDir ($filename + '.json')), ($methodOutput | ConvertTo-Json -Depth 12), [Text.UTF8Encoding]::new($false))
        $nativeMethods.Add([pscustomobject]@{ Method = $entry; ByteLength = $length; Instructions = $instructions.Count; BytesSha256 = $methodOutput.BytesSha256; TextFile = ($filename + '.txt') })
    }
}
$result = [pscustomobject]@{ CapturedUtc = [DateTime]::UtcNow.ToString('o'); Method = 'Fresh LibCpp2IL metadata/pointer/field-offset parsing; Iced decoding to next distinct managed-method pointer, includes cold fragments/padding, no game execution'; MetadataVersion = [LibCpp2IL.LibCpp2IlMain]::MetadataVersion; GlobalMetadataMethodCount = $metadata.methodDefs.Length; Types = @($types | ForEach-Object { $_.FullName }); AnalysisToolAssemblies = $loadedTools; FieldMap = $fieldMap.ToArray(); MethodMap = $methodMap.ToArray(); Disassembled = $nativeMethods.ToArray(); NativeSaveRouteAllowed = $false; GameInvoked = $false; SaveFilesAccessed = $false }
[IO.File]::WriteAllText((Join-Path $root 'native-map-v4.json'), ($result | ConvertTo-Json -Depth 12), [Text.UTF8Encoding]::new($false))
[pscustomobject]@{ MetadataVersion = $result.MetadataVersion; MethodsInMetadata = $result.GlobalMetadataMethodCount; SelectedMethods = $methodMap.Count; DisassembledMethods = $nativeMethods.Count; GameInvoked = $false; NativeSaveRouteAllowed = $false } | ConvertTo-Json
