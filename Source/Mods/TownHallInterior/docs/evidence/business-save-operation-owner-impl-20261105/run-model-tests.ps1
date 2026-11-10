#Requires -Version 7
# Offline linked-source tests only; never builds a Mod project or reads an installation.
[CmdletBinding()]
param([Parameter(Mandatory)][string]$Name, [string]$Filter)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$project = Join-Path $repo 'Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj'
$files = @('Source/Mods/TownHallInterior/src/BusinessConstruction.cs',
    'Source/Tests/TownHallInterior.Tests/BusinessSaveOperationOwnerTests.cs',
    'Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj')
$before = @($files | ForEach-Object { [ordered]@{ Path = $_; Sha256 = (Get-FileHash (Join-Path $repo $_) -Algorithm SHA256).Hash.ToLowerInvariant() } })
if ($Name -notmatch '^[a-z0-9-]+$') { throw 'Use a stable lowercase run name.' }
$run = Join-Path $PSScriptRoot $Name
if (Test-Path -LiteralPath $run) { throw "Never overwrite a test run: $run" }
$null = New-Item -ItemType Directory -Path $run
$start = [DateTime]::UtcNow
$gameCount = @(Get-Process | Where-Object { $_.ProcessName -in @('Schedule I', 'ScheduleI') }).Count
if ($gameCount -ne 0) { throw 'Game must remain closed for these checks.' }
if ($env:DOTNET_ROOT) { throw 'Unexpected DOTNET_ROOT override; inspect rather than silently changing it.' }
$sdk = (& dotnet --version).Trim()
if ($LASTEXITCODE -ne 0 -or $sdk -notmatch '^8\.') { throw "Expected .NET 8 SDK; got $sdk" }
if (-not (Test-Path -LiteralPath (Join-Path (Split-Path $project -Parent) 'obj/project.assets.json'))) { throw 'Existing offline restore assets missing.' }
$arguments = @('test', $project, '--no-restore', '-c', 'Release', '-p:S1NoDeploy=true',
    '--results-directory', $run, '--logger', 'trx;LogFileName=results.trx')
if ($Filter) { $arguments += @('--filter', $Filter) }
& dotnet @arguments 2>&1 | Tee-Object -FilePath (Join-Path $run 'output.log') | Out-Host
$code = $LASTEXITCODE
$unchanged = $true
foreach ($entry in $before) {
    if ((Get-FileHash (Join-Path $repo $entry.Path) -Algorithm SHA256).Hash.ToLowerInvariant() -ne $entry.Sha256) { $unchanged = $false }
}
$counters = $null
$trx = Join-Path $run 'results.trx'
if (Test-Path -LiteralPath $trx) {
    [xml]$xml = Get-Content -LiteralPath $trx -Raw
    $node = $xml.SelectSingleNode("/*[local-name()='TestRun']/*[local-name()='ResultSummary']/*[local-name()='Counters']")
    if ($node) { $counters = [ordered]@{ Total = [int]$node.total; Executed = [int]$node.executed; Passed = [int]$node.passed; Failed = [int]$node.failed } }
}
$status = if ($code -eq 0 -and $unchanged -and $counters -and $counters.Executed -gt 0 -and $counters.Failed -eq 0) { 'passed' } else { 'failed' }
[ordered]@{
    Schema = 1; StartedUtc = $start.ToString('o'); FinishedUtc = [DateTime]::UtcNow.ToString('o')
    Kind = 'Synthetic managed linked-source tests; no native or deployment evidence'
    Sdk = $sdk; GameProcessCount = $gameCount; DotnetRoot = $env:DOTNET_ROOT
    Command = 'dotnet'; Arguments = $arguments; SourceHashes = $before
    ExitCode = $code; SourcesUnchangedDuringRun = $unchanged; Counters = $counters; Status = $status
    NativeSaveWriteAllowed = $false; InstallationReadOrChanged = $false
} | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $run 'report.json') -Encoding utf8NoBOM
if ($status -ne 'passed') { exit 1 }
exit 0
