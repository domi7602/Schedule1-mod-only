# Changelog

## [0.1.2] - 2026-09-08

### Fixed
* **Sleeping Bag schwebte ~1,5m über dem Boden**: Die Item-Definition wird per `CloneFrom("bed")` vom Bett geklont — Vanilla `BuildUpdate_Grid` leitet daraus einen bett-hohen `verticalOffset` ab, der den flachen prozeduralen Schlafsack in die Luft hob.
  * Ghost-Fix: Für den Sleeping Bag wird der Vanilla-`verticalOffset` jetzt ignoriert (nur Pivot-Korrektur bleibt) — der Ghost liegt beim Platzieren direkt auf dem Boden (`BuildingPatches.BuildUpdate_Grid_CheckIntersections_Patch`).
  * Save-Heilung: Beim Restore wird die gespeicherte Position per Boden-Raycast nach unten gesnappt (`GroundPlacementAssistant.SnapToGround`). Alte Saves mit schwebenden Bags werden automatisch korrigiert; Bags auf Dächern/Brücken (≤0,5m über Fläche) bleiben korrekt liegen. Log-Zeile `[GroundFix]` zeigt die Korrektur an.
