using System;

namespace PropertyLEDs.Core
{
    /// <summary>One instance per slot/load session. Never redirect a dirty session to another slot.</summary>
    public sealed class SlotSession
    {
        private readonly LEDState _state;
        private readonly SaveDebounce _save;
        public SlotSession(int slotNumber, StateLoadResult load, double debounceSeconds = 0.3)
        {
            if (load == null) throw new ArgumentNullException(nameof(load));
            SlotNumber = slotNumber;
            _state = load.State.Clone();
            CanWrite = slotNumber >= 0 && load.CanWrite && _state.Normalize();
            _save = new SaveDebounce(debounceSeconds);
        }
        public int SlotNumber { get; }
        public bool CanWrite { get; }
        public long Revision => _save.Revision;
        public bool IsDirty => _save.IsDirty;
        public LEDState Snapshot() => _state.Clone();
        public bool SetGlobal(string? propertyId, LEDSettings? settings, double now)
        {
            if (!CanEdit(now) || !_state.SetGlobal(propertyId, settings)) return false;
            return _save.MarkDirty(now);
        }
        public bool SetRoom(string? propertyId, string? roomId, LEDSettings? settings, double now)
        {
            if (!CanEdit(now) || !_state.SetRoom(propertyId, roomId, settings)) return false;
            return _save.MarkDirty(now);
        }
        public bool UseProperty(string? propertyId, string? roomId, double now)
        {
            if (!CanEdit(now) || !_state.UseProperty(propertyId, roomId)) return false;
            return _save.MarkDirty(now);
        }
        public bool ShouldSave(double now, bool force = false) => CanWrite && _save.ShouldSave(now, force);
        public bool MarkSaved(long revision) => CanWrite && _save.MarkSaved(revision);
        private bool CanEdit(double now) => CanWrite && _save.AcceptsTime(now);
    }
}
