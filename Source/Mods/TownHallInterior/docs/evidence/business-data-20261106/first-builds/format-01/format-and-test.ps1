$ErrorActionPreference = 'Stop'
$repo = 'C:/Users/pc/Schedule1-mod-only'
$scratch = "$repo/.scratch/townhall-business-data-20261106"
$env:S1NoDeploy = 'true'
$env:S1TownHallBusinessData = 'true'
& dotnet format whitespace "$repo/Source/Mods/TownHallInterior/src/TownHallInterior.csproj" --no-restore --include "$repo/Source/Mods/TownHallInterior/src/BusinessConstruction.cs" "$repo/Source/Mods/TownHallInterior/src/BusinessBindingProbe.cs" --verbosity diagnostic > "$scratch/format-mod.log.txt" 2>&1
if ($LASTEXITCODE -ne 0) { Get-Content -LiteralPath "$scratch/format-mod.log.txt" -Tail 30; throw 'Production format failed' }
Remove-Item Env:S1TownHallBusinessData
& dotnet format whitespace "$repo/Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj" --no-restore --include "$repo/Source/Tests/TownHallInterior.Tests/BusinessDataTests.cs" --verbosity diagnostic > "$scratch/format-tests.log.txt" 2>&1
if ($LASTEXITCODE -ne 0) { Get-Content -LiteralPath "$scratch/format-tests.log.txt" -Tail 30; throw 'Test format failed' }
Write-Output 'Scoped production/test whitespace formatting passed.'
& dotnet test "$repo/Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj" -c Release -p:S1NoDeploy=true --no-restore --logger 'trx;LogFileName=townhall-green.trx' --results-directory "$scratch/tests" > "$scratch/tests/townhall-green.log.txt" 2>&1
if ($LASTEXITCODE -ne 0) { Get-Content -LiteralPath "$scratch/tests/townhall-green.log.txt" -Tail 35; throw 'TownHall tests failed' }
Get-Content -LiteralPath "$scratch/tests/townhall-green.log.txt" -Tail 3
[xml]$trx = Get-Content -LiteralPath "$scratch/tests/townhall-green.trx" -Raw
$dataResults = @($trx.TestRun.Results.UnitTestResult | Where-Object { $_.testName -like '*BusinessDataTests.*' })
if ($dataResults.Count -eq 0 -or @($dataResults | Where-Object { $_.outcome -ne 'Passed' }).Count) { throw 'Data tests not all passed within full run' }
[ordered]@{ source='townhall-green.trx'; derivedFromFullRun=$true; total=$dataResults.Count; passed=$dataResults.Count; failed=0 } | ConvertTo-Json | Set-Content -LiteralPath "$scratch/tests/data-final-counts.json" -Encoding utf8NoBOM
Write-Output "Data subset within full run: $($dataResults.Count) passed."
