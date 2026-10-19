# Upstream archive (historical reference)

These files came from the imported `LightControl-0.1.0-dev.1-DEVELOPMENT` archive
(`C:/Users/pc/AppData/Local/hermes/attachments/LightControl-0.1.0-dev.1-DEVELOPMENT.zip`). They are
kept for provenance and are **not active**: nothing here is built, tested or deployed by the
repository.

- `legacy-projects/` — the archive's three `.csproj` files, renamed to `*.csproj.txt` so the
  solution generator (`Tools/gen-sln.ps1`, which scans `Source/Mods` recursively) does not pick them
  up as live projects.
- `tools/`, `packaging/`, `tests/` — the archive's build/packaging scripts, file lists and Python
  packaging tests. They assume the old folder layout and are not claimed runnable here.
- `global.json`, `Directory.Build.props`, `NuGet.Config` — the archive's pinned .NET 6.0.428 SDK and
  its own build properties. The repository build now uses the central
  `Source/Mods/Directory.Build.props` and the user-local .NET 8 SDK.
- `*.md`, `*.txt`, `BUILD_ENVIRONMENT.json` — the archive's documentation and status reports.
- `CORE_MODEL_README.md` — the archive's detailed description of the pure model contract.
