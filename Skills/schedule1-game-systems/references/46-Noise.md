# Noise System (Schedule I)

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 50/52 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep. Decompiles are IL2CPP interop stubs — hierarchy/signatures/CallerCount verified, method bodies not readable.

## Core Classes

| Class | Namespace | Verified Purpose |
|-------|-----------|------------------|
| `NoiseUtility` | `ScheduleOne.Noise` | Static emitter: `EmitNoise(Vector3 origin, ENoiseType type, float range, GameObject source = null)` |
| `NoiseEvent` | `ScheduleOne.Noise` | Payload: `origin`, `range`, `type`, `source`; property `OriginInSewer` (public get, private set) |
| `Listener` | `ScheduleOne.Noise` | MonoBehaviour on NPCs; static `List<Listener> listeners`; `Sensitivity`, `HearingOrigin`, `SquaredHearingRange` |
| `NPCAwareness` | `ScheduleOne.NPCs` | Bridges hearing → reactions: `NoiseEvent(NoiseEvent)` handler; holds `Listener` + `Responses` refs |
| `LocalPlayerFootstepGenerator` | `ScheduleOne.PlayerScripts` | Footstep emitter for local player, extends `GenericFootstepDetector` (`DistancePerStep`) |

## ENoiseType (complete — 3 values)

`Footstep`, `Gunshot`, `Explosion`

**NOT implemented:** no `Breaking`, no `Scream`, no `Vehicle` types. The old claim "Footstep, Gunshot, Breaking, etc." was wrong — only 3 enum values exist.

## Events (verified raising chain)

| Event | Raising / Handler |
|-------|-------------------|
| `Listener.onNoiseHeard` (`HearingEvent` delegate, 236 delegate-ctor sites) | Entry point `Listener.Notify(NoiseEvent)`; delegate raise happens natively |
| `NPCAwareness.onGunshotHeard` (`UnityEvent<NoiseEvent>`) | Raised from `NPCAwareness.NoiseEvent(NoiseEvent)` for Gunshot |
| `NPCAwareness.onExplosionHeard` (`UnityEvent<NoiseEvent>`) | Same, for Explosion |
| `NPCResponses.GunshotHeard/ExplosionHeard(NoiseEvent)` | Virtual reaction entry, overridden by `NPCResponses_Police` + `NPCResponses_CartelGoon` |

- **No footstep UnityEvent** on `NPCAwareness` — footsteps flow only through `Listener.onNoiseHeard`.
- **No global noise bus** — `NoiseUtility.EmitNoise` (2 native call sites) is the only emission API; noise is not serialized anywhere.

## Detection Model

- Per-listener: NPC reacts when `origin` lies within its `SquaredHearingRange` (weighted by `Sensitivity` — exact formula **unverified**, bodies are native).
- `NoiseEvent.OriginInSewer` is a dedicated flag — sewer origins get special treatment (see [`15-Sewer.md`](15-Sewer.md); range behavior unverified).
- `NoiseEvent` consumers: `NPCAwareness`, `NPCResponses`, `NPCResponses_Civilian`, `NPCResponses_Police`, `NPCResponses_CartelGoon`.
- Combined detection: police pursuit = Vision ([`62-Vision.md`](62-Vision.md)) + Noise + Law ([`12-Heat-Pursuit-Law.md`](12-Heat-Pursuit-Law.md)).
- `Player.Sneaky` (verified property) exists as drug-effect variable; its exact influence on footstep emission is **unverified**.

## Save participation

None — `NoiseUtility` is static; `NoiseEvent`/`Listener` are not `ISaveable` and never persist.

## Hook Points

1. **Prefix `NoiseUtility.EmitNoise`** — single global tap: log, filter or amplify all noise (static method, no instance needed).
2. **Prefix `Listener.Notify`** — per-NPC hearing control (silence specific NPCs, spoof events).
3. No patch needed: subscribe `NPCAwareness.onGunshotHeard` / `onExplosionHeard` UnityEvents at runtime.
4. **S1API:** `S1API.Entities.NPC.OnGunshotHeard` / `OnExplosionHeard` (typed `NPCNoiseEvent`: `Origin`, `Range`, `Type`, `OriginInSewer`) + `NPCNoiseType` enum — prefer these over direct patching.

---

---

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `Scream`
- `Breaking`
