# Third-Party Mod Triage — decompile, map, compatibility-check

> UNVERIFIED against the installed runtime. Static method note: it records the order of work that
> proved reliable, not runtime behaviour. Proven on two downloaded mods inspected 2026-10-19 (see
> `third-party-mod-patterns.md` for the findings). Re-verify every identifier against the current
> game before relying on it.

## When to use

- A downloaded mod must be assessed before it is copied into `Mods\`.
- A third-party mod must be compared with one of ours for patch overlap.
- A mod's behaviour must be explained from the binary alone (no install, no game session).

## 0. Triage order

Do these in sequence. The point of the order is that steps 1–4 are cheap and often answer the
question, while step 5 is expensive and usually unnecessary.

1. **Inventory the package** — what ships besides the DLL (models, textures, extra DLLs)?
2. **Copy to an ASCII scratch path** — native tools cannot read odd filenames or `/tmp`.
3. **Identify the assembly** — mod name, author, declared version, target framework.
4. **Structural sweep** — types, Harmony targets, settings, item IDs, user-facing strings.
5. **Targeted body reads** — only the classes the sweep says matter.
6. **Risk scan** — network, process, registry, file IO, P/Invoke.
7. **Compatibility check** — compare patch surface and relevance gates with our mods.

## 1. Package inventory

List the archive before extracting, and note the target folder of every entry. A mod that ships
models or textures into `Mods\<Mod>\` behaves differently from a single-DLL mod: the DLL alone is
not a complete install.

```bash
python -c "
import zipfile, sys
z = zipfile.ZipFile(sys.argv[1])
for i in z.infolist(): print(f'{i.file_size:>10}  {i.filename}')
" 'C:/path/to/Mod.zip'
```

Watch for three things: a `UserLibs\` entry (a reference assembly, see §7), GLB/asset folders that
the code loads at runtime by path, and any second DLL that is itself a mod.

## 2. Scratch path and decompile

Native tools (ilspycmd, git, node) do not resolve MSYS paths and cannot read a bare `/tmp`. Copy
the DLL to an explicit Windows scratch path first, and strip non-ASCII characters from the name:

```bash
SCRATCH="/c/Users/<user>/AppData/Local/hermes/cache/scratch/<mod>"
mkdir -p "$SCRATCH" && cp "$HOME/Downloads/<Mod>.dll" "$SCRATCH/Mod.dll"
export DOTNET_ROOT="C:/Users/<user>/.dotnet"
export PATH="C:/Users/<user>/.dotnet:C:/Users/<user>/.dotnet/tools:$PATH"
ilspycmd "$SCRATCH/Mod.dll" -o "$SCRATCH/decomp" --nested-directories
```

`--nested-directories` gives one file per type. For large mods ILSpy may still emit a single flat
file — both shapes are fine for the sweep, but check the file count before assuming a layout.

## 3. Cheap structural sweep

Never read a large decompiled file top to bottom. A 60k-line assembly will flood any context. Use
Python to pull the structure out and print only the summary:

```python
import re
src = open(F, encoding='utf-8-sig').read()
lines = src.split('\n')

# assembly identity
[l for l in lines[:200] if '[assembly:' in l or l.startswith('using ')]

# type inventory with line numbers
decl = re.compile(r'^\s*(?:\[[^\]]*\]\s*)?(?:public|internal|private|protected|static|sealed|abstract|partial|\s)*\b(class|struct|enum|interface)\s+([A-Za-z_][\w]*)')
[(i+1, m.group(2)) for i, l in enumerate(lines) if (m := decl.match(l)) and 'namespace' not in l]

# patch surface
[(i+1, l.strip()) for i, l in enumerate(lines) if 'HarmonyPatch(' in l]

# settings surface
[(i+1, l.strip()) for i, l in enumerate(lines) if 'CreateEntry<' in l or 'CreateCategory' in l]

