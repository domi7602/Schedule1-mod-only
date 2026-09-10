# Nutzung: pwsh Tools/gen-sln.ps1
$ErrorActionPreference = "Stop"

function Get-DeterministicGuid {
    param([Parameter(Mandatory, Position=0)][string]$Text)
    $bytes = [System.Text.Encoding]::UTF8.GetBytes($Text)
    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $hash = $md5.ComputeHash($bytes)
    } finally {
        $md5.Dispose()
    }
    $hex = ($hash | ForEach-Object { $_.ToString("x2") }) -join ""
    $guid = "{0}-{1}-{2}-{3}-{4}" -f $hex.Substring(0,8), $hex.Substring(8,4), $hex.Substring(12,4), $hex.Substring(16,4), $hex.Substring(20,12)
    return "{" + $guid.ToUpper() + "}"
}

$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modRoot = Join-Path $workspaceRoot "Source\Mods"
$sln = Join-Path $modRoot "S1Mods.sln"

if (Test-Path -LiteralPath $sln) { Remove-Item -LiteralPath $sln -Force }

$projects = Get-ChildItem -LiteralPath $modRoot -Recurse -Filter "*.csproj" | Sort-Object FullName
$projectBlocks = New-Object System.Text.StringBuilder
$configBlocks = New-Object System.Text.StringBuilder
$folderBlocks = New-Object System.Text.StringBuilder
$nestedBlocks = New-Object System.Text.StringBuilder
$seenFolders = @{}
$folderGuids = @{}

foreach ($p in $projects) {
    $rel = $p.FullName.Substring($modRoot.Length + 1)
    $relDir = Split-Path $rel -Parent

    $folder = "Root"
    if ($relDir -like "Shared*") {
        $folder = "Shared"
    } elseif (-not [string]::IsNullOrEmpty($relDir) -and $relDir -ne "src") {
        $folder = ($relDir -split '[\\/]')[0]
    }

    if ($folder -ne "Root" -and -not $seenFolders.ContainsKey($folder)) {
        $seenFolders[$folder] = $true
        $fg = Get-DeterministicGuid "Folder:$folder"
        $folderGuids[$folder] = $fg
        [void]$folderBlocks.AppendLine("Project(`"{2150E333-8FDC-42A3-9474-1A3956D46DE8}`") = `"$folder`", `"$folder`", `"$fg`"")
        [void]$folderBlocks.AppendLine("EndProject")
    }

    $projGuid = Get-DeterministicGuid "Project:$rel"
    [void]$projectBlocks.AppendLine("Project(`"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}`") = `"$($p.BaseName)`", `"$rel`", `"$projGuid`"")
    [void]$projectBlocks.AppendLine("EndProject")
    if ($folder -ne "Root") {
        [void]$nestedBlocks.AppendLine("`t`t$projGuid = $($folderGuids[$folder])")
    }
    [void]$configBlocks.AppendLine("`t`t$projGuid.Debug|Any CPU.ActiveCfg = Debug|Any CPU")
    [void]$configBlocks.AppendLine("`t`t$projGuid.Debug|Any CPU.Build.0 = Debug|Any CPU")
    [void]$configBlocks.AppendLine("`t`t$projGuid.Release|Any CPU.ActiveCfg = Release|Any CPU")
    [void]$configBlocks.AppendLine("`t`t$projGuid.Release|Any CPU.Build.0 = Release|Any CPU")
}

$slnContent = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.0.0
MinimumVisualStudioVersion = 10.0.40219.1
$($folderBlocks.ToString())
$($projectBlocks.ToString())
Global
`tGlobalSection(SolutionConfigurationPlatforms) = preSolution
`t`tDebug|Any CPU = Debug|Any CPU
`t`tRelease|Any CPU = Release|Any CPU
`tEndGlobalSection
`tGlobalSection(ProjectConfigurationPlatforms) = postSolution
$($configBlocks.ToString())`tEndGlobalSection
`tGlobalSection(SolutionProperties) = preSolution
`t`tHideSolutionNode = FALSE
`tEndGlobalSection
`tGlobalSection(NestedProjects) = preSolution
$($nestedBlocks.ToString())`tEndGlobalSection
EndGlobal
"@

# MSBuild (.sln-Parser) toleriert keine gemischten Zeilenenden — alles auf CRLF normalisieren.
$slnContent = $slnContent -replace "`r?`n", "`r`n"
Set-Content -LiteralPath $sln -Value $slnContent -Encoding UTF8 -NoNewline
Write-Host "Solution: $sln ($($projects.Count) projects)" -ForegroundColor Green
