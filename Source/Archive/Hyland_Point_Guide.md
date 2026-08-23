# 🗺️ Schedule I - Hyland Point 3D Map Guide

Welcome to the official **Schedule I Hyland Point 3D cartography**!
This overview was extracted directly from game data with the **Real-Time S1MCP Game Scanner**.

---

## 📍 Table of Contents
- [📁 Files in this folder](#-files-in-this-folder)
- [🏠 Main Locations & Landmarks](#-main-locations--landmarks)
- [🌿 Special Objects in the Trailer Lab](#-special-objects-in-the-trailer-lab)
- [📦 Containers & Loot Objects](#-containers--loot-objects)
- [📊 Map Statistics](#-map-statistics)

---

## 📁 Files in this Folder
* `hyland_point_map.json`: 1,000 Unity GameObjects from a single `s1_scan_nearby_objects` run (parameter `radius=3000`, `limit=1000`) including 3D coordinates `(X, Y, Z)`, layers, tags and C# components. Real maximum distance 5.23 m — pure player vicinity, **not a full world scan**.
* `Hyland_Point_Guide.md`: This overview documentation.

> ⚠ The six region JSONs (`Docks`, `Northtown`, `Suburbia`, `The Sewers`, `Uptown`, `Westville`) are a **round-robin split** of the master records (`Master[i % 6]`), not geographically separate scans. `Downtown` is missing from the map folder and should be captured separately.

---

## 🏠 Main Locations & Landmarks

> ⚠ **Unconfirmed in the current dataset.** The following coordinates differ significantly from `hyland_point_map.json` (real range X 11.82–19.55, Y −0.30–4.23, Z −88.32–−81.74). Until an actual region scan is done, the values stay marked as TODO.

| Landmark | X | Y | Z | Description |
|---|---|---|---|---|
| 🚐 **Caravan / Hideout** | `16.86` | `2.04` | `-83.90` | Starting trailer & cannabis lab |
| ⛽ **Gas Station** | `50.00` | `2.00` | `-120.00` | Gas station & accessory shop |
| 🏨 **Motel** | `-45.00` | `2.00` | `-50.00` | Motel hideout & rooms |
| 🏬 **Warehouse** | `120.00` | `2.00` | `15.00` | Large warehouse |
| 👮 **Police Station** | `210.00` | `2.00` | `85.00` | Hyland Point police station |

---

## 🌿 Special Objects in the Trailer Lab

| Object Name | Position (X, Y, Z) | Description |
|---|---|---|
| `weedplant_stage7` | `(15.94, 1.53, -82.87)` | Cannabis plant (growth stage 7) |
| `FloorRack(Clone)` | `(15.94, 2.33, -82.87)` | Plant rack & drying shelf |
| `Moisture Display` | `(16.12, 1.66, -83.07)` | Moisture meter |
| `WaterLevelSlider` | `(16.12, 1.66, -83.07)` | Water level slider |
| `digital alarm` | `(17.02, 1.98, -85.22)` | Digital alarm clock |
| `Bedside_Drawer` | `(16.96, 1.74, -85.00)` | Nightstand drawer |

---

## 📦 Containers & Loot Objects
The `hyland_point_map.json` contains **34 container candidates** (heuristic categorization via name match, so it also includes `ViewmodelAvatarContainer`, `CameraContainer`, `BoundingBox`, `StorageFootprintTiles`). Actual searchable/buyable containers can only be determined via in-game analysis (`scheduleone/ObjectScripts/`):
* Nightstands and drawers (`Bedside_Drawer`, `Drawer`)
* Trash bins (`TrashContainer`, `TrashBin`)
* Safes and storage crates (`StorageContainer`)

---

## 📊 Map Statistics
* **Requested scan radius:** `3,000 metres` (parameter)
* **Observed maximum distance:** `5.23 m` (actual scope)
* **Total objects in the index:** `1,000` (player-vicinity capture)
* **JSON file size:** `302,294 B (295.21 KiB)`
* **Real coordinate span:** X `11.82–19.55`, Y `−0.30–4.23`, Z `−88.32–−81.74`
* **Region split:** round-robin shard (`Master[i % 6]`) across 6 files
* **Creation date:** `25 July 2026`
