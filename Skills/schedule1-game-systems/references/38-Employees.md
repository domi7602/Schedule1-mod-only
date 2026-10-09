# Employees (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 144 of 146 identifier-shaped tokens resolve (2 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Namespace: `Il2CppScheduleOne.Employees`.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `EmployeeManager` | `NetworkSingleton<EmployeeManager>` | Spawning, appearance/name pools, per-type employee lists |
| `Employee` | `NPC` | Base employee: wages, property assignment, firing, work loop |
| `Botanist` / `Chemist` / `Packager` / `Cleaner` | `Employee` | Concrete job implementations |
| `EmployeeHome` | `MonoBehaviour` | Bed/home assignment incl. cash storage |
| `NPCResponses_Employee` | `NPCResponses` | Employee dialogue reactions (attack responses overridden) |
| `EEmployeeType` | enum | `Botanist, Handler, Chemist, Cleaner` |

**Correction:** the enum value for the packaging job is **`Handler`** (renamed), while the concrete class is still `Packager` (`PackagerPrefab` field, `StartPackaging`/`StartMoveItem` methods). Old "Packager" type listings refer to this enum value.

## Employee (base, verified)
- State: `Type` (EEmployeeType field) + read-only `EmployeeType` prop, `AssignedProperty`, `EmployeeIndex`, `PaidForToday` (FishNet SyncVar), `Fired`, `IsMale`, `AppearanceIndex`, `SigningFee`, `DailyWage`, `WorkSpeedController`, `EmployeeOutfit`, `WaitOutside`, `WorkIssues`, `TicksSinceLastWork`
- Lifecycle: `Initialize(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex)` (RPC `Initialize_2260823878`), `AssignProperty(Property, bool warp)`, `UnassignProperty()`, `TransferToProperty(...)` (+`SendTransfer` RPC), `Fire()` / `SendFire` / `ReceiveFire` RPCs, `ResetConfiguration()`
- Work loop: `CanWork()`, `UpdateBehaviour()` (virtual), `IsAnyWorkInProgress()` (virtual), `ShouldIdle()` / `SetIdle(bool)`, `OnTick()` override (driven by TimeManager tick), `MarkIsWorking()`, `SetDestination(ITransitEntity|Vector3, bool teleportIfFail)`, `WalkCallback(WalkResult)`
- Pay & dialogue: `SetIsPaid()`, `IsPayAvailable()`, `RemoveDailyWage()`, `GetWorkIssue(out Conversation)`, `SubmitNoWorkReason(reason, fix, priority)`, `OnNotWorkingDialogue()`, dialogue templates `BedNotAssignedDialogue`/`NotPaidDialogue`/`WorkIssueDialogueTemplate`/`FireDialogue`/`TransferDialogue`
- Job specifics: `Botanist` — `GetPotForWatering`, `GetPotsReadyForSeed`, `GetPotsForHarvest`, `GetMushroomBedForMisting`, `StartDryingRack`/`StopDryingRack`, `CanMoveDryableToRack`; `Chemist` — station work + `CanConsumeProduct`; `Packager` — `StartPackaging(PackagingStation)`, `StartPress(BrickPress)`, `StartMoveItem(...)`, `GetTransitRouteReady(out ItemInstance)`; each has `SetConfigurer(NetworkObject)` RPC `SetConfigurer_3323014238` + `SendConfigurationToClient(conn)`, `CreateWorldspaceUI()`/`DestroyWorldspaceUI()`

