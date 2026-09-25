# Assets

Runtime assets copied to Mods/ during build belong here.

* `taxi.glb` — Stage 2 visual model (low-poly taxi, ~45 KB, 21 nodes, 8 materials).
  `Directory.Build.targets` deploys `assets/*.glb` to `<GameDir>\Mods\TaxiDriver\`,
  and `TaxiVisual` resolves `UserData\TaxiDriver\assets\taxi.glb` first (first hit
  wins, logged).
