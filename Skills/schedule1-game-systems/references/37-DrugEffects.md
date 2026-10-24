# Drug Effects (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 45/46 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Core Classes

| Class | Purpose |
|-------|---------|
| `EffectController` | Effect controller (NetworkSingleton) |
| `EffectHandler` | Effect handler per player |
| `Effect` | Base effect class |
| `EffectSettings` | Effect settings |
| `EffectMixCalculator` | MixMap calculation |

## All Effects

| Effect | Type | Impact |
|--------|------|--------|
| `AntiGravity` | Physics | Reduced gravity |
| `Athletic` | Movement | Faster sprinting |
| `Balding` | Visual | Hair loss |
| `BrightEyed` | Visual | Glowing eyes |
| `Calming` | Psychological | Relaxation |
| `CalorieDense` | Physics | No hunger |
| `Cyclopean` | Visual | Single eye |
| `Disorienting` | Visual | Distorted vision |
| `Electrifying` | Visual | Electric aura |
| `Energizing` | Energy | No exhaustion |
| `Euphoric` | Psychological | Feeling of euphoria |
| `Explosive` | Physics | Explode upon taking damage |
| `Focused` | Visual | Sharpened vision |
| `Foggy` | Visual | Foggy vision |
| `Gingeritis` | Visual | Red hair |
| `Glowie` | Visual | Glowing body |
| `Jennerising` | Visual | Caitlyn Jenner effect |
| `Laxative` | Physics | Increased bowel movement |
| `Lethal` | Death | Instant death |
| `LongFaced` | Visual | Elongated face |
| `Munchies` | Physics | Sudden hunger / Munchies |
| `Paranoia` | Psychological | Paranoia |
| `Refreshing` | Energy | Renewed energy |
| `Schizophrenic` | Psychological | Hallucinations |
| `Sedating` | Movement | Slowdown |
| `Seizure` | Visual | Seizure effect |
| `Shrinking` | Visual | Shrinking |
| `Slippery` | Physics | Slippery |
| `Smelly` | NPC | NPCs avoid player |
| `Sneaky` | Stealth | Silent movement |
| `Spicy` | Visual | Fire effect |
| `ThoughtProvoking` | Psychological | Deep thoughts |
| `Toxic` | Health | Poison damage |
| `TropicThunder` | Visual | Tropical outfit |
| `Zombifying` | Visual | Zombie appearance |

## MixMaps

- `MixerMap`: Defines effect combinations
- `MixerMapEffect`: Individual effect entry
- `MixerMapGenerator`: Generates MixMaps
- `EffectMixCalculator`: Calculates mix results

## Effect Parameters

- `GradientParameter`: Color gradient
- `NumericParameter`: Numeric value
- Each effect has its own `EffectSettings` wrapper
- `EffectState`: Current effect state (Active, Inactive, Fading)

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `EffectState`
