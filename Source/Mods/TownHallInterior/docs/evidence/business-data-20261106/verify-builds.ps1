$ErrorActionPreference = 'Stop'
$repo = 'C:/Users/pc/Schedule1-mod-only'
$scratch = "$repo/.scratch/townhall-business-data-20261106"
$project = "$repo/Source/Mods/TownHallInterior/src/TownHallInterior.csproj"
$results = @()
$modes = @(
    @{ Name = 'Normal'; Flag = '' },
    @{ Name = 'Subtype'; Flag = 'S1TownHallBusinessSubtype' },
    @{ Name = 'Interface'; Flag = 'S1TownHallBusinessInterface' },
    @{ Name = 'Manager'; Flag = 'S1TownHallBusinessManager' },
    @{ Name = 'ActiveSubtype'; Flag = 'S1TownHallBusinessActiveSubtype' },
    @{ Name = 'NetworkSubtype'; Flag = 'S1TownHallBusinessNetworkSubtype' },
    @{ Name = 'Data'; Flag = 'S1TownHallBusinessData' }
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
    $results += [ordered]@{ mode=$mode.Name; exitCode=$code; dllSha256=(Get-FileHash -LiteralPath "$target/TownHallInterior.dll" -Algorithm SHA256).Hash.ToLowerInvariant(); pdbSha256=(Get-FileHash -LiteralPath "$target/TownHallInterior.pdb" -Algorithm SHA256).Hash.ToLowerInvariant(); noDeploy=$true }
    Write-Output "$($mode.Name): passed"
}
$results | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath "$scratch/build-results.json" -Encoding utf8NoBOM
$guards = @(
    @{ Name='LegacyActivation'; Flags=@('S1TownHallBusinessActivation') },
    @{ Name='NetworkManager'; Flags=@('S1TownHallBusinessNetworkSubtype','S1TownHallBusinessManager') },
    @{ Name='NetworkActive'; Flags=@('S1TownHallBusinessNetworkSubtype','S1TownHallBusinessActiveSubtype') }
)
foreach ($other in @('Subtype','Interface','Manager','ActiveSubtype','NetworkSubtype','Construction')) {
    $guards += @{ Name="Data$other"; Flags=@('S1TownHallBusinessData',"S1TownHallBusiness$other") }
}
$rejected = @()
foreach ($guard in $guards) {
    if (@(Get-Process -Name 'Schedule I','ScheduleI' -ErrorAction SilentlyContinue).Count) { throw 'Game running' }
    $arguments = @('build', $project, '-c', 'Release', '-p:S1NoDeploy=true', '--no-restore')
    foreach ($flag in $guard.Flags) { $arguments += "-p:${flag}=true" }
    $log = "$scratch/build-$($guard.Name)-blocked.log.txt"
    & dotnet @arguments > $log 2>&1
    $code = $LASTEXITCODE
    if ($code -eq 0 -or -not (Select-String -LiteralPath $log -SimpleMatch 'error CS1029')) { throw "Build guard $($guard.Name) not confirmed" }
    $rejected += [ordered]@{ mode=$guard.Name; exitCode=$code; error='CS1029' }
}
$rejected | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath "$scratch/build-guards.json" -Encoding utf8NoBOM
Write-Output 'Seven no-deploy variants passed; nine incompatible/withdrawn routes rejected by CS1029.'
