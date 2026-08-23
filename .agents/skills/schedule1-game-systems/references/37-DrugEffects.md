# Drug Effects (Schedule I)

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
