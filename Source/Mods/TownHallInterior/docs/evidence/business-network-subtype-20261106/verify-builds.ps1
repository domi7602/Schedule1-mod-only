$ErrorActionPreference = 'Stop'
$repo = 'C:/Users/pc/Schedule1-mod-only'
$scratch = "$repo/.scratch/townhall-business-bind-20261106"
$project = "$repo/Source/Mods/TownHallInterior/src/TownHallInterior.csproj"
$results = @()
$modes = @(
    @{ Name = 'Normal'; Flag = '' },
    @{ Name = 'Subtype'; Flag = 'S1TownHallBusinessSubtype' },
    @{ Name = 'Interface'; Flag = 'S1TownHallBusinessInterface' },
    @{ Name = 'Manager'; Flag = 'S1TownHallBusinessManager' },
    @{ Name = 'ActiveSubtype'; Flag = 'S1TownHallBusinessActiveSubtype' },
    @{ Name = 'NetworkSubtype'; Flag = 'S1TownHallBusinessNetworkSubtype' }
)
foreach ($mode in $modes) {
    if (@(Get-Process -Name 'Schedule I','ScheduleI' -ErrorAction SilentlyContinue).Count) { throw 'Game running' }
    $arguments = @('build', $project, '-c', 'Release', '-p:S1NoDeploy=true', '--no-restore', '-warnaserror')
    if ($mode.Flag) { $arguments += "-p:$($mode.Flag)=true" }
    $log = "$scratch/build-$($mode.Name).log.txt"
    & dotnet @arguments > $log 2>&1
    $code = $LASTEXITCODE
    if ($code -ne 0) { Get-Content -LiteralPath $log -Tail 35; throw "Build $($mode.Name) failed ($code)" }
    $target = "$scratch/artifacts/$($mode.Name)"
    New-Item -ItemType Directory -Force -Path $target | Out-Null
    foreach ($name in @('TownHallInterior.dll', 'TownHallInterior.pdb')) {
        Copy-Item -LiteralPath "$repo/Source/Mods/TownHallInterior/src/bin/Release/net6.0/$name" -Destination "$target/$name"
    }
    $results += [ordered]@{ mode = $mode.Name; exitCode = $code; dllSha256 = (Get-FileHash -LiteralPath "$target/TownHallInterior.dll" -Algorithm SHA256).Hash.ToLowerInvariant(); pdbSha256 = (Get-FileHash -LiteralPath "$target/TownHallInterior.pdb" -Algorithm SHA256).Hash.ToLowerInvariant(); noDeploy = $true }
    Get-Content -LiteralPath $log -Tail 6
}
$results | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath "$scratch/build-results.json" -Encoding utf8NoBOM
foreach ($guard in @('LegacyActivation', 'NetworkManager', 'NetworkActive')) {
    if (@(Get-Process -Name 'Schedule I','ScheduleI' -ErrorAction SilentlyContinue).Count) { throw 'Game running' }
    $arguments = @('build', $project, '-c', 'Release', '-p:S1NoDeploy=true', '--no-restore')
    if ($guard -eq 'LegacyActivation') { $arguments += '-p:S1TownHallBusinessActivation=true' }
    elseif ($guard -eq 'NetworkManager') { $arguments += @('-p:S1TownHallBusinessNetworkSubtype=true', '-p:S1TownHallBusinessManager=true') }
    else { $arguments += @('-p:S1TownHallBusinessNetworkSubtype=true', '-p:S1TownHallBusinessActiveSubtype=true') }
    $log = "$scratch/build-$guard-blocked.log.txt"
    & dotnet @arguments > $log 2>&1
    if ($LASTEXITCODE -eq 0 -or -not (Select-String -LiteralPath $log -SimpleMatch 'error CS1029')) { throw "Build guard $guard not confirmed" }
}
Write-Output 'Six no-deploy variants passed; legacy activation and mixed network/manager and network/active modes blocked by CS1029.'
