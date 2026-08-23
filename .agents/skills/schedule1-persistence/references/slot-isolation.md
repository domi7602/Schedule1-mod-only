# Slot Isolation

Per-save files: `slot_{SaveSlotNumber}.json`. See `CalculatorState.cs:65-93`, `NotesApp.cs:111`, `PayoutStateStore.cs:23`.

```csharp
private static string _lastKnownSlot = "default";
public static string GetActiveSlotSuffix() {
  try { var info = LoadManager.Instance?.ActiveSaveInfo; if(info!=null){_lastKnownSlot=info.SaveSlotNumber.ToString();return _lastKnownSlot;}}catch{}
  return _lastKnownSlot;
}
public static string GetStateFilePath()=> SafeStorage.GetUserDataPath("MyMod",$"state_slot_{GetActiveSlotSuffix()}.json");

// Legacy once
void TryMigrateLegacy(string slotPath){
  string legacy=SafeStorage.GetUserDataPath("MyMod","state.json");
  if(!File.Exists(legacy))return; if(File.Exists(slotPath)){File.Delete(legacy);return;} File.Move(legacy,slotPath);
}
```

`PayoutStateStore.cs:20` additionally snapshots `_pendingPrevLastPaid` + `_pendingPrevPerBusiness` in `MarkInMemoryPaid:117` and restores in `Revert:149` — else Day 5 re-pays after Day 6 fail.

`HomelessMod`: `street_items_slot_{n}.json` + in-memory only + flush on `OnSaveComplete`.
