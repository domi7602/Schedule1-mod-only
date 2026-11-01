# Agent workspace entry

The workspace is the repository containing this file. Read [AGENTS.md](AGENTS.md)
completely before editing mods; it owns the current rules, inventory and verification
workflow. Load the relevant [skills](Skills/README.md) as directed there.

On 2026-10-28 Dominik identified `C:/Users/pc/Schedule1-mod-only` as his normal working
repository. The external knowledge vault is connected through
[Shared knowledge vault](docs/knowledge-vault.md).

## Identify the current checkout

Use the actual working directory and `git rev-parse --show-toplevel`. On another
machine, the path or drive letter may differ. Check the current `git status` and
`git rev-parse HEAD`; a past snapshot description does not establish today's files.
Preserve uncommitted work and follow the commit/push rules in [AGENTS.md](AGENTS.md).

The game installation and saves are separate from this repository. Resolve them using
the [developer workflow](DEVELOPERS.md) and [device guide](docs/working-across-devices.md).
The vault's machine-local mapping identifies the chosen checkout; it does not change
the game's location or deployment configuration.

## Historical copy record from 2026-10-25

An earlier version of this note described a copy named `Schedule1-mod-only 10.25`
on an external drive. That description was found in the normal working repository,
so it must not be used to classify this directory as the external snapshot.
The following inherited details are preserved as historical claims, not a new
verification of the current checkout:

- Copy command: `robocopy /E /COPY:DAT /DCOPY:DAT /MT:16 /R:1 /W:1 /XJ`.
- Reported inventory: 48,602 files, 5,962 directories, 4.27 GiB; zero missing or extra
  files, SHA-256 mismatches, size mismatches or LastWriteTime mismatches.
- The record says Git metadata was included and this note was added after the comparison.
- Recorded branch: `main`; `HEAD`: `94a42c5` (commit about tightening AGENTS.md and
  recording S1Interop installation). It reported a push to `origin/main` and a passing
  CI run of 5m22s.
- Recorded submodules: S1API `a4cbdd9` (v3.2.1), S1MAPI `89b8d01` (v2.0.1).
- Recorded remote: `https://github.com/domi7602/Schedule1-mod-only.git`.
- The record identifies a local-only tag `archive/tests-core-logic` containing 37
  archived commits. Preserve it; do not push or delete it without an explicit request.

The external copy's present contents and location were not verified while connecting
the knowledge vault. Use current repository evidence for development.
