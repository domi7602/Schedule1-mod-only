# Production source-file budget

New mods target 15–20 hand-maintained production files including the csproj; smaller mods
stay smaller. Tests, assets, docs, inherited build rules, external dependencies and generated
bin/obj are outside this source metric. This supports the owner's 20-attachment source-review
limit, not a claim that file count measures code quality. Additional attached reports still
consume attachment slots.

## Authorized consolidation scope

The owner authorized mechanical consolidation of all active workspace mods above the budget.
Preserve the actual dirty working-tree inputs, including untracked sources; HEAD is not a
substitute for ongoing work. Keep the existing csproj location, type identities, imported
symbol bindings, conditional test/production code and runtime assets. No commit/push was
requested. Shared is a library, not a mod; ThirdParty projects are not rewritten by this scope.
TaxiDriver's behavior repair is separate from the mechanical changes to the other mods.

## Baseline and verification

The scoped pre-consolidation worktree archive and SHA-256 manifest are at
`C:/Users/pc/ModConsolidation-2026-10-18/baseline-current-worktree.zip` and `manifest.json`.
The archive contains 547 source/project/document files and excludes the separately tracked
TaxiDriver workstream. Per-mod evidence is recorded beside it, not in distributable packages.

Fourteen mods already fit the budget and have been left source-byte-identical to that archive.
Their nonincremental Release builds passed with deployment disabled; every existing matching
test project was also executed. These results do not establish new in-game acceptance.

| Mod | Production files including csproj | Verification |
|---|---:|---|
| AutoPackagingStation | 11 | Build; 66 tests passed |
| BankApp | 13 | Build; 103 tests passed |
| CalculatorApp | 6 | Build; 18 tests passed |
| CustomSkateboard | 9 | Build; no dedicated test project |
| MessagesPlus | 12 | Build; 25 tests passed |
| MoreSaveSlots | 12 | Build; no dedicated test project |
| NotesApp | 4 | Build; no dedicated test project |
| PhoneScrollFix | 2 | Build; no dedicated test project |
| PotScanner | 10 | Build; no dedicated test project |
| ProductAppQoL | 3 | Build; no dedicated test project |
| PropertyLEDs | 19 | Build; 94 tests passed |
| StackLimitMod | 8 | Build; no dedicated test project |
| StorageScanner | 8 | Build; 37 tests passed |
| Weather | 3 | Build; no dedicated test project |

## Consolidated mods (central verification executed)

| Mod | Before (CS + project) | After | Existing tests rerun centrally |
|---|---:|---:|---|
| BusinessIncome | 23 | 20 | 228 + 17 passed |
| HitmanPhone | 32 | 16 | 33 passed |
| LightControl | 24 | 20 | 143 passed (console harness) |
| Minimap | 21 | 20 | 186 passed |
| PocketShop | 21 | 20 | 52 passed |
| WorldLook | 32 | 18 | 82 + 80 + 1,143 passed |

The parent independently compared all merged source sections with the frozen archive,
checked every unmerged source byte-for-byte, checked unchanged project files, and reran
all six nonincremental builds and all nine relevant test projects. All passed.
BusinessIncome retains 5 baseline compiler warnings; HitmanPhone retains 7. No new warning
was introduced. Per-type IL preservation and scoped format checks are recorded in the
per-mod reports; compiler static-data address labels are not behavioral differences.

The game was closed and built/installed Shared hashes matched before deployment. All six
mods were then rebuilt and deployed using the inherited target without building dependencies.
The parent read back installed DLL, PDB, metadata and packaged assets; all matched their
source/output hashes. The deterministic release DLLs also matched the central compile-only
builds, and the installed Shared hash remained unchanged. Previous installed artifacts are
preserved under `C:/Users/pc/ModConsolidation-2026-10-18/installed-before/`.

The complete budget inventory (`all21-budget.json`) contains 21 active workspace mods;
every row is at or below 20. Fourteen needed no source changes. TaxiDriver has exactly
19 CS plus csproj. Its separate reverse-recovery repair passed final delta review and was
released/deployed as 0.8.19 with 605 tests passed and 0 warnings/errors. Installed artifacts
were read back; native gameplay acceptance remains open. See its verification record.

Across these active mods, the parent executed 16 existing test projects/harnesses with
2,912 cases passed (including TaxiDriver's 605); Shared is not included in that total.
Mods without dedicated test projects were built but were not assigned invented test coverage.
No commit or push was performed.

These mechanical deployments preserve versions and historical gameplay acceptance; they
do not establish a new in-game test result.

A source layout change does not authorize a version bump or claim gameplay verification.
Only reviewed changed projects are eligible for deployment after game-closed and dependency
compatibility checks; a no-deploy build alone does not deliver an installed update.
