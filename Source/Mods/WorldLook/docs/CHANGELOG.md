# WorldLook

## 0.1.0

- Merged GoldenHour, WaterShader and GraphicsProbe into one mod and one DLL. The old three must not stay loaded beside it.
- Light bias is a mild warm shift instead of the previous gold wash. The clock still peaks at 06:00 and 18:00 and releases at noon and at night.
- The rendering probe no longer captures on startup or scene changes. Ctrl+Shift+F8 still takes a manual report. The old automatic scans were the multi-second hitch.
- **2026-10-18: owner confirmed the new look in-game** (save slot 2, game 0.4.7f12). Log from that session: `5/7 parameters` biased (volume + fog skipped as designed), clock weight 0.31→0.88 across the evening, water bound on `Main/Map/StylizedWater2_Ocean` with the reference shader, clean teardown on quit. Water look and a manual probe capture remain unverified.
