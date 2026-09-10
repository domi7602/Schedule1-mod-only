# Baut alle Mods (Release) und deployt sie nach <GameDir>\Mods\.
# Nutzung: pwsh Tools/build-all.ps1 [-Config Debug]
param(
    [string]$Config = "Release"
)

$ErrorActionPreference = "Stop"
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modRoot = Join-Path $workspaceRoot "Source\Mods"
$sln = Join-Path $modRoot "S1Mods.sln"

if (-not (Test-Path -LiteralPath $sln)) {
    Write-Host "Solution fehlt — wird generiert..." -ForegroundColor Yellow
    & (Join-Path $PSScriptRoot "gen-sln.ps1")
}

Write-Host "Building $sln ($Config)..." -ForegroundColor Cyan
dotnet build $sln -c $Config --nologo
if ($LASTEXITCODE -ne 0) {
    throw "Build fehlgeschlagen (exit $LASTEXITCODE)."
}
Write-Host "Build OK." -ForegroundColor Green
