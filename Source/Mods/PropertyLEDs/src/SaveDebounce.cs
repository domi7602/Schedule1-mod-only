using System;

namespace PropertyLEDs.Core
{
    /// <summary>Caller supplies a monotonic clock. A failed write never acknowledges its revision.</summary>
    public sealed class SaveDebounce
    {
        private readonly double _delay;
        private double _lastEdit;
        private long _savedRevision;
        public SaveDebounce(double delaySeconds = 0.3)
        {
            if (!double.IsFinite(delaySeconds) || delaySeconds < 0) throw new ArgumentOutOfRangeException(nameof(delaySeconds));
            _delay = delaySeconds;
        }
        public long Revision { get; private set; }
        public bool IsDirty => Revision != _savedRevision;
        public bool MarkDirty(double now)
        {
            if (!AcceptsTime(now)) return false;
            _lastEdit = now;
            Revision++;
            return true;
        }
        internal bool AcceptsTime(double now)
            => double.IsFinite(now) && now >= 0 && (Revision == 0 || now >= _lastEdit);
        public bool ShouldSave(double now, bool force = false)
            => IsDirty && double.IsFinite(now) && now >= _lastEdit && (force || now - _lastEdit >= _delay);
        public bool MarkSaved(long revision)
        {
            if (revision != Revision || !IsDirty) return false;
            _savedRevision = revision;
            return true;
        }
    }
}
