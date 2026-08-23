# Customer/Budget (Schedule I)

## Customer Budget Calculation
```
AdjustedWeeklySpend = Lerp($200, $500, Relationship / 5) × RankMultiplier
```
- Relationship 0 → $200 Base
- Relationship 5 → $500 Base
- Rank Multiplier: Street Rat = 1.0×, Kingpin = 3.5×+, max 10×
- **Max per week: $500 × 10 = $5,000**

## Daily Budget
```
OrderDays = 1–5 days (depending on addiction + relationship)
PerDayBudget = AdjustedWeeklySpend / OrderDays
Enjoyment scaling: ×0.66 to ×1.50
→ Effectively up to ~$1,500 per deal
```

## Influencing Factors
| Factor | Effect |
|--------|--------|
| Relationship | Linear $200–$500 |
| Rank | Multiplier 1.0×–10× |
| Addiction | More ordering days |
| Product quality/affinity | Up to +50% per unit |
| Curfew bonus | +20% |
| Rain bonus | +20% |
| Fast delivery bonus | +10% |
| Quality bonus | +15% |

## Important
- **No `dailyBudget` field** – budget is calculated live dynamically for each contract
- **Dealers have no separate budget** (they only broker transactions)
- Reset is not necessary as it is always calculated fresh
