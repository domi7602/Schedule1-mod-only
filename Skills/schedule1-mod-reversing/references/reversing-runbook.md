> verified 2026-10-20 — procedure executed end-to-end on three third-party DLLs (see `case-notes.md` for the per-mod evidence). Decompile, hook inventory and compatibility checks are confirmed; no in-game behaviour was tested.

# Reversing Runbook — commands and interpretation

Workspace root assumed. Shell is bash (git-bash on Windows) unless stated. Native tools
(`ilspycmd`, `git`, `node`) cannot read `/tmp` or MSYS `/c/...` paths — pass
`C:/Users/...`-style forward-slash paths to them.

## 1. Inventory

```bash
python -c "
import zipfile
z=zipfile.ZipFile(r'C:/Users/pc/Downloads/SomeMod.zip')
for i in z.infolist(): print(f'{i.file_size:>9} {i.filename}')
"
```

Read three things off the listing:

- **One entry vs. many** — a bare `SomeMod.dll` is a self-contained mod; a `Mods/` prefix or a
  model/asset folder means the mod expects files next to it (document that, it is an install
  requirement).
- **Size bands** — < 200 KB: readable C#. > 1 MB: check for embedded resources first (icons,
  sounds, design blobs); the logic is usually not proportionally bigger.
- **Companion files** — `README.txt`, `Models/`, `UserLibs/` hints. The README is a marketing
  document: useful for feature names, never for behaviour claims.

## 2. Extract into a durable folder

Scratch directories are pruned; analysis output must survive.

```bash
BASE="C:/Users/pc/Documents/Schedule1-Mod-Reversing"
D="$BASE/<ModName>-<version>"; mkdir -p "$D"
cp "/c/Users/pc/Downloads/<downloaded>.zip" "$D/"
python -c "import zipfile; zipfile.ZipFile(r'$D/<downloaded>.zip').extractall(r'$D/unpacked')"
sha256sum "$D"/unpacked/**/*.dll
```

Record the SHA-256 in the analysis header: it is the only unambiguous way to say later *which*
build was analysed.

## 3. Decompile

```bash
export DOTNET_ROOT="$USERPROFILE/.dotnet"; export PATH="$DOTNET_ROOT:$PATH"
ilspycmd -p -o "$D/src" "$D/unpacked/SomeMod.dll"
find "$D/src" -name "*.cs" | wc -l
wc -l "$D/src"/*/*.cs | sort -n | tail -20
```

`-p` emits a whole project (`.csproj` + one file per type). The generated `.csproj` is deliberate
evidence:

| What it shows | Reading |
|---|---|
| `TargetFramework` | `net472` = older MelonLoader generation; `netcoreapp6.0` = current, Il2CppInterop |
| `<Reference Include="…">` list | which assemblies it assumed: `Assembly-CSharp`, `FishNet.Runtime`, `S1API`, `Il2CppInterop.Runtime` |
| `<EmbeddedResource …LogicalName=…>` | assets shipped inside the DLL (this is why a 1.9 MB DLL can have 21k lines) |
| `AllowUnsafeBlocks` | almost always true for IL2CPP mods, not a signal by itself |

If `find … -name "*.cs" | wc -l` returns a handful of files with multi-hundred-KB size, the mod
is either asset-heavy or machine-generated — check `grep -c "class "` before reading.

## 4. Triage: what to read, what to skip

Read in this order (stop when the story is clear):

1. `Mod.cs` / `Main.cs` — `OnInitializeMelon` holds the patch registrations; `OnUpdate` holds
   the tick order.
2. The file with the most lines **that is not UI** — that is where the domain logic lives.
3. The "rules" file, if one exists (pure C# without Unity types: `…Rules.cs`, `…Config.cs`,
   `…State.cs`). This layer is where the formulas are and the only layer that is testable.
4. `README` claims → verify each against code; note the ones the code does not support.

Skip on first pass: UI layout files, mesh/model builders, animation rigs. Note their size and
purpose, move on. They are the bulk of a big mod's lines and the least transferable knowledge.

