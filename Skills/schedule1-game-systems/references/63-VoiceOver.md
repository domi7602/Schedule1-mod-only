# VoiceOver (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): NPC behaviour + the VO emitter/type content lives in **[`14-NPC-Behaviour.md`](14-NPC-Behaviour.md)** (VO-Types section). This file keeps only the class register. Class-list only — not yet re-verified against 0.4.7f9.

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
