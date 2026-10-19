# Frozen writes and personal-intent persistence

Use these rules for waypoint/bookmark/search stores whose current view changes with the loaded world.

## Failed writes and context changes

- Inspect `SaveAtomic` / `SaveTextAtomic`'s Boolean result. Only `true` may advance a saved generation, clear dirty state, discard a retry, or print a success message. A swallowed IO exception is a failed write, not an empty successful save.
- Freeze the immutable data snapshot AND its already-confirmed destination path before resolving a new slot or clearing the visible collection. Retry the frozen pair, never the old data with the new current path. Capturing rows before resolving a changed path can also copy slot A's rows into slot B.
- On a context switch, clear the presentation even when old-path IO failed, but retain the failed bound write independently. A future reload must not replace newer retained pending data with an older on-disk snapshot. Coalesce retries per confirmed target and bound any retained queue; do not retry every frame or log every frame.
- Advance the visible context revision before an operation that may fail at the pending-write budget. Retain the last unsaved old-context snapshot hidden; otherwise a cached Unity projection can continue showing foreign rows even when the core collection is hidden.
- A null current-view record during a load is not a deletion request. Keep an explicitly readable retained-intent snapshot, bound to its original world/path, until a fresh observation can bind the new view. Clear and cancellation operations must be able to update that retained intent without accidentally accepting it on reload.

## Validation and recovery

- Distinguish unreadable text from readable invalid JSON. `LoadTextSafe` handles the former; its returned text still requires parsing/schema validation and an explicit validated-backup attempt for the latter. Do not blindly assume `.bak` was consulted.
- A validated backup plus a corrupt primary is not automatically writable: the atomic writer may rotate that corrupt primary over the only good backup before replacing it. Keep recovered rows read-only until explicit repair/clear, or implement a separately verified quarantine/recovery transaction.
- Parse and validate into a candidate before replacing committed visible state. If neither primary nor backup validates, preserve/quarantine the source, show a storage fault and avoid silently overwriting it with an empty/default view. A newer unknown schema requires read-only handling, not "repair" into the old schema.
- Bound useful rows and label lengths; reject nonfinite coordinates, validate canonical colors, normalize names before duplicate checks, and make untrusted labels safe for their rendering context. Test the load boundary as well as Add; an Add-only cap does not bound a tampered file.
- A field-only DTO needs consistent `IncludeFields` options or JSON inclusion attributes; property-based DTOs avoid that dependency. `JsonSerializerOptions.IncludeFields` exists on .NET 6. Test the actual production serializer; do not infer correctness from an empty object's successful serialization. Do not invent data lost into `[{}]`.
- Persist personal acceptance/cancellation separately from observed native completion. `accepted=false` is a cancellation, NOT an older world's uncollected-key flag. Native rollback to an uncollected state reopens a search only when its personal accepted record is true.

## Tests and evidence

- Exercise false-write -> retained dirty snapshot -> context switch -> successful original-path retry, then verify both destinations. Assert that success counters/log decisions do not advance on failure.
- Test explicit clear-to-empty, failed clear, backup recovery, invalid primary plus invalid backup, oversized input, duplicates, nonfinite data, and a useful JSON roundtrip. Fault-injected writer tests establish policy, not a real disk-full or native-gameplay verification.
- Observe a native save completing and read the exact saved target. A void return, a stale cached SaveInfo timestamp, or one immediate `IsSaving=false` read does not prove asynchronous IO succeeded.