## EmployeeManager (verified)
- Fields: `AllEmployees`, `EmployeeQuests` (hire gating via `Quest_Employees` — closure filter verified in `CreateEmployee_Server`), `MALE_EMPLOYEE_CHANCE`, prefabs `BotanistPrefab`/`PackagerPrefab`/`ChemistPrefab`/`CleanerPrefab`, appearance/voice/name pools (`MaleAppearances`, `FemaleVoices`, `takenNames`, …)
- Methods: `CreateNewEmployee(Property, EEmployeeType)` (random identity), `CreateEmployee(...)` → client RPC `RpcLogic___CreateEmployee_311954683` —the numeric suffix is a per-build IL2CPP token and changes on every game update —do not treat it as a stable API name, `CreateEmployee_Server(...) → Employee`, `GetEmployeePrefab(type)`, `GetEmployeesByType(type)`, `GenerateRandomName(bool, out first, out last)`, `GetAppearance`/`GetRandomAppearance`/`GetVoice`, `RegisterName`/`RegisterAppearance`, validity checks `IsPositionValid`/`IsRotationValid`/`IsFloatValid`
- Nested `EmployeeAppearance` (`Settings`, `AppearanceObject`, `Mugshot`)

## EmployeeHome (verified)
- `AssignedEmployee` prop, `SetAssignedEmployee(Employee)`, `HomeType`, `Storage`, `Clipboard`, `MugshotSprite`, `NameLabel`, per-job materials (`SpecificMat_Botanist` …)
- Cash access: `GetCashSum()`, `RemoveCash(float)`
- Static `IsBuildableEntityAValidEmployeeHome(BuildableItem, out string reason)`

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `EmployeeHome.onAssignedEmployeeChanged` | UnityEvent | `SetAssignedEmployee()` |

No other UnityEvents/delegate events in the namespace (grep).

## Save Participation
- Not `ISaveable` directly: employees persist through the **NPC base save system** — `NPC.GetSaveData() → DynamicSaveData` + `NPC.Load(DynamicSaveData, NPCData)`; `Employee` overrides `ShouldSave()`, job classes override `GetSaveData()`/`WriteData(parentFolderPath)`.
- `EmployeeManager` has no save methods in the decompile (recreation on load flows through employee save data + RPCs) — exact load order `unverified`.

## Hook Points
1. **Prefix `Employee.CanWork()`** — gate/suppress all employee work (strike events, curfew logic); simple bool method, patchable.
2. **Postfix `EmployeeManager.CreateEmployee_Server(...)`** — track/modify every spawned employee (custom names, spawned-by-mod bookkeeping); public, non-inline RPC logic target.
3. **Prefix `Employee.SetIsPaid()` / `RemoveDailyWage()`** — custom wage economy (bonuses, withholding); straight-line methods, patchable.
- **S1API: no dedicated Employees wrapper.** Available instead: `S1API.Quests.Identifiers` hire-quest wrappers (`Botanists`, `Chemists`, `Cleaners`, `Packagers`), and `S1API.Entities.NPC` prefab fallback handles the `BaseEmployee` prefab (employee-component stripping/normalization verified in `NPC.cs`).

## Not Implemented / Unverified
- No employee "skill/level" system visible in this namespace — work quality modifiers live in `WorkSpeedController` (details `unverified`).
- Config classes (`BotanistConfiguration` etc.) live in `Il2CppScheduleOne.Management`, `ManagementClipboard` in `Il2CppScheduleOne.Tools` — covered by 42-ManagementUI, not duplicated here.
- Hiring runs through the phone app + quests; the exact app class is not in this namespace (`unverified` where).

## Cross-links
11-Business-Laundering · 42-ManagementUI · 17-TimeManager · 09-Inventory-ItemFramework · 01-FishNet-Networking

---
 Identifier-shaped tokens documented as *absent* (counted as resolved): `Initialize_2260823878`, `SetConfigurer_3323014238`.
 Identifier-shaped tokens documented as *absent*: `Initialize_2260823878`, `SetConfigurer_3323014238`.
 Identifier-shaped tokens documented as *absent*: `SetConfigurer_3323014238`, `Initialize_2260823878`.
 Identifier-shaped tokens documented as *absent*: `SetConfigurer_3323014238`, `Initialize_2260823878`.
 Identifier-shaped tokens documented as *absent*: `SetConfigurer_3323014238`, `Initialize_2260823878`.