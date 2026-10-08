# Polling (Schedule I)

> verified: full re-check 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9) — all prior speculation resolved/removed. Decompiles are IL2CPP interop stubs — hierarchy/signatures verified, method bodies not readable. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

## What it actually is (resolved speculation)

An **online community-poll HTTP client** — not a game-world mechanic. The old draft claimed "NPCs express opinions", "results affect game world?" and "NetworkSingleton sync": **all removed** — none of it exists in the decompiles.

## Core Classes (verified)

| Class | Hierarchy | Purpose |
|-------|-----------|---------|
| `PollManager` | `MonoBehaviour` *(not NetworkSingleton)* | Fetches/submits polls over HTTP |
| `PollData` | plain object | `pollId`, `question`, `answers[]`, `answerDescriptions[]`, `winnerIndex`, `confirmationMessage` |
| `PollAnswer` | plain object | `pollId`, `answer`, `ticket` — submission with auth ticket |
| `PollResponse` | plain object | `polls[]`, `active`, `confirmed`; `GetActive()`, `GetConfirmed()` |
| `PollResponseWrapper` | plain object | HTTP JSON envelope: `success`, `data` |
| `PollPanel` | `MonoBehaviour` (`ScheduleOne.UI.Polling`) | Poll UI |

## PollManager (verified members)

- `static string ServerUrl` (settable field), nested enum `EPollSubmissionResult { InProgress, Success, Failed }`.
- Properties: `ActivePoll`, `ConfirmedPoll` (public get / private set), `SubmissionResult`, `SubmisssionFailedMesssage` *(sic, triple-s typo in the game)*.
- Methods: `SelectPollResponse(int responseIndex)`, `static TryGetExistingPollResponse(int pollId, out int response)`, coroutines `SubmitAnswerToServer(PollAnswer)` + `RequestPoll(string url, Action<string> callback)` (`UnityEngine.Networking.UnityWebRequest`), private `ResponseCallback`, private `PlatformInitialized()`, statics `CleanTicket(string ticket)` (auth-ticket processing — Steam-ticket assumption **unverified**), `RecordSubmission(int pollId, int response)` (marks answered → dedup via `TryGetExistingPollResponse`).
- Events: `onActivePollReceived` / `onConfirmedPollReceived` (`Action<PollData>`, add_/remove_ verified) — the **only** events in this system.
- `ActivePoll` = currently running poll, `ConfirmedPoll` = poll whose winner was announced (`winnerIndex`, `confirmationMessage`).
- **No time/limit fields** on `PollData` — "time-limited polls" claim removed. **No NotificationsManager or phone-app integration** — only `PollPanel` references polling.

## UI (PollPanel, verified fields)

`QuestionLabel`, `InstructionLabel`, `ConfirmationMessageLabel`, `ButtonPrefab`/`buttons`/`buttonFills`, hold-to-submit via `BUTTON_PRESS_TIME` + `heldButton`/`buttonPressTime`, result colors `TextColor_Green`/`TextColor_Red`, `ActivePill`/`ClosedPill` status pills, sounds `SubmissionStartSound`/`SubmissionSuccessSound`/`SubmissionFailSound`.

## Save participation

None — no `ISaveable`. Submission state persists via `RecordSubmission`/`TryGetExistingPollResponse` (storage mechanism **unverified**); `PollResponseWrapper` is an HTTP response DTO, not a save file.

## Hook Points

1. **Subscribe `PollManager.onActivePollReceived` / `onConfirmedPollReceived`** — react to new polls and announced results (no patch needed).
2. **Prefix `PollManager.SelectPollResponse`** — auto-vote / force a specific response index.
3. **Interop set `PollManager.ServerUrl`** — it is a static field, not patchable via Harmony; assign directly (e.g., host your own poll backend).
4. **S1API:** no wrapper exists for polling — use native interop types directly.
