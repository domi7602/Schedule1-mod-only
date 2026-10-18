using System;
using System.Collections.Generic;

namespace PropertyLEDs.Core
{
    public sealed class PropertySettings
    {
        public LEDSettings Global { get; set; } = new LEDSettings();
        public Dictionary<string, LEDSettings> Rooms { get; set; } = new Dictionary<string, LEDSettings>(StringComparer.Ordinal);
    }

    public sealed class LEDState
    {
        public const int CurrentFormatVersion = 1;
        public int FormatVersion { get; set; } = CurrentFormatVersion;
        public Dictionary<string, PropertySettings> Properties { get; set; } = new Dictionary<string, PropertySettings>(StringComparer.Ordinal);

        /// <summary>Deep copy; intentionally does not normalize, so unsupported schemas remain unchanged.</summary>
        public LEDState Clone()
        {
            var result = new LEDState { FormatVersion = FormatVersion };
            if (Properties == null) { result.Properties = null!; return result; }
            foreach (var item in Properties)
            {
                if (item.Value == null) { result.Properties[item.Key] = null!; continue; }
                var source = item.Value;
                var copy = new PropertySettings { Global = source.Global?.Clone()! };
                if (source.Rooms == null) copy.Rooms = null!;
                else foreach (var room in source.Rooms) copy.Rooms[room.Key] = room.Value?.Clone()!;
                result.Properties[item.Key] = copy;
            }
            return result;
        }

        /// <summary>True for the supported schema (including repaired data); false leaves unknown versions untouched.</summary>
        public bool Normalize()
        {
            if (FormatVersion != CurrentFormatVersion) return false;
            var clean = new Dictionary<string, PropertySettings>(StringComparer.Ordinal);
            if (Properties != null)
            {
                foreach (var item in Properties)
                {
                    if (string.IsNullOrWhiteSpace(item.Key) || item.Value == null) continue;
                    var property = item.Value;
                    property.Global ??= new LEDSettings();
                    property.Global.Normalize();
                    var rooms = new Dictionary<string, LEDSettings>(StringComparer.Ordinal);
                    if (property.Rooms != null)
                    {
                        foreach (var room in property.Rooms)
                        {
                            if (string.IsNullOrWhiteSpace(room.Key) || room.Value == null) continue;
                            room.Value.Normalize();
                            rooms[room.Key] = room.Value;
                        }
                    }
                    property.Rooms = rooms;
                    clean[item.Key] = property;
                }
            }
            Properties = clean;
            return true;
        }

        public bool UseProperty(string? propertyId, string? roomId)
        {
            return FormatVersion == CurrentFormatVersion && !string.IsNullOrWhiteSpace(propertyId) &&
                !string.IsNullOrWhiteSpace(roomId) && Properties != null &&
                Properties.TryGetValue(propertyId, out var property) && property?.Rooms != null &&
                property.Rooms.Remove(roomId);
        }

        public bool SetGlobal(string? propertyId, LEDSettings? settings)
        {
            if (FormatVersion != CurrentFormatVersion || string.IsNullOrWhiteSpace(propertyId) || settings == null)
                return false;
            Properties ??= new Dictionary<string, PropertySettings>(StringComparer.Ordinal);
            var copy = settings.Clone();
            copy.Normalize();
            Properties[propertyId] = new PropertySettings { Global = copy };
            return true;
        }

        public bool SetRoom(string? propertyId, string? roomId, LEDSettings? settings)
        {
            if (FormatVersion != CurrentFormatVersion || string.IsNullOrWhiteSpace(propertyId) ||
                string.IsNullOrWhiteSpace(roomId) || settings == null) return false;
            Properties ??= new Dictionary<string, PropertySettings>(StringComparer.Ordinal);
            if (!Properties.TryGetValue(propertyId, out var property) || property == null)
                Properties[propertyId] = property = new PropertySettings();
            property.Rooms ??= new Dictionary<string, LEDSettings>(StringComparer.Ordinal);
            var copy = settings.Clone();
            copy.Normalize();
            property.Rooms[roomId] = copy;
            return true;
        }

        public LEDSettings GetEffective(string? propertyId, string? roomId = null)
        {
            LEDSettings? settings = null;
            if (FormatVersion == CurrentFormatVersion && !string.IsNullOrWhiteSpace(propertyId) &&
                Properties != null && Properties.TryGetValue(propertyId, out var property) && property != null)
            {
                settings = property.Global;
                if (!string.IsNullOrWhiteSpace(roomId) && property.Rooms != null &&
                    property.Rooms.TryGetValue(roomId, out var room) && room != null)
                    settings = room;
            }
            var result = settings?.Clone() ?? new LEDSettings();
            result.Normalize();
            return result;
        }
    }
}
