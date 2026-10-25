# Economy/Money (Schedule I)

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 4 of 7 identifier-shaped tokens resolve (3 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Inherited, not rerun; [evidence scope](README.md#source-history).

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

## Inherited economy observations to reproduce
1. **Pawn Shop Negotiation Bug**: `playerOffer <= lastShopOffer` → immediately accepted
2. **Zero-Fee Laundering**: $1,000 Cash → $1,000 Online, no cut taken
3. **Contract Bonus Stacking**: Up to +65% extra (Curfew + Rain + Quick + Quality)
4. **ATM**: `WeeklyDepositLimit` (10,000$) — the SCREAMING_SNAKE constant name does not exist in the original source snapshot

## MoneyManager (NetworkSingleton)
- `onlineBalance` (SyncVar, WritePermission ClientUnsynchronized)
- `lifetimeEarnings` (SyncVar)
- `cashBalance` (local via CashInstance in PlayerInventory)
- `ChangeCashBalance(float change, bool visualizeChange = true, bool playCashSound = false)` — plus `ChangeOnlineBalanceCommand(...)` and `ChangeLifetimeEarnings(float change)`. The recorded native-owner search did not find `AddFunds()` / `RemoveFunds()` / `ChangeOnlineBalance()`. That says nothing about similarly named S1API wrappers. Resolve the exact owner, version, return contract and authority before choosing a mutation. See [payment contracts](../../schedule1-economy/SKILL.md).
- Daily net worth check triggered per `onDayPass`

## Net Worth (for Achievements)
| Achievement | Threshold |
|-------------|-----------|
| BUSINESSMAN | $100,000 |
| BIGWIG | $1,000,000 |
| MAGNATE | $10,000,000 |

---

---

## Names reported absent in the original search

`AddFunds`, `ChangeOnlineBalance`, `RemoveFunds`. Absence is scoped to that search, not proof of removal from every supported version.
