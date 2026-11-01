#Requires -Version 7
# Read-only local diagnostic. No game launch, attachment or mutation.
param([ValidateRange(1,65535)][int]$Port = 8765)
$ErrorActionPreference = 'Stop'
function Describe-Process($Process) {
    @{ Id = $Process.Id; Path = $Process.Path; StartedUtc = $Process.StartTime.ToUniversalTime().ToString('O') }
}
try {
    $games = @(Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue | ForEach-Object { Describe-Process $_ })
    # Query all listeners so an absent port is normal, not an ignored query failure.
    $listeners = @(Get-NetTCPConnection -State Listen -ErrorAction Stop | Where-Object { $_.LocalAddress -eq '127.0.0.1' -and $_.LocalPort -eq $Port } | ForEach-Object {
        Describe-Process (Get-Process -Id $_.OwningProcess -ErrorAction Stop)
    })
    @{ available = $true; game_processes = $games; listeners = $listeners } | ConvertTo-Json -Depth 5 -Compress
} catch {
    @{ available = $false; error = $_.Exception.Message } | ConvertTo-Json -Compress
    exit 1
}
