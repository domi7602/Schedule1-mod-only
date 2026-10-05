# BusinessIncome safety verification

## Automated verification

Run from the repository root:

```sh
dotnet test Source/Tests/BusinessIncome.Tests/BusinessIncome.Tests.csproj -c Release --nologo
dotnet test Source/Tests/BusinessIncome.ConfigIo.Tests/BusinessIncome.ConfigIo.Tests.csproj -c Release --nologo
dotnet build Source/Mods/BusinessIncome/src/BusinessIncome.csproj -c Release -p:S1NoDeploy=true --nologo
```

The test project links the production calculator, collection configuration and financial core without game assemblies. It covers deterministic float reference outputs, input validation, payout windows, monotone counters, persistence failures and unresolved bank outcomes. The IO project additionally links production Shared SafeStorage, payout storage/facade and IncomeEngine against real files, replacing only game/environment boundaries. It verifies migration artifact protection, terminal-day idempotency, durable empty-business catch-up and client dry-run behavior. Passing these tests does not prove native lifecycle behavior or actual bank settlement. The mod build checks runtime integration against local game assemblies and must not deploy during verification.

## Preparation for one consolidated gameplay round

- Use disposable copies of saves and back up `UserData/BusinessIncome` plus `UserData/MelonPreferences.cfg`.
- Record the installed BusinessIncome and S1API versions from `MelonLoader/Latest.log`. Do not assume the installed DLL matches the source under test.
- Deploy the candidate DLL only as an explicitly authorized operation, with the game closed. No release/version change is implied.
- Record bank balance, in-game day/hour, owned businesses, staff and `biz stats` before each case.
- For file-corruption cases, edit only copied data while the game is closed. Preserve the original state, marker and all `.bak`/`.tmp` artifacts for diagnosis.

## Gameplay checklist

| Setup | Action | Expected result |
|---|---|---|
| Fresh copied save with no payout ledger | Load, inspect `biz stats`, restart once | Installation day is recorded as initialization, not a bank payout. No historical windfall. The seed survives restart. |
| Existing valid ledger; hour 0 | Cross a day boundary, then re-trigger the event/reload | Exactly one bank request for the newly eligible day; ordinary repetition does not pay twice. |
| Existing valid ledger; nonzero hour | Check before, inside and after the configured hour | Current day pays only inside the configured window; repeated hour events do not pay again. |
| Valid ledger behind current day | Load or sleep across missed days | Eligible days process oldest first, then the current day only if its window permits. Backlog respects the cap. |
| No owned businesses, then later purchase one | Advance days without businesses, then buy a business | Empty days become durable terminal days; purchase does not create a windfall for days already processed without businesses. |
| Any active save | Run `biz trigger` and inspect balance/files | Preview only: no bank transfer and no payout-state/marker write. An unreadable business list is an error, not a successful zero preview. |
| Paid day | Run `biz trigger --commit` | Refused without explicit `--force`; pending safety locks remain effective even with `--force`. |
| Valid pending marker in copied slot | Load and run automatic payout plus manual forced trigger | Both remain blocked. Check bank history before making an explicit reconciliation choice. |
| Pending transfer known to have arrived | Run `biz pending confirm`, then reload | Forward state is durable before marker removal; no repeat payout. A write/delete failure remains visibly blocked. |
| Pending transfer known not to have arrived | Read `biz pending` instructions and use explicit resolution | No claim of successful recovery unless required state/marker operations succeed. Never resolve merely because an API exception occurred. |
| Corrupt marker, unreadable backup or orphan marker artifact | Load copied save | Loud blocked status; no silent retry, fresh initialization or automatic artifact deletion. Preserve files for manual repair. |
| Corrupt payout ledger or older backup-only ledger | Load copied save | No silent fresh seed or automatic replay from an older ledger. Recovery requires inspection and an explicit decision. |
| Distinct slots A and B | Load A, return to menu, load B | No writes during unresolved load transitions; each slot retains its own ledger and lock status. |
| Multiplayer host and client | Cross payout boundary and inspect both file sets | Only host performs bank requests and initialization/cap/terminal state writes. Client may preview without writing. |
| Collection sidecar with empty weekend list | Restart and inspect `biz config`/weekend preview | Empty list remains empty. Invalid entries are warned and discarded individually; scalar TOML settings are not overridden by legacy JSON scalar fields. |

## Diagnostic handoff

For an unexpected result, retain the relevant `MelonLoader/Latest.log` lines, exact command output, day/hour, bank balance/history, slot identity and copies of payout/marker files with artifacts. Do not remove a pending marker as a generic repair: deleting it does not prove money was received and can enable a duplicate payout.

Fault injection for write/delete failures and mutate-then-throw bank calls is covered by automated seams. Do not damage the real game installation to simulate these cases.
