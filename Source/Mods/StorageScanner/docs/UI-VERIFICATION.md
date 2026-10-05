# StorageScanner UI verification

## Automated checks

Run from the repository root without deploying:

```bash
dotnet test Source/Tests/StorageScanner.Tests/StorageScanner.Tests.csproj -c Release --nologo
dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj -c Release --nologo -p:S1NoDeploy=true
dotnet build Source/Mods/StorageScanner/src/StorageScanner.csproj -c Release --nologo -p:S1NoDeploy=true
```

The StorageScanner tests link the actual core/controller source. They cover category selection, filtering before aggregation, fallback categories, cache retention, category plus property plus search combinations, reset without an extra scan, save-session resets, neutral readiness, and scan-failure persistence across filter/search/sort/detail changes until the next successful capture. They do not instantiate Unity UI components. Source configuration guards also enforce zero flexible height on fixed bands and disable forced vertical expansion in horizontal groups; these guards do not simulate Unity layout.

## Consolidated in-game round

Prerequisites: use the rebuilt mod, load a save with owned property storage, and keep items from at least two categories. If possible, include a long display name and a stack with a large quantity. Game rendering remains necessary to verify Unity layout and phone input.

| Check | Action | Expected result |
| --- | --- | --- |
| Overview | Open Storage Scanner. | Readable title, search, filter/sort controls, separated item names, prominent quantities, secondary container counts. Neutral footer is not a diagnostic log. No build/version metadata. |
| Compact geometry | Inspect the first open, with and without a warning. | Header/search/toolbar remain compact. Only the item viewport absorbs free height. No selection summary without active filters. |
| Drawer | Open Filters, scroll through all category and property choices, then collapse it. | Drawer is independently scrollable and bounded. Item list remains usable. Chosen category/property remains visible above the list when collapsed. |
| Combined filters | Choose one category and property, then search for an item. | Only matching stock contributes to item totals. Selected choices are highlighted. Empty results produce a readable empty state. |
| Reset | Use Reset after the combined-filter test. | Both filters and the search field clear together; all available stock returns. Reset does not require Refresh. |
| Sort and pooled rows | Cycle all sort modes, change filters, then tap a row that changed position. | Sort label matches ordering. Detail title and locations belong to the tapped item, never the former row occupant. |
| Details | Open a long-name item and scroll its locations, then use Back to stock. | Name wraps without overlapping quantity; locations remain inside the viewport. Detail panel hides the overview and back navigation restores it. |
| Long labels | Inspect long item/property names and large quantities with drawer open and closed. | Names wrap with adequate row height. Quantities stay distinct and the footer remains inside the phone. |
| Last-known stock | Scan one property, leave it for another, and refresh. | Retained items show last known stock. Availability notice is separate from the footer; incomplete stock is not silently presented as complete. |
| Waiting state | Open the app before property metadata is ready. | Neutral waiting notice and No scan yet footer; no false No items found result. |
| Phone lifecycle | Close/reopen the phone and reload a different save. | No duplicate callbacks, stale detail overlay, or search/category/property selection leaking into the next save. |

If a check differs: capture the entire phone (not just a cropped footer), note the active filters and property, and inspect `MelonLoader/Latest.log` for `[StorageScanner]` errors. Do not add raw diagnostics to the normal UI to investigate layout.

## Verification limits

A synthetic 400x750 HTML layout probe can reveal spacing and wrapping mistakes, but it is not a Unity rendering test. A clean build and green controller tests do not establish in-game phone behavior. No gameplay success should be recorded until this round has been completed in Schedule I.
