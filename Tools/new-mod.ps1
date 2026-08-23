# Legt ein neues Mod-Gerüst an: Source/Mods/<Name>/src/ + docs/.
# Nutzung: pwsh Tools/new-mod.ps1 -Name "MyMod" -Author "Dominik" -Version "0.1.0"
param(
    [Parameter(Mandatory = $true)][string]$Name,
    [string]$Author = "Dominik",
    [string]$Version = "0.1.0"
)

$ErrorActionPreference = "Stop"
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modRoot = Join-Path $workspaceRoot "Source\Mods"
$modDir = Join-Path $modRoot $Name
$srcDir = Join-Path $modDir "src"
$docsDir = Join-Path $modDir "docs"

if (Test-Path -LiteralPath $modDir) {
    throw "Mod-Ordner existiert bereits: $modDir"
}

New-Item -ItemType Directory -Path $srcDir -Force | Out-Null
New-Item -ItemType Directory -Path $docsDir -Force | Out-Null

$ns = $Name -replace '[^A-Za-z0-9_]', ''
$csproj = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <AssemblyName>$Name</AssemblyName>
    <RootNamespace>$ns</RootNamespace>
  </PropertyGroup>
</Project>
"@

$modCs = @"
using MelonLoader;

[assembly: MelonInfo(typeof($ns.Mod), "$Name", "$Version", "$Author")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace $ns;

public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("[$Name] initialisiert (v$Version).");
    }
}
"@

$modJson = @"
{
  "name": "$Name",
  "author": "$Author",
  "version": "$Version",
  "description": "Noch keine Beschreibung."
}
"@

$readme = "# $Name`n`nTODO: Beschreibung."
$changelog = "# Changelog`n`n## $Version`n- Initiale Version."

Set-Content -LiteralPath (Join-Path $srcDir "$Name.csproj") -Value $csproj -Encoding UTF8
Set-Content -LiteralPath (Join-Path $srcDir "Mod.cs") -Value $modCs -Encoding UTF8
Set-Content -LiteralPath (Join-Path $docsDir "mod.json") -Value $modJson -Encoding UTF8
Set-Content -LiteralPath (Join-Path $docsDir "README.md") -Value $readme -Encoding UTF8
Set-Content -LiteralPath (Join-Path $docsDir "CHANGELOG.md") -Value $changelog -Encoding UTF8

Write-Host "Mod '$Name' angelegt unter $modDir" -ForegroundColor Green
Write-Host "Danach: pwsh Tools/gen-sln.ps1 (Solution regenerieren)"
