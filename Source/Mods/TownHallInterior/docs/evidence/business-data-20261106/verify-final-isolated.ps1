$ErrorActionPreference = 'Stop'
$repo = 'C:/Users/pc/Schedule1-mod-only'
$scratch = "$repo/.scratch/townhall-business-data-20261106"
$sources = @()
foreach ($module in @('S1API','S1MAPI')) {
    $path = "$repo/ThirdParty/$module"
    $head = (& git -C $path rev-parse HEAD).Trim()
    if ($LASTEXITCODE -ne 0) { throw 'Local submodule revision unavailable' }
    $changes = & git -C $path status --porcelain --untracked-files=all
    if ($LASTEXITCODE -ne 0 -or $changes) { throw 'Local submodule is not clean' }
    $sources += [ordered]@{ module=$module; path=$path; commit=$head; clean=$true }
}
$configBase = if ($env:GIT_CONFIG_COUNT) { [int]$env:GIT_CONFIG_COUNT } else { 0 }
$config = @(
    @{ Key="url.file:///$repo/ThirdParty/S1API.insteadOf"; Value='https://github.com/ifBars/S1API.git' },
    @{ Key="url.file:///$repo/ThirdParty/S1MAPI.insteadOf"; Value='https://github.com/domi7602/S1MAPI.git' },
    @{ Key='protocol.file.allow'; Value='always' }
)
for ($offset = 0; $offset -lt $config.Count; $offset++) {
    Set-Item -Path "Env:GIT_CONFIG_KEY_$($configBase + $offset)" -Value $config[$offset].Key
    Set-Item -Path "Env:GIT_CONFIG_VALUE_$($configBase + $offset)" -Value $config[$offset].Value
}
$env:GIT_CONFIG_COUNT = [string]($configBase + $config.Count)
[ordered]@{ processLocalOnly=$true; reason='Use already available clean pinned local submodules for the corrected isolated snapshot'; noGlobalGitConfigChanges=$true; sources=$sources } | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath "$scratch/isolated-local-submodules.json" -Encoding utf8NoBOM
& pwsh -NoProfile -ExecutionPolicy Bypass -File "$repo/Tools/verify-clean-checkout.ps1" -Mode Pure > "$scratch/isolation-final-console.log.txt" 2>&1
$verifyExit = $LASTEXITCODE
Get-Content -LiteralPath "$scratch/isolation-final-console.log.txt" -Tail 8
exit $verifyExit
