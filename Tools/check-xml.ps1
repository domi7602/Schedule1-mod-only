#Requires -Version 7
<#
.SYNOPSIS
    Parses Git-tracked MSBuild project and build files.

.DESCRIPTION
    Inventory is tracked *.csproj, *.props and *.targets, including archived
    projects. Untracked and generated files are not parsed. A deleted tracked
    path is skipped. Invalid XML fails and names the file. This check does not
    write files or change Git configuration.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/check-xml.ps1
#>
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$workspaceRoot = [IO.Path]::GetFullPath((Split-Path $PSScriptRoot -Parent))

function Invoke-TrackedGit {
    param([string[]]$Arguments)
    # Decode Git output as UTF-8 regardless of the host console code page.
    # -c is process-local and does not change the user's Git configuration.
    $start = [System.Diagnostics.ProcessStartInfo]::new('git')
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.StandardOutputEncoding = [Text.UTF8Encoding]::new($false)
    $start.StandardErrorEncoding = [Text.UTF8Encoding]::new($false)
    foreach ($config in @('core.quotepath=false', 'i18n.logOutputEncoding=utf-8')) {
        $start.ArgumentList.Add('-c')
        $start.ArgumentList.Add($config)
    }
    $start.ArgumentList.Add('-C')
    $start.ArgumentList.Add($workspaceRoot)
    foreach ($argument in $Arguments) { $start.ArgumentList.Add($argument) }
    $start.Environment['GIT_OPTIONAL_LOCKS'] = '0'
    $process = [System.Diagnostics.Process]::Start($start)
    try {
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        $process.WaitForExit()
        $errorText = $stderr.GetAwaiter().GetResult()
        if ($process.ExitCode -ne 0) {
            throw "Git XML inventory could not be read (exit $($process.ExitCode)): $errorText"
        }
        return $stdout.GetAwaiter().GetResult()
    }
    finally { $process.Dispose() }
}

$inventory = Invoke-TrackedGit @('ls-files', '-z', '--cached', '--', '*.csproj', '*.props', '*.targets')
$paths = @($inventory -split "`0" | Where-Object { $_ } | Sort-Object -Unique)
$problems = [Collections.Generic.List[string]]::new()
$checked = 0
foreach ($relative in $paths) {
    $full = Join-Path $workspaceRoot ($relative -replace '/', [IO.Path]::DirectorySeparatorChar)
    if (-not (Test-Path -LiteralPath $full -PathType Leaf)) { continue }
    $checked++
    $reader = $null
    try {
        $settings = [System.Xml.XmlReaderSettings]::new()
        $settings.DtdProcessing = [System.Xml.DtdProcessing]::Prohibit
        $settings.XmlResolver = $null
        $settings.CloseInput = $true
        $reader = [System.Xml.XmlReader]::Create($full, $settings)
        $document = [System.Xml.XmlDocument]::new()
        $document.Load($reader)
    }
    catch {
        $message = $_.Exception.Message
        if ($_.Exception.InnerException) { $message = $_.Exception.InnerException.Message }
        $problems.Add("${relative}: $message")
    }
    finally {
        if ($reader) { $reader.Dispose() }
    }
}

Write-Host ("=== XML check ({0} files) ===" -f $checked)
if ($problems.Count -eq 0) {
    Write-Host 'XML check OK.'
    exit 0
}
Write-Host ("Invalid XML: {0}" -f $problems.Count)
foreach ($problem in $problems) { Write-Host "  - $problem" }
exit 1
