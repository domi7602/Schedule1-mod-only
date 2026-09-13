# Audio (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `AudioManager` | Global audio manager (NetworkSingleton) |
| `MusicManager` | Music playlist controller |
| `SFXManager` | Sound effects, footsteps, impact sounds |
| `MusicPlayerTool` | In-game record player item |

## Audio Zones

- `AudioZone`: Spatial audio sphere/volume with dedicated track
- `AudioZoneModifierVolume`: Override parameters within an audio zone
- `TimeOfDayVolumeController`: Volume adjustment based on time of day

## Pursuit Music

- `PursuitMusicTrack` / `PursuitLoopMusicTrack`: Overrides normal background music during police pursuits
- `SpottedTremolo`: Tremolo audio effect when spotted by police
- `HeartbeatSoundController`: Heartbeat audio effect at low player health

## Ambient System

- `AmbientTrackGroup`: Random selection from ambient music tracks
- `AmbientOneShot`: Random single-occurrence ambient sound effects
- `SewerAmbientSound`: Specialized ambient sound system for the sewer environment

## Audio Controllers

- `AudioSourceController`: Wrapper around Unity AudioSource (fading, crossfading)
- `AudioClipListPlayer`: Plays random clips from a configured list
- `RBImpactSounds`: Physics rigidbody impact sounds
- `ButtonSound`: UI button click audio feedback

## SFX Configuration

- `SFXConfiguration`: Predefined audio events
- `EAudioType`: Categories (Music, SFX, UI, Voice, Ambient)
