# Game References

`decompiled/` is generated locally from the installed Schedule I game assemblies and is intentionally not versioned. It is a research aid, not a build input.

Generate or refresh it after a game update:

```pwsh
pwsh Tools/bootstrap-game-references.ps1
```

The script reads `<GameDir>\MelonLoader\Il2CppAssemblies`, installs `ilspycmd` into the ignored `.cache/tools/` directory when needed, and writes decompiles to `GameReferences/decompiled/`.

Use `$env:SCHEDULE1_PATH` to target a non-default game installation. Do not commit generated output.
