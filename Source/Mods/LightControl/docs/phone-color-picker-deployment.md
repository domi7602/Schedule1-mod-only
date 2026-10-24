# LightControl UI deployments

## Latest installation — 0.1.0, 2026-10-24

The approved graphite native port is installed. Version 0.1.0 is the stable release of the
owner-accepted dev.19 code (version metadata only). Its checked source/build identity, installed
hashes and deployment receipt are recorded in
[graphite-ui-verification.md](graphite-ui-verification.md). Native appearance and hit testing are
owner-accepted 2026-10-24 (not itemized).

## Historical installation — 0.1.0-dev.19, 2026-10-22

## Historical installation — 0.1.0-dev.18, 2026-10-22

Dominik approved installing the existing V4 candidate after reviewing screenshots of dev.17's
lamp list and missing color-picker footer. The exact retained DLL/PDB/metadata were installed
through `Tools/deploy-mod.ps1` with the game closed. No rebuild was performed.

Source: `%LOCALAPPDATA%/Schedule1Tools/VerifiedBuilds/LightControl/0.1.0-dev.18-v4-professional-list/`.
Before deployment, all three artifact hashes, all 20 production source/project hashes and the
archived source snapshot matched the retained manifest. Installed Shared matched the recorded
baseline. The explicit plan and successful receipt contained exactly these three targets:

| Target under the game installation | SHA-256 independently read back after installation |
|---|---|
| `Mods/LightControl.dll` | `4782502d543e7dc7b7dc6d255bcadc6900af80d5df3c77829fa223c9a8dd0448` |
| `UserData/LightControl/LightControl.pdb` | `501a84476a95c6d35687a3fb875c8202b3a40816f0ff8131599a8dea3075cbdc` |
| `UserData/LightControl/mod.json` | `cf060a9e4a0f194b3ab5b1d9e13c43e89d2840abbf6ba0bfb07b3d306d3202ae` |

The helper reported **Copied 3, Unchanged 0**. Source/destination paths and hashes were checked
against the receipt; the latest and attempt receipt identities matched. All three previous-file
backups were independently hash-checked against the pre-deployment files. The previous DLL was
dev.17, hash `f0d41f141ffb74da7f2de9cab028a57aafe5f547e77d8117a8061a8df82a4b21`.

- Game installation: `C:/Program Files (x86)/Steam/steamapps/common/Schedule I`.
- Attempt: `20261022T044940332Z-1273356dc26c`.
- Latest receipt: `<Game>/UserData/.deploy/receipts/LightControl.json`.
- Attempt receipt: `<Game>/UserData/.deploy/receipts/LightControl.20261022T044940332Z-1273356dc26c.json`.
- Verified backup: `<Game>/UserData/.deploy/backups/20261022T044940371Z-LightControl-2850a20c/`.

The final preflight confirmed the game was still closed; installed Shared retained its prior hash.
No game launch, save mutation or commit was performed. The older full-install/save witness counts
below belong only to the dev.17 deployment; they were not re-measured for this installation.

The retained metadata and build manifest keep their original compile-only labels byte-for-byte;
this record supersedes those labels for current installation status. The existing 253 checks and
clean Release build are retained build evidence, not new test runs. **In-Game-Verify remains open**:
check the lamp table, visible/clickable Back/Select buttons and `[ui-layout]` measurements in the
next game session using [the acceptance checklist](phone-color-picker-verification.md).

## Historical compact picker deployment — 0.1.0-dev.17

### Installed after separate owner approval — In-Game-Verify open

On 2026-10-21 Dominik approved installing the exact retained dev.17 candidate, without rebuilding:
LightControl DLL, PDB and metadata only, with backups of the previous files. Shared and game saves
were outside the write scope. No game launch, live mutation, dependency update, commit or push was
performed. Installation does not establish native UI/rendering acceptance.

The original implementation, review and compile evidence remain in
[phone-color-picker-verification.md](phone-color-picker-verification.md). The accepted behavior is
[phone-color-picker-design.md](phone-color-picker-design.md).

## Artifact and deployment scope

