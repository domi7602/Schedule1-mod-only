# TabToHome

**Status: shelved (2026-10-04) — not deployed.** In-game the phone still closed because S1API's own `Phone.SetIsOpen` calls run after the patch's skip; the mod is pulled and Tab behaves stock again until upstream S1API is fixed. The code stays in the repo.

Tab closes the active phone app to the HomeScreen instead of putting the
phone away. The phone stays up; a second Tab (from the HomeScreen) puts it
away as usual. Works for vanilla and S1API apps alike.

Note: the final put-away still plays the game's lowering animation, so the
brief empty-phone frame on put-away itself is unchanged — this mod only adds
the HomeScreen stopover.
