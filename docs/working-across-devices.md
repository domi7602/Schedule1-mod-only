# Working Across Devices

The repository is the single source of truth for everything that ships with the mods. This page describes what travels through git between machines (PC, laptop, …), what stays machine-local on purpose, and how to set up a second device so nothing is missing.

## What syncs through git

- All tracked files, full history, branches and tags.
- Both pinned submodules: `ThirdParty/S1API` (ifBars/S1API) and `ThirdParty/S1MAPI` (domi7602/S1MAPI). Both are public, so cloning needs no GitHub login.
- CI-enforced formatting, solution determinism and documentation checks reproduce on any clone.

**Rule:** finish every session with `git add -A`, `git commit` and `git push`. Uncommitted changes, local-only branches and stashes exist only on the machine that created them — they never appear on the other device.

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
| Stashes and extra worktrees | Live inside one clone only | Commit and push instead of stashing |

## New device checklist

Prerequisites (current versions: [`docs/compatibility.md`](compatibility.md) · setup details: [Requirements](../DEVELOPERS.md#requirements)): Windows 10/11, a .NET SDK that can build the mod target framework, PowerShell 7, Schedule I with MelonLoader installed, and `$env:SCHEDULE1_PATH` set if the game is not in the default Steam location.

```pwsh
# 1. Clone with the pinned submodules (public repo, no login needed)
git clone --recurse-submodules https://github.com/domi7602/Schedule1-mod-only.git
cd Schedule1-mod-only

# 2. Generate the machine-local build props (prompts for the game path if needed)
pwsh Tools/setup-workspace.ps1

# 3. Optional: enable the pre-commit hook (format, version sync, doc paths)
git config core.hooksPath .githooks

# 4. Build (deploys to the game directory; add -p:S1NoDeploy=true to compile only)
dotnet build Source/Mods/S1Mods.sln -c Release
```

Verify the clone is complete:

```pwsh
git submodule status               # each line: no leading '-' (uninitialized) or '+' (mismatch)
git status                         # clean working tree
git log --branches --tags --not --remotes --oneline   # prints nothing when everything is pushed
pwsh Tools/check-doc-paths.ps1     # documentation integrity
```

## Daily rhythm when switching machines

- **Before leaving a machine:** commit and push everything (`git status` clean, `git log origin/main..HEAD` empty). Do not rely on stashes.
- **After arriving on the other machine:** `git pull --ff-only`, then `git submodule update --init --recursive` if the pull moved a submodule pointer; re-run `Tools/setup-workspace.ps1` only when the game path changed.

## Optional: sandboxed second workspace

`Tools/new-laptop-workspace.ps1` clones the repository into a second, independent workspace and renames the remote from `origin` to `upstream`, so accidental pushes to the main repository are physically impossible. Use it for experiments; cherry-pick or merge useful commits back through a `git push` from the main workspace.

## Auditing repository completeness (maintainer)

To prove a clone is complete, compare a fresh clone against a known-good workspace:

```pwsh
git clone --recurse-submodules https://github.com/domi7602/Schedule1-mod-only.git $env:TEMP\s1-audit
(Compare-Object (git ls-files) (git -C $env:TEMP\s1-audit ls-files))   # no output = identical inventories
git -C $env:TEMP\s1-audit submodule status                              # matches the pinned commits
```

Both submodule commits must be reachable on their upstream remotes (they are always pushed together with a pointer bump in this repository).