Source directory:
`%LOCALAPPDATA%/Schedule1Tools/VerifiedBuilds/LightControl/0.1.0-dev.17-compact-picker/`

Before copying, the parent re-read all three artifact hashes, the 20 production source inputs,
the archived source snapshot and the 184 pinned reference hashes. The actual repository preflight
confirmed the resolved game installation and that the game was closed. The explicit deployment plan
contained exactly these three runtime/metadata targets:

| Target under the game installation | SHA-256 read back after installation |
|---|---|
| `Mods/LightControl.dll` | `f0d41f141ffb74da7f2de9cab028a57aafe5f547e77d8117a8061a8df82a4b21` |
| `UserData/LightControl/LightControl.pdb` | `742e644009df7c4366ec45ecb2a9e15650aa7b9073406db0a983ab6951626e28` |
| `UserData/LightControl/mod.json` | `812674619d6131ecddba65c9cb2eb69dde92a6597b2b55ff168a511c41c1771a` |

`Tools/deploy-mod.ps1` was invoked directly with the retained folder as ProjectDir, explicit retained
DLL/PDB/metadata paths, ProjectName LightControl and Configuration Release. No AssetsDir or build was
used. The helper performed its own game-closed preflight under its per-install mutex, backed up all
three replaced files, and reported **Copied 3, Unchanged 0**. Backups and deployment receipts are
additional audit files, not extra mod/library deployments.

The parent then separately read the installed targets and both receipt copies, verified the exact
source/target path set and hashes, and verified all three backup contents against the before hashes.
The installed DLL's metadata was read without executing the mod: MelonInfo version **0.1.0-dev.17**,
`LightControl.Core.UiSelectionPolicy/ColorPickerState` present, and
`LightControl.LightControlApp::ApplyBrightnessStep` present.

## Receipt, backup and persistent evidence

- Game installation: `C:/Program Files (x86)/Steam/steamapps/common/Schedule I`.
- Successful attempt: `20261021T080941733Z-616873812034`.
- Latest receipt: `<Game>/UserData/.deploy/receipts/LightControl.json`.
- Attempt receipt: `<Game>/UserData/.deploy/receipts/LightControl.20261021T080941733Z-616873812034.json`.
- Verified previous-file backup: `<Game>/UserData/.deploy/backups/20261021T080941771Z-LightControl-5f6339f1/`.
- Persistent before/readback/receipt/backup-manifest evidence:
  `%LOCALAPPDATA%/Schedule1Tools/LightControlDeployments/20261021T080906Z-dev17-picker/`.

The old DLL was dev.16, SHA-256 `a0697ef72100891f31149dbd341af7f136e02d02678c05f696203a561791f344`.
Its DLL/PDB/metadata backup was hash-verified before replacement and again during the parent readback.
The helper provides bounded in-process rollback, not automatic crash/power-loss recovery.

## Unchanged witnesses

Before/after file sets and contents matched exactly:

- **76** unrelated mod/library/plugin binary/asset files, including installed Shared.
- **4** LightControl persistence/config sidecar files, excluding the intentionally replaced PDB/metadata.
- **125** native save files below the actual user-local Schedule I Saves root.

Built and installed Shared matched before installation; the installed Shared hash remained unchanged.
No new save, save backup, sidecar mutation or game-start operation was used as part of this install.
The authoritative helper preflight also confirmed the game was still closed after readback.

## Metadata and acceptance boundary

The retained `mod.json` and frozen build manifest keep their original compile-only/not-installed
labels byte-for-byte. Those labels describe the artifact's creation state; this verified deployment
record supersedes them for current installation status. Do not edit the installed metadata or rebuild
merely to change that historical label. The repository README/inventory/compatibility rows now state
installed, while keeping **In-Game-Verify open**.

The 251/251 core/adapter/source checks, clean forced Release and independent review PASS belong to
this same retained artifact. They were not rerun through a new build during deployment. No new runtime
load, clipping/hit-testing, phone reopen, native color/brightness, persistence or teardown acceptance
is claimed. Use the singleplayer checklist in the verification record for the owner's next session.
