# VoiceOver (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): NPC behaviour + the VO emitter/type content lives in **[`14-NPC-Behaviour.md`](14-NPC-Behaviour.md)** (VO-Types section). This file keeps only the class register. Class-list only — not yet re-verified.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Core Classes

| Class | Purpose |
|-------|---------|
| `VODatabase` | VoiceOver database (all entries, organized by NPC/category, random selection) |
| `VODatabaseEntry` | Individual line (audio clip + text) |
| `VOEmitter` | Plays VO on NPC (3D positioned, distance volume, priority interrupts) |
| `EVOLineType` | Line types (Response, Idle, Alert, …) |
| `PoliceChatterVO` | Police radio chatter (pursuit) |

## Triggers

Dialogue events · pursuit police chatter · interaction responses · cartel goon threats.
