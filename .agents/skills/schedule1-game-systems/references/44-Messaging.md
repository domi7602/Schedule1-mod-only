# Messaging (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `MessagingManager` | Global messaging manager (NetworkSingleton) |
| `MSGConversation` | Individual conversation |
| `Message` | Individual message |
| `Response` | Response option |

## Messaging System

- SMS-based communication system
- NPCs send messages based on game events
- Player can choose from predefined responses
- `EConversationCategory`: Categories (Deal, Contract, Social)

## Conversations

- `MSGConversation`: Contains message thread
- `SendableMessage`: Sendable message
- `ResponseCallback`: Callback on response
- `IMessageEntity`: Message entity interface

## UI

- `MessagesApp`: Messages app on the phone
- `MessageBubble`: Message bubble
- `MessageChain`: Message chain
- `MessageSenderInterface`: Sender UI
- `ConfirmationPopup`: Confirmation popup

## Dealer Management

- `DealerManagementApp`: Dealer management
- `DealWindowSelector`: Deal window selector
- `WindowSelectorButton`: Window button

## Persistence

- `MessagesData`: Persistence data
- `MSGConversationData`: Conversation data
- `TextMessageData`: Message data
- `TextResponseData`: Response data
