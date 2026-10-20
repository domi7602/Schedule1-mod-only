> verified 2026-10-20 — every flag below was observed in one of three analysed third-party mods; file references are to the local decompiles under `Documents/Schedule1-Mod-Reversing/`.

# Red Flags — what to look for and what it tells you

Ordered by "how much does this change the verdict". Each entry: signal → what it means →
what to write in the analysis.

## 1. Swallowed exceptions

```csharp
catch (Exception) { }
```

Whole-tick `try { … } catch (Exception) { }` around `OnUpdate` (FaleHouseKeys `Main.cs:71`) means
every failure of the mod — patch drift, null refs, IL2CPP cast errors — is invisible. Combined
with `[Conditional]` logging (see 2) the mod can be effectively dead while `Latest.log` stays
clean.

**Verdict line:** "failures are silent by construction; after a game update this mod can lose
functionality with no log evidence."

Distinguish `SafeInvoker`-style catches (log every occurrence — noisy but visible) from
`Diagnostics.ErrorOnce`-style (first hit logged, repeats counted — quiet but visible).

## 2. Compiled-away logging

```csharp
[Conditional("MOD_DIAGNOSTICS")]
internal static void Msg(string message) { }
```

Without the symbol, the calls are **removed from the shipped DLL**, not merely disabled. A
`ReleaseLog` wrapper class like this means the release build has zero diagnostics by design.

**Verdict line:** "no diagnostic surface in the release build; troubleshooting requires a custom
build with the symbol defined."

## 3. Patches on compiler-generated types

```csharp
Patch(typeof(ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique), "MoveNext", …)
Patch(typeof(_PlayMethAnimation_d__27), "MoveNext", …)
```

These names encode nesting and a sequential index; the compiler regenerates them on any change to
the enclosing method. They work today and are the first thing to break. Usually used to hook
coroutines (per-customer animations, handover sequences).

**Verdict line:** "N of M hooks are compiler-generated and will break at the next build; the
feature they gate (…) degrades silently unless the mod has a hook counter."

## 4. Private field / method-name reflection

```csharp
AccessTools.Field(typeof(GoonPool), "goons")?.GetValue(instance)
```

Silent failure mode: field renamed → the guard simply stops guarding, no exception. Common for
pool sizes, caches and "is this our object" checks.

**Verdict line:** "guard depends on a private field name; on rename the mod keeps running without
the guard."

## 5. Hash-suffixed RPC targets

`RpcLogic___TakeDamage_3505310624`, `RpcLogic___Arrest_Client_2166136261`,
`RpcLogic___ResetBehavioursEnableState_2166136261`, `RpcLogic___BeginPackaging_2166136261`.

The number is a generated hash of the signature. Two very different behaviours exist:

- **Hard check** — `if (method == null) throw new MissingMethodException(…)` (Roguelike `Mod.cs:52`):
  the whole mod refuses to initialise after an update. Honest, but total.
- **Soft lookup** — `AccessTools.Method(...)` returning null and patching nothing: feature off,
  no message.

**Verdict line:** name the failure mode (hard/soft) explicitly; always run the compatibility check.

## 6. Direct `RpcLogic___` invocation

Calling `enemy.Behaviour.RpcLogic___ResetBehavioursEnableState_2166136261()` from a mod is a
*local* call, not a network send. Fine in singleplayer, wrong in co-op. Look for the corresponding
gate: `Player.PlayerList.Count <= 1`, `InstanceFinder.IsServer`, or `NetworkGuard`.

**Verdict line:** "singleplayer-only by construction because client RPCs are invoked directly
instead of via FishNet."

## 7. Per-frame postfixes

`ListingUI.Update`, `ProductItemInstance.get_Name`, `MultiTypeVisualsSetter.ApplyVisuals`,
`ManagementWorldspaceCanvas.UpdateInputPrompt`, plus LINQ inside damage callbacks.

Any patch on something the UI calls per item per frame is a permanent tax. Not automatically
wrong — it is how a UI stays in sync — but it must be measured, and it interacts badly with other
mods patching the same method (double-scaling when two mods both postfix `get_Price`).

**Verdict line:** "hot path; measure frame time with it installed, do not copy the pattern
without a dirty-flag."

## 8. State outside the save, no `.bak`

Custom items are invisible to the game save, so mods re-create them from a sidecar file:

```
tmp write → File.Replace(tmp, path, path + ".bak")   ← safe
tmp write → File.Move(tmp, path, overwrite: true)    ← no backup, crash = corrupt
```

Plus a **snapshot cadence**: restore-after-load from a 10 s inventory snapshot can resurrect a
dropped key or lose a new one. And a slot-detection fallback (`unknown_slot.json`) merges the state
of *all* save slots.

**Verdict line:** "inventory state is reconstructed from a 10 s snapshot; a fast world switch can
roll items back, and a slot-detection failure merges saves."

## 9. Guessed save-slot reflection

```csharp
foreach (var name in new[] { "SaveSlotNumber", "SaveSlotIndex", "saveSlotNumber", "saveSlotIndex" })
```

Reflection over guessed member names. When nothing matches, the mod writes a shared file. Always
check which one actually exists in the current runtime before believing per-slot isolation.

## 10. Network access

Grep for `HttpClient`, `UnityWebRequest`, `WebRequest`, `DownloadString`, `http://`, `https://`.

Judge by **destination**:
- user-supplied URL (logo/image import) → fine, still note the UA string and the timeout;
- hardcoded host → read what is sent, and report it as a finding either way;
- update check / telemetry → note it, even if it "only" fetches a version string.

## 11. Install footprint

Check the live folder before writing the report:

```bash
ls "/c/Program Files (x86)/Steam/steamapps/common/Schedule I/Mods/" | grep -i <name>
ls "/c/Program Files (x86)/Steam/steamapps/common/Schedule I/UserData/" | grep -i <name>
```

"No, it is not installed" is itself a finding — it changes whether the analysis is preventive or
urgent. Also note leftovers a mod would leave behind (UserData folder, MelonPreferences category)
when there is no uninstall path.

## 12. Contrast signals (what a careful mod looks like)

Worth naming in the report because they are the adoptable part:

- per-call-site error de-duplication (`Log.ErrorOnce(where, ex)`),
- hook counters plus a startup block naming the failed targets,
- a config version with a migration chain instead of overwriting user settings,
- assets generated at runtime (meshes, icons via `RenderTexture`, synthesised `AudioClip`s)
  instead of shipped files,
- lazy registration of custom items through the registry lookup prefix, so old saves need no
  migration,
- restoring mutated collider/behaviour state on teardown (store the original `enabled` value).
