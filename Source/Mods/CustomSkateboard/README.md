# Pro Cyber Skateboard - High Performance Skateboard for Schedule I

The **first and only custom skateboard mod** for *Schedule I*!

Introduces the **Pro Cyber Skateboard**, a handcrafted, aerodynamic high-performance skateboard featuring custom physics, extreme carving control, instant Ollies, cyberpunk neon underglow, carbon fiber deck styling, anti-gravel suspension, store integration with Jeff Gilmore, and customizable configuration.

---

## ⚡ Key Features

- 🚀 **Extreme High-Speed Carving:** Reaches up to **100+ km/h** with progressive acceleration and tight carving authority (no vanilla speed cutoff).
- 🕹️ **Ultra-Responsive Steering:** 4x steering responsiveness (`TurnChangeRate: 64.0`, `TurnForce: 15.0`) with increased lateral grip (`1.60x`) for razor-sharp turns like on rails.
- 🛹 **Instant Ollie Jumps:** Instantaneous jump execution upon tapping or holding <kbd>Space</kbd> with authentic pop height and balanced gravity landing.
- 🛸 **Cyberpunk Neon Underglow:** Dynamic cyan neon illumination beneath the deck.
- 🛑 **Anti-Gravel Suspension:** Eliminates all slowdown when riding over grass, gravel, dirt, and rough terrain.
- 🛒 **In-Game Store Integration:** Seamlessly integrated into **Jeff Gilmore's** dialogue shop for in-game purchase ($1,500).
- 💻 **Console Commands:** Spawn or inspect live board stats with `#skate give` or `#skate stats`.
- ⚙️ **Fully Configurable:** Easily tweak speed, turn force, jump height, and prices in `UserData/CustomSkateboard/config.json`.
- 🛡️ **100% Avatar & Vanilla Safe:** Fully isolated runtime physics. Does not overwrite or interfere with vanilla skateboards or character apparel.

---

## 📦 Requirements

- [MelonLoader 0.7.3+](https://melonwiki.xyz/) (Standard IL2CPP)
- [S1API 3.1.15+](https://www.nexusmods.com/schedule1/mods/)

---

## 🔧 Installation

1. Install **MelonLoader 0.7.3+** on your *Schedule I* game directory.
2. Ensure **S1API.Il2Cpp.MelonLoader.dll** is installed in your `<Game>\Mods\` folder.
3. Extract the contents of this archive (`CustomSkateboard.dll`, `CustomSkateboard.json`, `CustomSkateboardIcon.png`) into your `<Game>\Mods\` directory:
   ```text
   Schedule I/
   └── Mods/
       ├── CustomSkateboard.dll
       ├── CustomSkateboard.json
       └── CustomSkateboardIcon.png
   ```
4. Start the game and enjoy!

---

## 🎮 How to Get In-Game

- **Buy from Jeff Gilmore:** Talk to Jeff Gilmore in-game and choose *"Looking for a board?"* to purchase the Pro Cyber Skateboard.
- **Developer Console / Hash:** Press <kbd>F1</kbd> or <kbd>~</kbd> and type:
  ```text
  #skate give
  ```
- **Inspect Live Stats:**
  ```text
  #skate stats
  ```

---

## ⚙️ Configuration (`UserData/CustomSkateboard/config.json`)

```json
{
  "SkateboardId": "custom_skateboard",
  "SkateboardName": "Pro Cyber Skateboard",
  "Price": 1500.0,
  "TopSpeed_Kmh": 100.0,
  "PushForceMultiplier": 5.2,
  "PushForceDuration": 0.35,
  "PushCooldown": 0.22,
  "TurnForce": 15.0,
  "TurnChangeRate": 64.0,
  "TurnReturnToRestRate": 56.0,
  "TurnSpeedBoost": 2.5,
  "LateralFrictionForceMultiplier": 1.60,
  "MaxBoardLean": 28.0,
  "JumpForce": 6.5,
  "JumpDuration_Min": 0.38,
  "JumpDuration_Max": 0.58,
  "JumpForwardBoost": 1.8,
  "DisableTerrainSlowdown": true,
  "AutoInjectToJeffGilmore": true
}
```

---

## 👤 Author & Credits

- **Author:** Dominik
- **Game:** Schedule I (TVGS)
- **Framework:** S1API & MelonLoader
