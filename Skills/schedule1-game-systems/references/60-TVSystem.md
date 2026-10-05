# TV System (Schedule I)
> verified: TVApp/TVInterface/TVHomeScreen, minigame classes and UnityEvents re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

Namespace: `Il2CppScheduleOne.TV` (11 types; `TVPauseScreen` lives in `UI`).

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `TVApp` | MonoBehaviour | Base class for all TV apps: `AppName`, `Icon`, `Pauseable`, `Canvas`, `CanvasGroup`, `PauseScreen`, `PreviousScreen`; ctor takes a display index |
| `TVHomeScreen` | TVApp | App launcher: `Apps`, `AppButtonContainer`, `AppButtonPrefab`, `PlayerDisplays`, `TimeLabel` |
| `TVInterface` | MonoBehaviour | The TV session: `Players` list, `HomeScreen`, `Canvas`, `CameraPosition`, `TimeLabel`, `Daylabel` |
| `TVInteractable` | MonoBehaviour | World TV: `IntObj` (InteractableObject) + `Interface` wiring in `Start()`; `Hovered()`/`Interacted()` |
| `TVPauseScreen` | — (UI namespace) | Pause overlay for the TV |

## TVApp Lifecycle
- `Open()` / `Close()` / `Resume()` / `TryPause()` (virtual); `IsOpen` / `IsPaused` getters; `OnClose()` virtual
- `Exit(ExitAction)` closes the whole TV session
- **`ActiveMinPass()`** protected virtual — called per game-minute while the app is open (apps can tick with game time, 17-TimeManager)
- Open/close uses a scale/alpha lerp (`SCALE_MIN`/`SCALE_MAX`/`LERP_TIME`)
- `TVHomeScreen.AppSelected(TVApp)` switches screens; `PlayerChange(Player)` reacts to local player changes; `UpdateTimeLabel()` refreshes the clock label

## TVInterface (multi-seat co-op)
- `CanOpen()`, `Open()` (with `OPEN_TIME`/`FOV` camera transition), `Close()`, `Exit(ExitAction)`
- `AddPlayer(Player)` / `RemovePlayer(Player)` manage `Players` + `PlayerDisplays` on the home screen
- `MinPass()` updates the time/day labels each game minute

## TV Games
| Game | Class | Enums | UnityEvents |
|------|-------|-------|-------------|
| Pong | `Pong` | `EGameMode {SinglePlayer, MultiPlayer}`, `ESide {Left, Right}`, `EState {Ready, Playing, GameOver}` | `onServe`, `onLeftScore`, `onRightScore`, `onGameOver`, `onLocalPlayerWin`, `onReset` |
| Snake | `Snake` | `EGameState {Ready, Playing}`, `TileType` | `onStart`, `onEat`, `onGameOver`, `onWin` |
| Runner | `RunnerGame` | — | `onJump`, `onHit`, `onNewHighScore` |

Details:
- **Pong**: AI via `ReactionTime`, `TargetRandomization`, `SpeedMultiplier`; `GoalsToWin`; ball physics `InitialVelocity`, `VelocityGainPerSecond`, `MaxVelocity`; `GoalHit(ESide)`, `Win(ESide)`, `SetPaddleTargetY(ESide, float)`. `PongBall` (UnityEvent `onHit`), `PongPaddle` (`BOUND_Y`, `MOVE_SPEED`, `SetTargetY`).
- **Snake**: grid `SIZE_X`/`SIZE_Y`, `TimePerTile`; direction queue (`Direction`/`QueuedDirection`/`NextDirection`); `Eat()`, `SpawnFood()`, `Win()` (full board).
- **Runner**: `GameSpeed` ramp (`MinGameSpeed`/`MaxGameSpeed`/`SpeedIncreaseRate`), `Gravity`/`JumpForce`/`DropForce`; `RunnerGameCharacter` triggers `onHit` via `OnTriggerEnter`.

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `onPlayerAdded` / `onPlayerRemoved` (TVInterface) | `UnityEvent<Player>` | `AddPlayer()` / `RemovePlayer()` |
| Pong/Snake/Runner events above | `UnityEvent` | game state transitions (`GoalHit`, `Eat`, `EndGame`, ...) |

## Save Participation
- **None**: no ISaveable in the TV namespace. Snake/Runner/Pong scores are session-only; RunnerGame highscore persistence mechanism unverified (no PlayerPrefs reference found).

## Hook Points
1. **S1API wrapper (verified in 3.2.1-beta.8)**: subclass `S1API.TVApp.TVApp` (extends `Registerable`) — S1API builds the canvas and registers the app into the native `TVHomeScreen`; this is the intended custom-app path.
2. **Prefix `TVInterface.CanOpen()` / `Postfix Open()`** — gate TV usage (shop hours, perks).
3. **Postfix `TVApp.ActiveMinPass()`** — per-minute logic while a TV session is open (time-based events).
- Subscribe `TVInterface.onPlayerAdded` to track co-op sessions.

## Not Implemented / Unverified
- No custom-app registration API in the game itself — only via S1API.
- No TV save data (highscores/settings) found in this namespace.

## Cross-links
06-UI-Phone-HUD · 40-Interaction · 17-TimeManager (min ticks) · 01-FishNet-Networking (Player seats)
