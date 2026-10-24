# Stack-limit policy and ownership

> verified: StackLimitMod lessons 2026-09-19; timing note re-checked 2026-10-05 (OnSaveInfoLoaded = 0 firings in the instrumented run).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 26/27 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Inherited StackLimitMod notes; not revalidated against the installed game in this
package. Keep policy separate from native discovery and mutation adapters.

## Define policy before applying

- Bound the requested limit and validate configuration before scanning.
- Keep excluded IDs, original non-stackability, and item-specific restrictions
  explicit. A global limit should not silently make unique/equipped items stackable.
- Snapshot the original value once per confirmed definition lifetime before the
  first owned override. A recreated definition with the same ID needs fresh context.
- Distinguish the original native value from the mod's most recent applied value.
  Use an ownership policy before restoring; do not clobber another mod's later edit.

## Apply and reapply

Discover definitions through the verified bounded scan and registration routes.
Check native wrapper lifetime before reading ID or fields, and retain a diagnostic
reason for an invalid object. Perform native access on the appropriate Unity thread.

Only successful mutation counts as applied. Keep failed candidates available for a
bounded retry and report scan coverage separately. If a setting now excludes an
item or disables non-stackable overrides, restore an existing owned override where
safe; simply skipping the new write would leave the previous override active.

When an instance-level limit method needs a fallback patch, verify the exact owner,
signature, native implementation, and derived dispatch. Apply the same policy there;
do not overwrite a result solely because the definition scan missed an item.

## Patchability and timing

The original notes report a failed attempt to patch a generated
`get_DefaultStackLimit` field accessor. Treat this as a known diagnostic case, not
a universal prohibition on property patches. Inspect the current interop member
and native target before deciding whether a real patch point exists.

The original timing fix used a completed-load refresh plus late-registration
handling. Verify current readiness and callback installation; do not reactivate an
unobserved save-info event merely because an older sample used it.

## Diagnostics

Expose counts for discovered, eligible, applied, already-correct, excluded,
non-stackable, invalid, and failed items. Include representative IDs and current
versus baseline values. Bound reports and avoid per-item logging on every frame.
Historical `stack check`, `stack report`, and report-file names are mod-specific
interfaces; inspect the current project before invoking them.

## Verification

Test native limit 1, ordinary stackable items, exclusions, changed configuration,
recreated definitions, late registration, duplicate scan layers, failed first
application, and another mod changing a value after yours. Verify player inventory,
world storage, shelves, and vehicles only where the project promises coverage.
Use [registry discovery](registry-and-scan.md) and
[capacity](inventory-capacity.md) to distinguish limit defects from fit defects.
