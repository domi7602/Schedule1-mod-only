#Requires -Version 7
# Shared, side-effect-free SDK helpers. Dot-source this file; loading it never
# invokes dotnet or changes the process environment.

function Resolve-S1DotnetPath {
    param([string]$Requested)

    if (-not [string]::IsNullOrWhiteSpace($Requested)) {
        if (Test-Path -LiteralPath $Requested -PathType Leaf) {
            return (Resolve-Path -LiteralPath $Requested).Path
        }
        $command = Get-Command $Requested -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($command) { return $command.Source }
        throw "dotnet not found at '$Requested'."
    }

    $command = Get-Command dotnet -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($command) { return $command.Source }
    if ($env:USERPROFILE) {
        $userLocal = Join-Path $env:USERPROFILE '.dotnet/dotnet.exe'
        if (Test-Path -LiteralPath $userLocal -PathType Leaf) { return $userLocal }
    }
    throw 'dotnet was not found on PATH or in the user-local fallback. Install the .NET 8+ SDK or pass -DotnetPath.'
}

function Assert-S1SdkVersion {
    param([string]$Output)

    $version = $Output.Trim()
    if ($version -notmatch '^(?<major>\d+)\.\d+\.\d+(?:[-+][0-9A-Za-z.-]+)?$') {
        throw "dotnet --version produced no valid SDK version: '$version'."
    }
    if ([int]$Matches.major -lt 8) {
        throw "The active .NET SDK must be 8 or newer for C# 12; selected: '$version'."
    }
    return $version
}

function Assert-S1DotnetSdk {
    param([Parameter(Mandatory)][string]$DotnetPath)

    $versionOutput = @(& $DotnetPath --version 2>&1)
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet --version failed (exit $LASTEXITCODE); only a runtime may be installed: $($versionOutput -join ' ')"
    }
    $version = Assert-S1SdkVersion -Output ($versionOutput -join "`n")
    $sdkOutput = @(& $DotnetPath --list-sdks 2>&1)
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet --list-sdks failed (exit $LASTEXITCODE): $($sdkOutput -join ' ')"
    }
    if (@($sdkOutput | Where-Object { [string]$_ -match '^\s*\d+\.\d+\.\d+' }).Count -eq 0) {
        throw 'No .NET SDK is installed (dotnet --list-sdks was empty); this requires SDK 8 or newer.'
    }
    return $version
}
