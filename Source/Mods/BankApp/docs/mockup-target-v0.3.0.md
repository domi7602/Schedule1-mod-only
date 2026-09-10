# BankApp — Ziel-Design Mockup (v0.3.0 Referenz)

**Quelldatei:** `mockup-target-v0.3.0.png`
**Analyse:** MiniMax VLM via `mmx vision describe`, 2026-09-09
**Status:** Ziel-Design, noch nicht umgesetzt

## Beschriebenes Layout (top → bottom)

1. **Weekly Progress Header**
   - Links: „Weekly Progress" (bold, weiß)
   - Rechts: „$3870/10000"
   - Darunter: dünner Teal-Fortschrittsbalken, ca. 1/3 gefüllt

2. **Balance-Sektion (zwei Spalten)**
   - Links: „Cash Balance" → `$11,560` (teal/cyan)
   - Rechts: „Online Balance" → `$3,552` (teal/cyan)

3. **Amount-Zeile**
   - Links: „Amount"-Label
   - Rechts: `$0` (aktueller Betrag; wird über Chips gesetzt)

4. **Modus-Tabs**
   - „⟳ Deposit" (links, selektiert = blau)
   - „⟳ Withdraw" (rechts, dunkel, unselektiert)

5. **Chip-Grid (2 Spalten × 5 Reihen)**
   | Linke Spalte | Rechte Spalte |
   |---|---|
   | $1 | $5 |
   | $10 | $25 |
   | $50 | $100 |
   | $500 | $1000 |
   | ✕ Clear | MAX |

6. **Primär-Button**
   - Großer grüner „Deposit"-Button, volle Breite, unten
   - (Label vermutlich tab-abhängig: Deposit/Withdraw)

## VLM-Nebenbeobachtungen (Design-Hinweise)

- Chips vollständig lesbar, großzügiges Padding — kein Quetschen wie in v0.2.0 (5 Chips in 1 Reihe)
- Clear/MAX sehen wie Betrags-Chips aus, sind aber Aktionen → mögliches Verwechslungsrisiko
- Amount wirkt wie Read-Only-Anzeige statt Textfeld — manuelle Eingabe unklar