# user-facing strings (messages, instructions, item names)
sorted(set(re.findall(r'"([^"]{18,170})"', src)))
```

The user-facing strings are the highest-value output of the whole sweep: they usually state the
mod's actual rules (prices, durations, quality tiers, unlock requirements) more precisely than the
code does.

For readability, write a second copy with the IL annotations stripped — they are typically 20–25%
of the lines and carry no meaning for a behavioural read:

```bash
grep -v '^\s*//IL_' decomp/Mod.decompiled.cs > decomp/Mod.clean.cs
```

## 4. Targeted body reads

Read bodies only for the classes the sweep flagged. Useful per-class summaries:

- method signatures + line numbers, to find the entry points,
- `new GameObject("…")` / `Child(…, "…")` names, for a built-object inventory,
- `Kit.`-style helper calls, for a construction vocabulary,
- `ModelRoot.Load(…)` / asset loads, for external file dependencies,
- `switch`/dictionary literals, for id → behaviour tables.

## 5. Risk scan

Before recommending an install, scan for the categories that matter:

```python
pats = {
 'network': r'HttpClient|WebClient|WebRequest|Socket|TcpClient|UnityWebRequest|DownloadString',
 'process': r'Process\.Start|ProcessStartInfo|cmd\.exe|powershell',
 'registry': r'Microsoft\.Win32\.Registry|RegistryKey',
 'file-write': r'File\.WriteAll|StreamWriter|File\.Delete|File\.Move|Directory\.Delete',
 'native': r'DllImport|GetProcAddress',
}
```

Report the hits and where the files go. `DllImport` on `user32`/`kernel32` is usually clipboard or
window handling, not a red flag; a file write into `UserData\` is normal mod behaviour. Say plainly
that the scan is pattern-based and does not prove the absence of anything.

## 6. Compatibility check against our mods

The decisive question is not "does it patch the same class" but **"does it patch it for the same
instances"**. Work through this:

1. **Patch surface overlap.** Compare the third-party `HarmonyPatch` targets with our mods' targets.
2. **Relevance gates.** Many well-built mods patch a vanilla class but return early unless the
   object is theirs — an item-id check in a slot, an ownership flag, a feature-unlock flag. A gate
   is what decides whether the overlap is real. Read the gate, not the patch attribute.
3. **Which side is additive.** A mod that only *reads* or *calls* vanilla APIs (an engine consumer)
   is far less likely to collide than one that *replaces* UI or state logic with prefixes.
4. **Cloned objects.** If our mod clones a vanilla prefab, the third-party patch will match the
   clone too, because patches bind by type. Whether that matters is decided by the gate in step 2.
5. **Write the conflict as a concrete scenario** ("if X lands in slot Y while Z is unlocked, then
   …") instead of a general warning. A scenario can be tested; a warning cannot.

## 7. Pitfalls that cost time

- **`$TMPDIR` can resolve to `/tmp`** on this host. Bash builtins and redirection work there, but a
  native tool told to open `/tmp/…` fails with "does not exist" even though `ls` shows the file.
  Always stage for native tools under an explicit Windows path.
- **Non-ASCII filenames** (`✽ Mod.dll`, spaces, emoji) break tool invocations. Copy to an ASCII
  name first; do not try to quote your way through it.
- **`ilspycmd` needs `DOTNET_ROOT`/`PATH`** set to the user-local SDK in agent shells, otherwise it
  reports no SDK.
- **`ilspycmd -l c,i,s,d,e`** produced no type list in the pinned version — decompile and use the
  regex sweep instead.
- **`MelonInfo` is the identity**, not the file name: it carries the real mod name, author and
  declared version.
- **A reference library belongs in `UserLibs\`, not `Mods\`.** A non-mod DLL in `Mods\` gets
  scanned as a mod and produces loader errors. Check the archive for a `UserLibs\` entry and make
  sure it lands there.
- **Embedded resources vs. disk paths.** A mod may look for `Textures\Foo.png` on disk while also
  shipping the same image as an embedded resource with a file fallback. Check both before
  declaring an archive incomplete.
- **Decompiled bodies can be incomplete.** ILSpy emits unresolved-IL annotations when interop
  metadata is missing; a class name is not proof that a feature runs. Keep source-level findings
  and runtime verification strictly apart, and never copy third-party code or constants.

## 8. Reporting the result

Close with three separate statements, never mixed:

1. **What was inspected** — artefact, date, method (static decompile).
2. **What the code says** — with line references.
3. **What remains unverified** — runtime behaviour, multiplayer, save effects, in-game look.
