# Messaging (Schedule I)
> verified: classes + RPC surface + event types + save-participation re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

Namespace: `Il2CppScheduleOne.Messaging` (UI in `Il2CppScheduleOne.UI.Phone.Messages`).

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `MessagingManager` | `NetworkSingleton<MessagingManager>` | Conversation registry + full message RPC surface |
| `MSGConversation` | `Il2CppSystem.Object` | One thread: history, responses, UI wiring (not a MonoBehaviour) |
| `Message` | `Il2CppSystem.Object` | Single history entry (`messageId`, `text`, `sender`, `endOfGroup`) |
| `SendableMessage` | `Il2CppSystem.Object` | Player-side sendable draft with validity checks |
| `Response` | `Il2CppSystem.Object` | Predefined player response option (`text`, `label`, `callback`) |
| `MessageContactInfo` | `Il2CppSystem.Object` | Contact identity (`_name`, `_npcId`, `_icon`, `_canConversationBeHidden`, `_displayRelationshipInfo`) |
| `EConversationCategory` | enum | **`Customer, Supplier, Dealer`** — old "Deal/Contract/Social" list was wrong |

## MessagingManager (verified)
- `_senderIdConversationMap` (`Dictionary<string, MSGConversation>`), `Register(MSGConversation)`, `TryGetConversation(string senderId, out MSGConversation)`
- Server/client method pairs (each backed by FishNet RPCs): `SendMessage_Server/Client(Message, bool notify, string id)` (hash 2134336246), `SendMessageChain_Server/Client(MessageChain, string id, float initialDelay, bool notify)` (3949292778), `SendResponse_Server/Client(int responseIndex, string id)` (2801973956), `SendPlayerMessage_Server/Client(int sendableIndex, int sentIndex, string id)` (1952281135), `ClearResponses_Server/Client(string id)` (3615296227), `ShowResponses_Server/Client(string id, List<Response>, float delay)` (995803534)
- `ReceiveMSGConversationData(NetworkConnection, string id, MSGConversationData)` — target RPC (2662241369) pushing full conversation history to joining clients; `OnSpawnServer` triggers it
- `MessageChain` type lives in `Il2CppScheduleOne.UI.Phone.Messages`

## MSGConversation (verified)
- State props: `ConversationId`, `IsSenderKnown`, `Read`, `Index`, `IsOpen`, `EntryVisible`, `Categories`, `MessageHistory`; `_sender` (MessageContactInfo), `_messageHistory`, `_messageChainHistory`, `_bubbles`, `_sendables`, `currentResponses`
- Core flow: `SendMessage(Message, bool notify = true, bool network = true)`, `SendMessageChain(MessageChain, float initialDelay = 0, ...)` (coroutine roll-out), `ShowResponses(List<Response>, float delay, bool network)`, `GetResponse(string label)`, `ResponseChosen(Response, bool network)`, `ClearResponses(bool network = false)`, `CreateSendableMessage(string) → SendableMessage`, `SendPlayerMessage(...)`, `CanSendNewMessage()`, `CheckSendLoop()`
- Contact/UI: `SetIsKnown(bool)`, `SetCategories(List<EConversationCategory>)`, `MoveToTop()`, `EntryClicked()`, `SetOpen(bool)`, `SetRead(bool)`, `SetEntryVisibility(bool)`, `DisplayRelationshipInfo()`, `RenderMessage(Message)`, `RenderPlayerMessage(SendableMessage)`, `GetSenderIcon()`, `CreateUI()`/`EnsureUIExists()`/`RefreshPreviewText()`
- Replication helpers: `ShouldReplicate()`, `GetReplicationByteSize()`

## SendableMessage / Response (verified)
- `SendableMessage`: `Text`, `ShouldShowCheck` / `IsValidCheck` (**delegate fields**, Func-style — not UnityEvents), `onSelected`, `onSent`, `conversation`, `disableDefaultSendBehaviour`, `sentIDs`; `ShouldShow()`, `IsValid(out string reason)`, `Send(bool network, int id = -1)`
- `Response`: `text`, `label`, `callback` (delegate field), `disableDefaultResponseBehaviour`
- `Message.GetSaveData() → TextMessageData`; `MessageContactInfo.IsNull()`, `TryGetNPC(out NPC)`

## Events / UnityEvents (delegate fields — NOT UnityEvents)
| Event | Type | Raised by |
|-------|------|-----------|
| `MSGConversation.onMessageRendered` | `Il2CppSystem.Action` | message bubble render |
| `MSGConversation.onLoaded` | `Il2CppSystem.Action` | conversation load |
| `MSGConversation.onResponsesShown` | `Il2CppSystem.Action` | `ShowResponses(...)` |
| `MSGConversation.onConversationOpened` | `Il2CppSystem.Action` | `SetOpen(true)` path |

No UnityEvents in the Messaging namespace (grep).

## Save Participation
- `MSGConversation.GetSaveData() → MSGConversationData` + `Load(MSGConversationData)` — per-conversation persistence; `Message` persists via `TextMessageData` (response data class: `TextResponseData`).
- Data classes live in `Il2CppScheduleOne.Persistence.Datas` (see 02-Save-Persistence); `MessagingManager` itself is **not** ISaveable (grep) — conversations replicate/save individually.

## Hook Points
1. **Postfix `MessagingManager.SendMessage_Server(Message, bool, string)`** — monitor every NPC→player message server-side (loggers, triggers, deal detection); RPC logic method, non-inline.
2. **Prefix/postfix `MSGConversation.ShowResponses(List<Response>, float, bool)`** — inject or rewrite the player's response options (custom dialogue trees); instance method on a plain object, patchable.
3. **Prefix `MSGConversation.CanSendNewMessage()`** — throttle/block player sends (phone-signal events); simple bool, patchable.
- **S1API (3.2.1-beta.8) wrappers (verified in source):** `S1API.Messaging.Response` (`Label`, `Text`, `OnTriggered` → wraps native `Response.callback`); `S1API.Entities.NPC` messaging helpers call `MSGConversation.SendMessage`/`ShowResponses`/`ClearResponses`/`SetCategories`/`EnsureUIExists` directly (send + response flow via `NPC.SendTextMessage`, verified in `NPC.cs`).

## Not Implemented / Unverified
- No global "message received" event on `MessagingManager` — hook the RPCs or per-conversation actions instead.
- NPC→player message scheduling/queues (when NPCs decide to text) are not in this namespace (`unverified`, likely AI/behaviour side).
- `MSGConversation` UI internals (`entry`, `bubbleContainer`, `slider`, `dialogueScreenUIPanel`) verified as fields; the phone UI classes (`MessagesApp`, `MessageBubble`, dealer management + `DealWindowSelector`) live in `UI.Phone.Messages` — covered by phone-UI docs, not duplicated here.

## Cross-links
02-Save-Persistence · 11-Business-Laundering · 09-Inventory-ItemFramework · 01-FishNet-Networking
