# Product Context — Schedule1-mod-only

> **Single Source of Truth ist `AGENTS.md`.** Diese Datei ist eine session-orientierte Orientierung für Agenten, **kein** autoritatives Inventar. Bei Widerspruch gilt `AGENTS.md` (bzw. der Code selbst). Versionen hier nur als Momentaufnahme (2026-09-19).

---

## Was ist das?

Privates Modding-Workspace für **Schedule I** (TVGS) — eine Sammlung eigener MelonLoader-Mods plus eine geteilte Library, geführt als einzelnes Git-Repo (`github.com/domi7602/Schedule1-mod-only`).

| Eigenschaft | Wert |
|---|---|
| **Spiel** | Schedule I v0.4.6f13 (Steam, Default Branch) |
| **Runtime** | Unity 2022.3, **IL2CPP** (primär), Mono (Alt-Branch — siehe AGENTS.md §4) |
| **Loader** | MelonLoader 0.7.3 |
| **TFM / Sprache** | `net6.0`, `LangVersion` 12, `Nullable` enabled |
| **Repo-Position** | `C:\Users\pc\Schedule1-mod-only` — **außerhalb** des Spielverzeichnisses |
| **Spielpfad-Auflösung** | `$env:SCHEDULE1_PATH`, Fallback `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` |

## Wer arbeitet damit?

**Dominik** (Owner/Entwickler) — Solo-Entwickler, kein Team. Der Agent (Cline) ist der ausführende Engineer. Sprache: Deutsch. Qualitätsanspruch: "5/5 Sterne, keine halben Sachen", aber Ehrlichkeit über "In-Game-Verify offen" statt vorgetäuschter Perfektion.

## Warum (Zielbild)

Stabile, wartbare und **im Spiel verifizierte** Mods. Nicht maximale Mod-Anzahl, sondern saubere Architektur (Shared Library statt Mod-Duplikate), defensive IL2CPP-Praxis und dokumentierte Drift-Guards.

## Repository-Struktur (Kurzfassung)

| Pfad | Zweck |
|---|---|
| `Source/Mods/` | 15 aktive Mods + `Shared` + `S1Mods.sln` |
| `Source/Archive/` | Archivierte Mods — **nicht bauen, nicht deployen** |
| `Source/Tests/` | xUnit-Projekte (Shared, AutoPackagingStation, CalculatorApp, BackpackMod) |
| `GameReferences/` | Lokale Decompiles (gitignored, nur lokal vorhanden) |
| `ThirdParty/` | Gepinnte Dependencies (S1API, S1MCP, Archive) — **read-only für Mod-Code** |
| `Tools/` | 14 PowerShell-Skripte (Build, Release, Guards, Setup) |
| `Skills/` | 20 Agent-Skills (nach Task laden, `schedule1-modding` immer zuerst) |
| `docs/` | `architecture.md`, `release-process.md`, `pitfalls.md` |
| `memory-bank/` | Session-Kontext (diese Dateien) |

## Aktive Mods (Snapshot 2026-09-19)

| Mod | Version | Domäne |
|---|---|---|
| NotesApp | 1.0.3 | PhoneApp (Notizen) |
| PotScanner | 0.5.4 | PhoneApp (Property/Growing) |
| CalculatorApp | 0.2.3 | PhoneApp (Rechner) |
| CustomSkateboard | 1.1.5 | Gameplay (Skating) |
| MoreSaveSlots | 1.0.12 | QoL (Speicherstände) |
| PocketShop | 0.2.7 | PhoneApp (Shop, Multi-Payment) |
| BankApp | 0.4.4 | PhoneApp (Banking) |
| BusinessIncome | 0.1.6 | Economy (passives Einkommen) |
| StackLimitMod | 0.1.4 | Items (Stack-Limits) |
| AutoPackagingStation | 0.2.8 | Gameplay (industrielle Packaging-Line) |
| HitmanPhone | 0.2.9 | PhoneApp (Contracts/Quests) |
| _DiagPerfCounter | 0.3.2 | Dev-Tool (nicht in Releases) |
| Shared | — | Workspace-Library (nicht in Releases) |
| S1MCP | 1.0.1 | Drittanbieter-Infrastruktur (TCP :8765) |

Archiviert/nicht aktiv: DayCounter, HomelessMod, Minimap, ProfitTracker, TVBrowser, BackpackMod, SnackVendor. Entfernt: PhoneScroll, MoreDrugs, Herer's Minimap.

## Shared Library (`Source/Mods/Shared/`)

Zentrale Bausteine, die **jeder Mod** nutzen soll statt Eigenbau:

`PatchGuard` · `SafeStorage` (atomic + `.bak`) · `SafeInvoker` · `GameObjectResolver` · `HotkeyManager` · `ModConfig<T>` · `ModLogger` · `NetworkGuard` · `SceneGate` · `TypeResolver` · `UITheme` (Method 3, `Sp`/`Dp`)

## Nicht verhandelbare Constraints (Kurzfassung)

1. **Deploy-Konvention:** DLL/PNG/Bundle → `<GameDir>\Mods\`; `mod.json` + `.pdb` → `<GameDir>\UserData\<Mod>\`. **Niemals** JSON/PDB nach `Mods\`.
2. **IL2CPP-Disziplin:** `IntPtr`-Ctor für `[RegisterTypeInIl2Cpp]`, `EventHelper`/`ButtonUtils` statt `UnityAction`, kein `foreach`/LINQ auf Il2Cpp-Listen, `WasCollected`/`IsAlive`-Guards.
3. **Host-Authority:** MP-relevante Economy-Änderungen nur mit `NetworkGuard.IsHostOrSingleplayer`.
4. **Slot-Isolation:** `slot_{n}`, kein `slot_-1.json`.
5. **Archivierte Mods** (`Source/Archive/`) werden nicht gebaut. `Shared`/`_DiagPerfCounter` kommen nicht in Releases.
6. **ThirdParty/** ist read-only ohne explizite Anweisung.
7. **Keine** Commits/Pushes ohne explizite Aufforderung; keine Secrets; keine CI-Guard-Umgehungen.
