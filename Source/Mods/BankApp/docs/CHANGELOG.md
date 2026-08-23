# Changelog — BankApp

All notable changes to the BankApp mod will be documented in this file.

## [0.1.0] — 2026-08-17

### Initial Release
- **S1API PhoneApp Integration**: Full smartphone banking app registration under `BankApp`.
- **Digital Account Dashboard**: Large checking card, cash on hand and net worth overview.
- **Slot-Aware ATM Operations**: Precise double-entry cash deposit & withdrawal with slot capacity calculation.
- **Weekly ATM Limits**: Configurable $10,000 weekly deposit limit respecting vanilla game economy.
- **Save-Slot Isolation**: Independent JSON persistence per save slot with `.bak` safety backup.
- **Audio Feedback**: Native cash sounds + procedural UI clicks and deny buzzers.
- **Input Focus Protection**: `BankAppInputFocus` component prevents player movement while editing amount fields.
- **Responsive UI Theme**: Methode 3 dynamic scaling with modern Fintech dark palette.
