# Active Context — Schedule1-mod-only

**Stand:** 2026-09-19
**Branch:** `main` (synchron mit `origin/main`)

---

## Letzte Session (2026-09-19)

### Erledigt

1. **Push:** 2 lokale Commits auf `origin/main` gepusht
   - `dc5022d` docs(repo): 5-Star-Polish — THIRD-PARTY-NOTICES.md, assets/ Screenshot-Konventionen, Test/License-Badges, s1interop CI-Aktivierungs-Checkliste
   - `1d40500` fix(AutoPackagingStation): v0.2.8 — 2x2-Footprint, InteractableObject für E-Taste, Hustler-I-Rank-Gate

2. **Release-Packaging (nur lokal, kein Nexus-Upload):** 4 Zips in `Release/`
   - `AutoPackagingStation-v0.2.8.zip` (59,2 KB)
   - `PocketShop-v0.2.7.zip` (87,1 KB)
   - `BusinessIncome-v0.1.6.zip` (37,8 KB)
   - `StackLimitMod-v0.1.4.zip` (27,2 KB)
   - Deploy-Konvention in allen Zips verifiziert (DLL/PNG → `Mods/`, mod.json + pdb → `UserData/<Mod>/`, Docs im Root)
   - Version-Sync vor Lauf geprüft: 11 Mods, 0 Drift

### Gelernt / Pitfall

- `package-release.ps1` **nicht parallel** für mehrere Mods ausführen — File-Locks auf geteilten `Shared`-Build-Outputs lassen Builds fehlschlagen. Entweder seriell pro Mod oder `-Mod All` in einem Lauf (intern seriell).

---

## Offene Punkte

| Thema | Details |
|---|---|
| **s1interop CI-Aktivierung** | Checkliste in `dc5022d` angelegt (`.github/workflows/ci.yml`), Umsetzung offen |
| **Memory Bank aufbauen** | `memory-bank/` war leer; `activeContext.md` mit dieser Session begonnen |

## Verifikations-Stand (AGENTS.md)

Alle 11 aktiven Mods + Shared auf `Verified`-Stand. Zuletzt verifiziert 2026-09-19:
- AutoPackagingStation v0.2.8 (Bugfix-Runde 7)
- PocketShop v0.2.7 (Level-Lock & Inline-Qty)
- BusinessIncome v0.1.6 (0-Business Backlog-Fix)
- StackLimitMod v0.1.4 (Agriculture-Only & Weapon-Shield)

## Entscheidungen

- Release-Zips werden **nur lokal** bereitgestellt, kein Nexus-Upload (Dominik, 2026-09-19).
