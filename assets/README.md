# Assets

Images used by the root [`README.md`](../README.md) and other repository-level documentation. Runtime assets that ship with a mod (icons, bundles, GLB models) live in the mod's own `Source/Mods/<Mod>/assets/` folder and are deployed by the build — do not put them here.

## Layout

```text
assets/
├── README.md
├── bankapp/
│   └── dashboard-v0.3.0.png       In-game screenshot of the BankApp dashboard (verified build v0.3.0)
├── icon-sources/                  1024×1024 source renders for the phone-app icons
│   ├── bank.jpg  calculator.jpg  notes.jpg  pots.jpg  weather.jpg   (BankApp, CalculatorApp, NotesApp, PotScanner, Weather)
│   └── shop_b.jpg                 PocketShop icon; shop.jpg / shop_a.jpg / shop_v2.jpg are unused variants
└── <modname>/                     one folder per mod for screenshots (add as needed)
```

The deployed 256×256 PNG icons in each mod's `assets/` folder (e.g. `Source/Mods/BankApp/assets/bank_icon.png`) are downscaled from these sources.

## Adding screenshots

1. Take the screenshot in-game with the mod UI clearly visible. No personal or save-sensitive information.
2. Save as PNG, at most 1920 px wide, ideally under ~1 MB (compress with e.g. `pngquant`).
3. Put it in `assets/<modname>/` using the lowercase mod name and a short descriptive file name, optionally with the mod version the UI belongs to:

   ```text
   assets/notesapp/list-and-editor.png
   assets/calculatorapp/history.png
   assets/potscanner/filter-tabs.png
   assets/pocketshop/item-detail.png
   assets/weather/dashboard.png
   assets/autopackagingstation/conveyor-line.png
   assets/hitmanphone/contract-offer.png
   assets/moresaveslots/pagination.png
   assets/customskateboard/riding.png
   ```

4. Reference it from the root README's *Screenshots* section:

   ```html
   <img src="assets/notesapp/list-and-editor.png" width="260" alt="NotesApp: note list and editor">
   ```

   Several screenshots side by side work best as a Markdown table with one image per cell and a one-line caption row beneath.

5. Do not add mock-ups, renders or placeholder images that could be mistaken for in-game screenshots. Design mock-ups belong next to the mod (`Source/Mods/<Mod>/docs/`, see `BankApp/docs/mockup-target-v0.3.0.png`).

Wanted first: NotesApp, CalculatorApp, PotScanner, PocketShop, Weather (phone apps); AutoPackagingStation, CustomSkateboard, HitmanPhone, MoreSaveSlots (gameplay / QoL).
