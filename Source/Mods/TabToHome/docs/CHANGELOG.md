# Changelog

## 0.1.0 (2026-10-04)
- Initial release: Harmony prefix on Phone.SetIsOpen reroutes Tab (app open) to the HomeScreen via the game's own closeApps event; put-away from the HomeScreen is untouched.
- **Shelved same day:** in-game the phone still closed — S1API's own `Phone.SetIsOpen(false)` calls run after the patch's skip, so the redirect cannot win. Game copy removed, not deployed; re-enable once upstream S1API handles `SetIsOpen`.
