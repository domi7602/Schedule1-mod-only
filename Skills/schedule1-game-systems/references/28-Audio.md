# Audio (Schedule I)
> verified: classes + methods/fields + singleton types re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

## Core Classes

| Class | Verified base type | Purpose |
|-------|--------------------|---------|
| `AudioManager` | `PersistentSingleton<AudioManager>` (NOT networked) | Volume, mixers, distortion, source pooling |
| `MusicManager` | `PersistentSingleton<MusicManager>` | Track enable/stop, distortion, current track |
| `SFXManager` | `Singleton<SFXManager>` | Footsteps, impact sounds, AudioSource pool |
| `MusicTrack` | `MonoBehaviour` | One track: Enable/Disable/Play/Stop, fade |
| `PursuitMusicTrack` | `MusicTrack` | Auto reacts to `EPursuitLevel` changes |
| `PursuitLoopMusicTrack` | `MusicTrack` | Looping pursuit layer |
| `MusicPlayerTool` | `MonoBehaviour` | Drives tracks for a device: `PlayTrack(string trackName)`, `StopTracks()` |
| `ObjectScripts.Jukebox` | `GridItem` (+ nested `Track`, `JukeboxState`) | The in-world jukebox object (this is what `S1API.Audio` wraps) |
| `AudioSourceController` | `MonoBehaviour` | Volume/pitch/loop/spatial wrapper; `Play()`, `PlayOneShot()`, `DuplicateAndPlayOneShot()`, `SetClip()`, `Stop()` |
| `LegacyAudioSourceController` / `RandomizedAudioSourceController` | `AudioSourceController` | Variants (legacy FixedUpdate logic / randomized clips) |
| `SFXConfiguration` | `Configuration<SFXSettings>` | `TryGetImpactTypeData(EImpactSound)`, `TryGetFootstepSoundGroup(EMaterialType)` |
| `AudioZone` | `PolygonalZone` | Polygon zone with tracks + modifier list |
| `AudioZoneModifierVolume` | `MonoBehaviour` | `IsCameraWithinVolume()` → applies modifier via `AddModifier/RemoveModifier` on zone |
| `AudioZoneTrack` | plain object | `Init()`, `Update(float multiplier)`, `UpdateTimeMultiplier(int time)` |
| `PolygonalZone` | `MonoBehaviour` | Point-in-polygon tests (`IsPointInsideZone`, `GetDistanceToClosestPointOnZone`) |
| `AmbientTrackGroup` / `AmbientOneShot` | `MonoBehaviour` | Ambient playlists; one-shot has `EPlayTime` and hooks `onUncappedMinutePass` |
| `RBImpactSounds` | `MonoBehaviour` | Rigidbody collision → `OnImpacted(Impact)` |
| `ButtonSound`, `HeartbeatSoundController`, `SpottedTremolo`, `SewerAmbientSound`, `TimeOfDayVolumeController`, `AudioClipListPlayer`, `StartLoopStopAudio`, `StartLoopMusicTrack` | `MonoBehaviour` | UI clicks, low-health heartbeat, spotted-by-police tremolo, sewer ambience, TOD mixing, clip list, loop start/stop helpers |

## Volume Model (AudioManager)
- Per-category fields: `_masterVolume`, `_musicVolume`, `_fxVolume`, `_ambientVolume`, `_uiVolume`, `_voiceVolume`, `_footstepsVolume`, `_weatherVolume`
- `SetVolume(EAudioType, float)`, `GetVolume(EAudioType, bool scaled = true)`, `SetMasterVolume(float)`
- Statics: `MinGameVolume`, `MaxGameVolume`, `GameVolumeLerpSpeed`; `Update()` lerps `_currentMainMixerVolume`
- `SetDistorted(bool, float transition = 5f)` swaps mixer snapshot `_defaultSnapshot` ↔ `_distortedSnapshot` (used e.g. while intoxicated)
- `CreateAudioSourceController(EAudioType, AudioClip, float defaultVolume = 1, bool spatialize = false)` — correct way to spawn mod audio into the right mixer group
- `EAudioType` lives in `ScheduleOne.Core.Audio` (Core module, **not** in this Assembly-CSharp dump; values unverified)

## Music System (MusicManager)
- Fields: `_tracks` (`List<MusicTrack>`), `_currentTrack`, static `TrackUpdateInterval`
- API: `SetTrackEnabled(string trackName, bool)`, `TryGetTrack(string, out MusicTrack)`, `StopTrack(string)`, `StopAndDisableTracks()`, `SetMusicDistorted(bool, float transition = 5f)`, `UpdateTracks()` (private, invoked by repeating callback)
- `MusicTrack` fields: `_trackName`, `_priority`, `_volumeMultiplier`, `_fadeInTime`, `_fadeOutTime`, `_autoFadeOut`; property `IsPlaying`
- `PursuitMusicTrack` registers itself to pursuit-level events: `PursuitLevelChange(EPursuitLevel old, EPursuitLevel new)` (delegate hookup via `RegisterEvent()`), dips music after `OutOfSightTimeToDipMusic`

## Events
| Event | Type / where raised |
|-------|--------------------|
| `AudioManager.onVolumeSettingsChanged` | `PreallocatedAction` field on AudioManager instance (not a C# `event`) — raised after volume settings change |
| `PursuitLevelChange` | method, not event; `PursuitMusicTrack` subscribes internally to `PlayerCrimeData` pursuit level |
| No global "song changed" event exists in these classes — poll `MusicManager._currentTrack` |

## Save Participation
- None of the Audio classes implement `ISaveable` / `SaveData` (grep verified). Volume settings persist via the `Configuration` system (`AudioSettings` in DevUtilities), not via this namespace.

## Hook Points (Harmony)
1. `AudioManager.SetVolume(EAudioType, float)` — regular instance method (CallerCount 7); prefix to intercept/log/redirect all volume changes. Not a field accessor, method body does mixer work (not inlined).
2. `MusicManager.SetTrackEnabled(string, bool)` — non-trivial instance method (CallerCount 7); gate or replace playlist behavior.
3. `SFXManager.PlayFootstepSound(EMaterialType, float, Vector3, float)` / `PlayImpactSound(EImpactSound, Vector3, float)` — public, stateful (pool lookup), safe to postfix for custom footstep/impact sounds.
- S1API: no direct `AudioManager`/`MusicManager` wrapper. `S1API.Audio` ships `Jukebox` / `JukeboxManager` / `JukeboxSnapshots`, which wrap the in-world `Il2CppScheduleOne.ObjectScripts.Jukebox` (GridItem) — jukebox music playback, not the global managers.

## Not Implemented / Notes
- `MusicManager.UpdateTracks()` and `SFXManager.SetupSoundPool()` are private with CallerCount 0 in the dump — called indirectly (callback/coroutine), do not patch expecting direct call sites.
- `IAudioZoneModifier` is an interface wrapper (interop stub), fine to implement against via `Il2CppObjectBase` pattern.
