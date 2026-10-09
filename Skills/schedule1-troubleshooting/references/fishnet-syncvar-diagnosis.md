# FishNet SyncVar Diagnosis — Multiplayer Cache Staleness

> verified: 2026-10-05 — decompile facts in §3 are grep-verified against `GameReferences/decompiled/Assembly-CSharp/` (older game build proxies, regenerated 2026-10-03). The onChangeCallback surface (§4 Option A) and ALL host/client runtime behaviors are **unverified** — no in-game multiplayer test was run for this document. FishNet / S1API.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 23/30 identifier-shaped tokens resolve (5 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## 1. Symptom

- In multiplayer, a mod's cached value or UI shows a **stale value** while the host shows the current one.
- A value the mod just wrote **snaps back** to the previous value moments later (next replicated tick overwrote it).
- The same code path works fine in singleplayer — the bug only reproduces with a second player connected.

## 2. Cause

FishNet in Schedule I is strictly **server-authoritative** (`../schedule1-game-systems/references/01-FishNet-Networking.md`, §Server Authority): clients send requests via ServerRpc, the server validates and distributes state. Every `[SyncVar]` game field exists as a `SyncVar<T>` wrapper whose writes are funneled through native accessors and replicated to clients via `ReadSyncVar`. Consequence for mods:

- A mod that caches a game value client-side races the replication loop — the next replicated write wins and the cache goes stale.
- A mod that writes a SyncVar-backed value on a **client** gets overwritten (or silently stays local) depending on the field's `WritePermission`. Fields declared `WritePermission.ClientUnsynchronized` (documented game pattern) let the client change the local value while the server keeps its own state — guaranteed divergence between client view and server state.
- Mod logic running on **both** host and client additionally double-executes game writes (see `SKILL.md` §8 "Multiplayer Host Authority").

## 3. Verified decompile facts (grep evidence, 2026-10-05)

| Fact | Evidence |
|---|---|
| Game classes expose `SyncVar<T> syncVar____<field>` wrapper properties (e.g. `SleepController.syncVar____IsHostReadyToProceed_k__BackingField`, `EquippedItemHandler.syncVar____user`) | `Il2CppScheduleOne/GameTime/SleepController.cs:784`, `Il2CppScheduleOne/Equipping/EquippedItemHandler.cs:146,161` |
| Every SyncVar read/write goes through native accessor methods `sync___get_value__<field>` / `sync___set_value__<field>` (real method pointers in the interop proxies) | `SleepController.cs:632-634,975-976`, `EquippedItemHandler.cs:76-84,353-357` |
| Replication entry point per class: `public override bool ReadSyncVar___<ClassName>(PooledReader, uint, bool)` | `SleepController.cs:1255`, `EquippedItemHandler.cs:80` |
| The `Il2CppFishNet` decompile is **limited**: only 3 generated serializer files (`Serializing/Generated/Generated{Comparers,Readers,Writers}___Internal.cs`); the `SyncVar<T>` class itself and any `onChangeCallback` member are **NOT present** in the interop decompiles | directory listing + grep `class SyncVar` / `onChangeCallback` → 0 hits, 2026-10-05 |

## 4. Diagnostic pattern (proposed)

**Step 1 — Tag every log line with the network role** so host and client logs can be told apart:

```csharp
string role = FishNetClient.IsServerStarted ? "HOST" : (FishNetClient.IsClientStarted ? "CLIENT" : "SP");
Logger.Msg($"[{role}] value={x}");
```

**Step 2 — Log the SyncVar write path.** Two options:

- **Option A (unverified surface):** subscribe a logging callback on the SyncVar change notification (`syncVar.onChangeCallback`) as FishNet's C# API would allow. This is the cleanest hook, but the onChangeCallback member was **not found** in the limited interop decompile — verify it exists first via `ilspycmd -t` on the FishNet proxy assemblies under `MelonLoader/Il2CppAssemblies/` before relying on it.
- **Option B (verified hook points):** Harmony postfix on the native accessor `sync___set_value__<FieldName>` of the class you are diagnosing — every SyncVar write funnels through it (§3, row 2). Method names contain triple underscores; patch by exact name string. Expect the accessor to fire on **local** writes; whether it also fires on client-side replicated writes is **unverified** (§5).

**Step 3 — Interpret.** Reproduce with host + client open, compare timestamped pairs: if the client log shows your value written at T and the same field re-written at T+Δ by replication (no matching local write), the SyncVar server authority is the overwriter. If the host log never shows the client's write at all, the field is `ClientUnsynchronized` / host-authority-gated and the client write was always local-only.

## 5. Rules

1. **UI updates only after `OnLoadComplete`.** Never build UI from a cache filled before save data + replication settle (`../schedule1-troubleshooting/references/save-load-timing.md` §8.4: "Race with FishNet SyncVars … defer UI updates until OnLoadComplete").
2. **Host-authority gate game writes.** Wrap economy/world writes in `NetworkGuard.IsHostOrSingleplayer()` (schedule1-modding Rule 15, `../schedule1-game-systems/references/45-Multiplayer.md`) or the equivalent `IncomeEngine.IsHostOrSingleplayer()` check (SKILL.md §8).
3. **Don't cache SyncVar-backed values across frames.** Read through, or re-resolve caches on `OnLoadComplete` — a stale client cache is the symptom, not the bug.
4. **Treat `ClientUnsynchronized` writes as local-only.** A client-side write to such a field never becomes server state; don't design logic that assumes it replicated.

## 6. Open questions (unverified)

- Does the `SyncVar<T>` interop wrapper expose a subscribable `onChangeCallback` in the live proxy assemblies? Not decidable from the in-repo decompiles (§3, row 4) — needs an `ilspycmd` check against `MelonLoader/Il2CppAssemblies/`.
- Do Harmony patches on `sync___set_value__<field>` thunks fire for replicated client-side writes, or only for local writes? Untested.
- Exact replication tick rate and its ordering relative to `OnLoadComplete` — unmeasured.
- Is patching `ReadSyncVar___<Class>` viable for write interception, or does IL2CPP final/virtual stripping block it (compare `SKILL.md` §4)? Untested.

---

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `ClientUnsynchronized`
- `WritePermission`
