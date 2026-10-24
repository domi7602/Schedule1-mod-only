# TaxiDriver tests and verification

Executable tests live in [TaxiDriver.Tests](../../../Tests/TaxiDriver.Tests/TaxiDriver.Tests.csproj).
Use the repository runner for xUnit and console suites together:

```pwsh
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1 -Selection pure
```

For a focused TaxiDriver run (from the repository root):

```pwsh
dotnet test Source/Tests/TaxiDriver.Tests/TaxiDriver.Tests.csproj -c Release -p:S1NoDeploy=true
```

Current gameplay status: TaxiDriver 1.0.0 is the version-only milestone of the owner-accepted
0.8.22 code, confirmed 2026-10-24 without itemized actions. Known road correction/jitter
remains a limitation. See [compatibility](../../../../docs/compatibility.md#verification-matrix)
and the [driver-binding record](../docs/driver-binding-verification.md).

The [preserved test notes](../docs/history/test-notes-20261024.md) contain the former
165/207-test snapshots, beta API references and then-open gameplay checklists. Those
results retain their dates and scope; they are not the current backlog or proof that
each old action was individually observed. Multiplayer acceptance remains out of scope.