## 5. Hook inventory

```bash
cd "$D/src"
grep -nE "Patch\(typeof|AccessTools\.(Method|PropertyGetter|Field)|HarmonyPatch\(" */*.cs | sed 's/^/  /'
grep -c "Patch(typeof" */*.cs
```

Build a table: **target → hook kind → effect**. Two derived signals:

- **Hot-path hooks:** anything named `Update`, `LateUpdate`, `get_Name`, `get_Price`, a per-item
  UI refresh, or a registry lookup. These run continuously; flag them for measurement.
- **Fragile targets:** hash-suffixed (`…_2166136261`), private field names, compiler-generated
  types. Rank them; they are what breaks at the next update.

Dynamic patching (scanning assemblies and patching by method name) shows up as a loop over
`AppDomain.CurrentDomain.GetAssemblies()` + `type.GetMethods(...)` and a `harmony.Patch` inside.
Note the name patterns it matches — that *is* the hook inventory for such a mod.

## 6. Compatibility check against the current runtime

This is the step that converts an analysis into a finding. `GameReferences/decompiled` is the
local decompile of the installed `Assembly-CSharp` (regenerate with
`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/bootstrap-game-references.ps1`).

```bash
cd /c/Users/pc/Schedule1-mod-only/GameReferences/decompiled
check() { printf "%-58s " "$1"; grep -rqE "$2" --include="*.cs" . && echo OK || echo MISSING; }
check "PlayerHealth damage RPC"     "TakeDamage_3505310624"
check "Player arrest RPC"           "Arrest_Client_2166136261"
check "ShopListing.Price"           "public unsafe float Price"
check "ListingUI.Update"            "public unsafe void Update\(\)"
check "GoonPool.goons (private)"    "NativeFieldInfoPtr_goons"
```

Rules for reading the result:

- **OK for every anchor** → the mod initialises on this runtime. Say exactly that, nothing more.
- **MISSING for a hash-suffixed RPC** → the update relocated it. If the mod throws
  `MissingMethodException` on it, it will not load at all; if it only checks for null, the feature
  is gone.
- **MISSING under dynamic patch-by-name** → no crash, silently reduced capability. Name the lost
  capability, not just "one more missing".
- **Compiler-generated names** (`…ObObObUnique`, `_PlayMethAnimation_d__27`): verify them
  explicitly; they are the first casualty of any rebuild, even when everything else still matches.

Also check the live install before claiming anything about the user's setup:

```bash
ls "/c/Program Files (x86)/Steam/steamapps/common/Schedule I/Mods/" | grep -i <name>
```

An installed DLL changes the risk conversation: it is running right now, whether or not the
analysis is finished.

## 7. Report

One Markdown file per mod under `Documents/Schedule1-Mod-Reversing/<Mod>-<Version>/ANALYSIS.md`
with the sections from `SKILL.md` §6. Include `file:line` for every risk and every formula.
Keep the chat answer short: identity, three findings that matter, risks, reusable patterns,
and the open in-game questions. The document is the deliverable; the chat message is the digest.

## Pitfalls

- **`strings` is not on this host.** Grepping a DLL for type names with `grep -c` works (raw byte
  match on the metadata heap); do that instead of `strings`/`dotnet-ildasm`.
- **IL2CPP decompiles look wiry.** `il2cpp_runtime_invoke` stubs mean the *real* implementation
  is native — behaviour questions ("does `Add` replace or append?") cannot be answered from the
  decompile. Say so instead of guessing.
- **`object __instance, object[] __args` prefixes** are legitimate: one handler for many
  signatures. Do not mistake them for reflection voodoo.
- **Version claims drift.** A `README` header, an `AssemblyVersion` and a hardcoded
  `MelonLogger.Msg("v… loaded")` string can disagree inside one DLL. Only `AssemblyVersion` /
  the file version resource is a source of truth; a log string is whatever the author last
  edited.
- **Do not read the whole mod.** 21k lines of somebody else's mesh code teaches nothing
  transferable. Read the entry point, the biggest logic file, the rules file, and the patches.
