# Casino/Gambling (Schedule I)

## Slot Machine
- **RTP (Return to Player): ~131.9%** – mathematically guaranteed profit in the long run
- Jackpot conditions inspectable in code
- **Save-scumming possible**: Slot machine state is not persisted, money remains untouched upon reload

## Blackjack
- Standard Blackjack rules
- Betting/interaction handled via UI

## Known Bugs / Exploits
1. **Positive Expected Value**: Slot machine has a positive expected value (131.9%)
2. **No server validation of `betAmount`**: Client can theoretically send arbitrarily high bets
3. **Save-scumming**: Casino state is not saved → reload on loss

## Casino Mod
- Mod increases bet limit to 10,000
- Slot machine maintains positive expected value
- In video: Mega jackpot of over $1,000,000 won
