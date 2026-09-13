# Economy/Money (Schedule I)

## Money Types
| Type | Description |
|------|-------------|
| **Cash** | Physical cash (in inventory, CashSlot max 1,000) |
| **Online** | Digital funds (SyncVar, server-authoritative) |
| **Safe** | Safe balance (stored in properties) |

## Income Sources
1. **Drug Sales** (Main income source)
2. **Weapon Sales** (Police gear)
3. **Gambling** (Casino, slot machine 131.9% RTP)
4. **Money Laundering** (Cash → Online, 24h, no fee)
5. **Recycler** (Trash converted to money, $0–$10 per item)
6. **Quests** (Variable monetary rewards)

## Known Exploits
1. **Pawn Shop Negotiation Bug**: `playerOffer <= lastShopOffer` → immediately accepted
2. **Zero-Fee Laundering**: $1,000 Cash → $1,000 Online, no cut taken
3. **Contract Bonus Stacking**: Up to +65% extra (Curfew + Rain + Quick + Quality)
4. **ATM**: `WEEKLY_DEPOSIT_LIMIT = 10,000$`

## MoneyManager (NetworkSingleton)
- `onlineBalance` (SyncVar, WritePermission ClientUnsynchronized)
- `lifetimeEarnings` (SyncVar)
- `cashBalance` (local via CashInstance in PlayerInventory)
- `AddFunds()`, `RemoveFunds()`, `ChangeCashBalance()`, `ChangeOnlineBalance()`
- Daily net worth check triggered per `onDayPass`

## Net Worth (for Achievements)
| Achievement | Threshold |
|-------------|-----------|
| BUSINESSMAN | $100,000 |
| BIGWIG | $1,000,000 |
| MAGNATE | $10,000,000 |
