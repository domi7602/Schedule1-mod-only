# Working Across Devices

The repository is the single source of truth for everything that ships with the mods. This page describes what travels through git between machines (PC, laptop, …), what stays machine-local on purpose, and how to set up a second device so nothing is missing.

## What syncs through git

- All tracked files, full history, branches and tags.
- Both pinned submodules: `ThirdParty/S1API` (ifBars/S1API) and `ThirdParty/S1MAPI` (domi7602/S1MAPI). Both are public, so cloning needs no GitHub login.
- CI-enforced formatting, solution determinism and documentation checks reproduce on any clone.

Before switching devices, review the intended changes and explicitly decide what to commit
and push. Agents must wait for Dominik's authorization; there is no automatic end-of-session
commit or push. Uncommitted files, local-only commits and stashes do not travel to another
device through the remote. A successful local verifier does not mean changes are published.

## What is machine-local by design (never synced)

| Item | Why | How to regenerate |
|---|---|---|
| `ThirdParty/S1API/local.build.props`, `ThirdParty/S1MAPI/local.build.props` | Embed the local game path; git-ignored | `pwsh Tools/setup-workspace.ps1` |
| `GameReferences/decompiled/` | Large generated decompiles, licensing-sensitive | `pwsh Tools/bootstrap-game-references.ps1` (research only) |
| `.cache/tools/` (ilspycmd) | Tool cache | Installed automatically by `Tools/bootstrap-game-references.ps1` |
| `.scratch/` | Scratch notes by design | n/a |
| Build outputs (bin/obj folders) | Derived artifacts | `dotnet build` |
| The ZIPs under Release | Packaged artifacts | `pwsh Tools/package-release.ps1` |
| `core.hooksPath` git config | Per-clone configuration | `git config core.hooksPath .githooks` |
| Stashes and extra worktrees | Live inside one clone only | Include intended work in explicitly approved commits before synchronizing |

## New device checklist

Prerequisites (current versions: [compatibility](compatibility.md) · setup details:
[Requirements](../DEVELOPERS.md#requirements)): Windows 10/11, .NET SDK 8+ for C# 12,
the .NET 6 runtime for the mod/test host, PowerShell 7, Schedule I with MelonLoader,
and `SCHEDULE1_PATH` if the game is outside the default Steam location.

```pwsh
# 1. Clone with the pinned submodules (public repo, no login needed)
git clone --recurse-submodules https://github.com/domi7602/Schedule1-mod-only.git
cd Schedule1-mod-only

# 2. Generate the machine-local build props (prompts for the game path if needed)
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/setup-workspace.ps1

# 3. Optional: enable the pre-commit hook (format, version sync, doc paths)
git config core.hooksPath .githooks

# 4. Verify with deployment disabled; keep the game closed for Full/build checks
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify.ps1 -Mode Auto
```

Setup requires both pinned submodule templates and the local game references. It fails
with a concrete prerequisite error before writing props if required inputs are missing.
It does not build, install or start the game. For an unattended setup use
`-NonInteractive -GameDir <path>`; missing required input fails instead of prompting.
Hooks are optional: `-EnableHooks` enables them; otherwise noninteractive setup leaves
the hook configuration alone. For installation, follow [Build](../DEVELOPERS.md#build).

Verify the clone is complete:

```pwsh
git submodule status               # each line: no leading '-' (uninitialized) or '+' (mismatch)
git status                         # clean working tree
git log --branches --tags --not --remotes --oneline   # prints nothing when everything is pushed
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify.ps1 -Mode Quick
```

## Daily rhythm when switching machines

- **Before leaving a machine:** inspect `git status` and the commits ahead of the intended
  remote branch. Commit/push only the reviewed, authorized changes; retain unrelated work.
  Local scratch, backups and saves need their own intentional transfer if required.
- **After arriving on the other machine:** inspect local changes first, then synchronize
  with `git pull --ff-only` and `git submodule update --init --recursive`. Preserve local
  work if synchronization cannot proceed cleanly. Re-run setup when local paths change.

## Optional: sandboxed second workspace

`Tools/new-laptop-workspace.ps1` creates a new clone with recursive pinned submodules,
renames `origin` to `upstream`, optionally adds a backup remote named `origin`, and runs
setup. The name `upstream` is a workflow convention, **not a technical push restriction**.
Do not publish to either remote without explicit authorization.

For unattended use, supply `-TargetDir <new-directory> -NonInteractive` and the game path
when necessary. `-BackupUrl` and `-EnableHooks` are explicit options. Existing targets
are rejected without overwriting them. A failed new clone/setup is retained for diagnosis;
the script reports failure instead of announcing a complete workspace.

## Auditing repository completeness (maintainer)

To prove a clone is complete, compare a fresh clone against a known-good workspace:

```pwsh
git clone --recurse-submodules https://github.com/domi7602/Schedule1-mod-only.git $env:TEMP\s1-audit
(Compare-Object (git ls-files) (git -C $env:TEMP\s1-audit ls-files))   # no output = identical inventories
git -C $env:TEMP\s1-audit submodule status                              # matches the pinned commits
```

Both submodule commits must be reachable on their upstream remotes (they are always pushed together with a pointer bump in this repository).
