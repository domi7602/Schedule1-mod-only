#Requires -Version 7
<#
.SYNOPSIS
    Game-free fixture tests for the Markdown file-path guard.
.DESCRIPTION
    Creates unique disposable Git repositories outside the workspace. Only
    their indexes are changed; no commits, game access or deployment occur.
    Fixtures and child-process output remain under ScratchRoot as receipts.
#>
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$checker = Join-Path $repoRoot 'Tools/check-doc-paths.ps1'
$gitExecutable = (Get-Command git -CommandType Application -ErrorAction Stop).Source
if (-not $ScratchRoot) { $ScratchRoot = [IO.Path]::GetTempPath() }
$scratchFull = [IO.Path]::GetFullPath($ScratchRoot)
$repoFull = [IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($scratchFull.Equals($repoFull, [StringComparison]::OrdinalIgnoreCase) -or
    $scratchFull.StartsWith($repoFull + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Fixture scratch must be outside the repository.'
}
$scratch = Join-Path $scratchFull ('doc-paths-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
$null = New-Item -ItemType Directory -Path $scratch
$script:passed = 0
$script:failed = 0
$script:runs = 0

function Check([bool]$Condition, [string]$Message) {
    if ($Condition) { $script:passed++ }
    else { $script:failed++; Write-Host "FAIL: $Message" -ForegroundColor Red }
}

function Write-File([string]$Root, [string]$Path, [string]$Content) {
    $full = Join-Path $Root $Path
    $null = New-Item -ItemType Directory -Path (Split-Path $full -Parent) -Force
    [IO.File]::WriteAllText($full, $Content, [Text.UTF8Encoding]::new($false))
}

function Git([string]$Root, [string[]]$Arguments) {
    $output = & $gitExecutable -C $Root @Arguments 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) { throw "Fixture git failed: $output" }
    return $output.Trim()
}

function New-Fixture([string]$Name) {
    $root = Join-Path $scratch $Name
    $null = New-Item -ItemType Directory -Path (Join-Path $root 'Tools') -Force
    $null = Git $root @('init', '-q')
    $null = Git $root @('config', 'core.autocrlf', 'false')
    Copy-Item -LiteralPath $checker -Destination (Join-Path $root 'Tools/check-doc-paths.ps1')
    Write-File $root '.gitignore' "docs/local/`n**/bin/`n**/obj/`n**/Library/`n"
    return $root
}

function Run-Fixture([string]$Root, [string[]]$Extra = @()) {
    $script:runs++
    $output = & pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File (Join-Path $Root 'Tools/check-doc-paths.ps1') @Extra 2>&1 | Out-String
    $code = $LASTEXITCODE
    $receipt = Join-Path $scratch ("run-{0:D2}-{1}.txt" -f $script:runs, (Split-Path $Root -Leaf))
    [IO.File]::WriteAllText($receipt, "ExitCode: $code`n$output", [Text.UTF8Encoding]::new($false))
    return [pscustomobject]@{ Code = $code; Output = $output }
}

# Real links resolve relative to their document, including parent/simple paths,
# spaces, encoded Unicode, fragments, parentheses and optional link titles.
$root = New-Fixture 'valid'
foreach ($path in @('README.md', 'docs/README.md', 'docs/Guide with spaces.md', 'docs/Grüße.md', 'docs/Guide(draft).md', 'docs/images/picture.png')) {
    Write-File $root $path ''
}
Write-File $root 'docs/guide.md' @'
[root](../README.md#not-a-checked-anchor)
[simple](README.md)
[dot](./README.md "A title")
![image](images/picture.png)
[angle](<Guide with spaces.md> 'Optional title')
[encoded](Guide%20with%20spaces.md)
[unicode](Gr%C3%BC%C3%9Fe.md)
[parentheses](Guide(draft).md)
[escaped](Guide\(draft\).md)
[web](https://example.invalid/missing.md)
[mail](mailto:fixture@example.invalid)
[app](codex://threads/example)
[network](//example.invalid/missing.md)
[absolute](C:/missing.md)
[anchor](#missing-heading)
[query](?view=source)
[reference][local]
[local]: README.md "A reference title"
`docs/README.md` remains a repo-relative prose token.
`[sample](missing-inside-code.md)`
``[sample](missing-inside-double-code.md) `tick` ``
```md
[ignored](missing-fence.md)
````
~~~~md
[ignored](missing-tilde.md)
~~~
[still ignored](missing-short-close.md)
~~~~
````md
```
[still ignored](missing-backtick-short-close.md)
````
'@
$null = Git $root @('add', '--', 'README.md', 'docs/README.md')
$indexBefore = (Get-FileHash -LiteralPath (Join-Path $root '.git/index')).Hash
$statusBefore = Git $root @('status', '--porcelain', '--untracked-files=all')
$result = Run-Fixture $root
Check ($result.Code -eq 0) "supported valid links and literal examples pass: $($result.Output)"
Check ($result.Output -match '6 Markdown-Dateien, 6 Quelle/Ziel-Paare') 'valid links are recognized and equivalent encoded/title/code targets deduplicate'
Check ($result.Output -match 'Anker nicht geprueft') 'success states that anchor targets were not checked'
Check ((Get-FileHash -LiteralPath (Join-Path $root '.git/index')).Hash -eq $indexBefore) 'guard does not mutate the fixture Git index'
Check ((Git $root @('status', '--porcelain', '--untracked-files=all')) -eq $statusBefore) 'guard does not mutate fixture files/status'

# The same token succeeding elsewhere, or at the root, must not mask a failure.
$root = New-Fixture 'broken'
Write-File $root 'docs/shared.md' ''
Write-File $root 'docs/a/shared.md' ''
Write-File $root 'docs/a/readme.md' '[valid](shared.md)'
Write-File $root 'docs/b/readme.md' @'
[broken](shared.md)
[duplicate](./shared.md)
[parent](../missing-parent.md)
![missing image](images/missing.png)
[space](<missing file.md>)
[title](missing-title.md "A title")
[ref]: missing-reference.md "A title"
[fence ended](missing-after-fence.md)
'@
Write-File $root 'docs/c/readme.md' '[also broken](shared.md)'
Write-File $root 'docs/root-mask.md' @'
`docs/shared.md`
[broken despite root and code](docs/shared.md)
'@
$result = Run-Fixture $root
Check ($result.Code -eq 1) 'missing paths fail'
Check ($result.Output -match 'docs/b/readme.md:1:.*shared.md') 'same token in another source does not mask a broken link'
Check ($result.Output -match 'docs/c/readme.md:1:.*shared.md') 'both failing source files are reported'
Check ($result.Output -match 'docs/root-mask.md:2:.*docs/shared.md') 'existing repo-root/code path cannot mask a broken document-relative link'
Check ([regex]::Matches($result.Output, 'docs/b/readme.md:\d+:.*shared.md').Count -eq 1) 'equivalent targets in one source are deduplicated'
foreach ($target in @('missing-parent.md', 'images/missing.png', 'missing file.md', 'missing-title.md', 'missing-reference.md', 'missing-after-fence.md')) {
    Check ($result.Output.Contains($target)) "reports missing target: $target"
}
Check ($result.Output -notmatch 'Pfad nicht gefunden.*A title') 'titles are not treated as target paths'

# Inventory follows Git ownership, not all files physically below these trees.
$root = New-Fixture 'inventory'
$included = @('TOP.md', 'docs/untracked ünicode.md', 'docs/Mixed.Md', 'Tools/README.md', 'Skills/example/SKILL.md', 'Source/Mods/Example/docs/README.md', 'ThirdParty/README.md', 'GameReferences/README.md')
foreach ($path in $included) { Write-File $root $path '[broken](missing-owned.md)' }
$excluded = @('docs/local/ignored.md', 'Source/Mods/Example/bin/generated.md', 'Source/Mods/Example/obj/generated.md', 'Source/Mods/Example/unity/Library/generated.md', 'Source/Mods/Example/build/logs/old/report.md', 'Source/Mods/Example/docs/upstream/README.md', 'Source/Mods/Example/vendor/README.md', 'Source/Mods/Example/node_modules/README.md', 'Source/Mods/External/README.md', 'ThirdParty/External/README.md', 'GameReferences/decompiled/README.md', 'Source/Archive/Old/docs/README.md')
foreach ($path in $excluded) { Write-File $root $path '[broken](missing-excluded.md)' }
Write-File $root '.gitmodules' @'
[submodule "external"]
    path = Source/Mods/External
    url = https://example.invalid/external.git
'@
# Force-track generated docs to prove the explicit exclusions remain effective.
$null = Git $root @('add', '-f', '--', 'TOP.md', 'Source/Mods/Example/bin/generated.md', 'Source/Mods/Example/obj/generated.md', 'Source/Mods/Example/unity/Library/generated.md')
$result = Run-Fixture $root
Check ($result.Code -eq 1) 'expanded owned-document inventory catches broken links'
foreach ($path in $included) { Check ($result.Output.Contains("${path}:1:")) "scans maintained document: $path" }
foreach ($path in $excluded) { Check (-not $result.Output.Contains("${path}:1:")) "excludes generated/external document: $path" }

# Optional/generated prerequisites are checked after resolving the link. A
# missing initialized-submodule target remains a failure, unlike an empty one.
$root = New-Fixture 'optional'
Write-File $root '.gitmodules' @'
[submodule "api"]
    path = ThirdParty/S1API
    url = https://example.invalid/api.git
[submodule "empty"]
    path = ThirdParty/Empty
    url = https://example.invalid/empty.git
'@
Write-File $root 'ThirdParty/S1API/README.md' 'Existing submodule content.'
$null = New-Item -ItemType Directory -Path (Join-Path $root 'ThirdParty/Empty') -Force
# Real gitlinks reproduce check-ignore's exit 128 for submodule interiors.
$fakeCommit = '1234567890123456789012345678901234567890'
$null = Git $root @('update-index', '--add', '--cacheinfo', "160000,$fakeCommit,ThirdParty/S1API")
$null = Git $root @('update-index', '--add', '--cacheinfo', "160000,$fakeCommit,ThirdParty/Empty")
Write-File $root 'docs/optional.md' @'
[local build config](../ThirdParty/S1API/local.build.props)
[empty submodule](../ThirdParty/Empty/missing.md)
[decompile](../GameReferences/decompiled/missing.cs)
[ignored output](local/optional.dat)
[existing submodule target](../ThirdParty/S1API/README.md)
'@
$result = Run-Fixture $root
Check ($result.Code -eq 0) "defined local/submodule prerequisites remain allowed: $($result.Output)"
Check ($result.Output -match 'Submodul nicht initialisiert') 'empty-submodule exception is visible'
Check ($result.Output -match 'docs/local/optional.dat.*gitignored') 'ignored target is resolved relative to the document'
Write-File $root 'docs/broken.md' @'
[bad initialized target](../ThirdParty/S1API/missing.md)
[typo config](../ThirdParty/S1API/local.build.prop)
[bad relative config](ThirdParty/S1API/local.build.props)
'@
$result = Run-Fixture $root
Check ($result.Code -eq 1) 'initialized-submodule typos fail'
Check ($result.Output -match 'docs/broken.md:1:') 'does not exempt all initialized submodule contents'
Check ($result.Output -match 'docs/broken.md:2:') 'local.build.props exception is exact'
Check ($result.Output -match 'docs/broken.md:3:') 'root-based local config exception cannot rescue a wrong relative link'
$result = Run-Fixture $root @('-Allow', '^ThirdParty/S1API/(missing\.md|local\.build\.prop)$|^docs/ThirdParty/S1API/local\.build\.props$')
Check ($result.Code -eq 0) 'explicit Allow applies to normalized repo-relative targets'

# Historical evidence can retain the old inline-code spelling without waiving
# real links, the following line, or malformed/reasonless annotations.
$root = New-Fixture 'history'
Write-File $root 'docs/history.md' @'
`Knowledge/retired.md` <!-- doc-paths: historical-reference (original evidence location; no longer present) -->
'@
$result = Run-Fixture $root
Check ($result.Code -eq 0) 'reasoned same-line annotation permits historical code evidence'
Write-File $root 'docs/history.md' @'
`Knowledge/retired.md` <!-- doc-paths: historical-reference (original evidence) --> [real link](missing.md)
`Knowledge/next-line.md`
`Knowledge/no-reason.md` <!-- doc-paths: historical-reference -->
`Knowledge/empty-reason.md` <!-- doc-paths: historical-reference () -->
`Knowledge/whitespace-reason.md` <!-- doc-paths: historical-reference (   ) -->
`Knowledge/near-miss.md` <!-- doc-paths: historical (original evidence) -->
'@
$result = Run-Fixture $root
Check ($result.Code -eq 1) 'history marker does not broadly waive failures'
Check ($result.Output -match 'docs/history.md:1:.*\(Link\): missing.md') 'real links are still checked on annotated lines'
Check ($result.Output -notmatch '\(Code\): Knowledge/retired.md') 'only the annotated code token is exempt'
foreach ($line in 2..6) { Check ($result.Output -match "docs/history.md:${line}:") "invalid/unscoped marker does not waive line $line" }

# A non-Git tree must fail rather than silently claiming that zero docs passed.
$nonGit = Join-Path $scratch 'not-git'
$null = New-Item -ItemType Directory -Path (Join-Path $nonGit 'Tools') -Force
Copy-Item -LiteralPath $checker -Destination (Join-Path $nonGit 'Tools/check-doc-paths.ps1')
$result = Run-Fixture $nonGit
Check ($result.Code -ne 0) 'Git inventory failure is fatal'
Check ($result.Output -notmatch 'Doc-Pfad-Check OK') 'Git failure cannot produce a successful report'

Write-Host "Doc-paths tests: $script:passed passed, $script:failed failed. Receipts: $scratch"
if ($script:failed -gt 0) { exit 1 }
exit 0
