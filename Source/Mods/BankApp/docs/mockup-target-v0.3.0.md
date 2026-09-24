# BankApp — Target Design Mockup (v0.3.0 Reference)

**Source file:** `mockup-target-v0.3.0.png`
**Analysis:** MiniMax VLM via `mmx vision describe`, 2026-09-09
**Status:** Target design, not yet implemented

## Described Layout (top → bottom)

1. **Weekly Progress Header**
   - Left: "Weekly Progress" (bold, white)
   - Right: "$3870/10000"
   - Below: thin teal progress bar, about 1/3 filled

2. **Balance section (two columns)**
   - Left: "Cash Balance" → `$11,560` (teal/cyan)
   - Right: "Online Balance" → `$3,552` (teal/cyan)

3. **Amount row**
   - Left: "Amount" label
   - Right: `$0` (current amount; set via chips)

4. **Mode tabs**
   - "⟳ Deposit" (left, selected = blue)
   - "⟳ Withdraw" (right, dark, unselected)

5. **Chip grid (2 columns × 5 rows)**
   | Left column | Right column |
   |---|---|
   | $1 | $5 |
   | $10 | $25 |
   | $50 | $100 |
   | $500 | $1000 |
   | ✕ Clear | MAX |

6. **Primary button**
   - Large green "Deposit" button, full width, at the bottom
   - (Label likely tab-dependent: Deposit/Withdraw)

## VLM Side Observations (Design Notes)

- Chips fully readable, generous padding — no squeezing like in v0.2.0 (5 chips in 1 row)
- Clear/MAX look like amount chips but are actions → possible confusion risk
- Amount looks like a read-only display instead of a text field — manual input unclear