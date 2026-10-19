// ============================================================================
// Consolidated domain file: Minimap search-mode data model
//
// Mechanically merged from the previously separate sources listed below.
// Each original file is preserved verbatim inside its own block namespace, so
// every local import binding stays scoped exactly as before. Only the file-scoped
// namespace declarations became block namespaces; no type, initializer, method
// body or using directive was rewritten.
//
//   - Source/Mods/Minimap/src/Search/Core/SearchIdentity.cs
//   - Source/Mods/Minimap/src/Search/Core/SearchObservation.cs
//
// Domain: the Unity-free search model carried between the runtime adapter and the
// controller (opaque verified identity plus one adapter observation).
// ============================================================================

// ==== BEGIN original: Source/Mods/Minimap/src/Search/Core/SearchIdentity.cs ====
namespace Minimap
{
    using System;


    /// <summary>
    /// Opaque, adapter-verified world and target identity for the optional search mode.
    /// The controller never derives or invents identity; it only carries the strings the
    /// runtime adapter supplies once it has verified them. An empty value means unknown.
    /// </summary>
    public readonly struct SearchIdentity : IEquatable<SearchIdentity>
    {
        private readonly string? _world;
        private readonly string? _target;

        public SearchIdentity(string? world, string? target)
        {
            _world = world;
            _target = target;
        }

        public string World => _world ?? string.Empty;

        public string Target => _target ?? string.Empty;

        /// <summary>Both world and target were supplied (non-empty) by the adapter.</summary>
        public bool IsKnown => World.Length > 0 && Target.Length > 0;

        public bool Equals(SearchIdentity other)
            => string.Equals(World, other.World, StringComparison.Ordinal)
            && string.Equals(Target, other.Target, StringComparison.Ordinal);

        public override bool Equals(object? obj) => obj is SearchIdentity other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(
                StringComparer.Ordinal.GetHashCode(World),
                StringComparer.Ordinal.GetHashCode(Target));

        public static bool operator ==(SearchIdentity left, SearchIdentity right) => left.Equals(right);

        public static bool operator !=(SearchIdentity left, SearchIdentity right) => !left.Equals(right);

        public override string ToString() => World + "/" + Target;
    }
}
// ==== END original: Source/Mods/Minimap/src/Search/Core/SearchIdentity.cs ====

// ==== BEGIN original: Source/Mods/Minimap/src/Search/Core/SearchObservation.cs ====
namespace Minimap
{

    /// <summary>
    /// One adapter observation of the current world for the optional search mode.
    /// <see cref="Collected"/> is nullable: null means the adapter could not reliably read the
    /// state (not loaded, manager unavailable, read error) and is never treated as false.
    /// Proximity, target position and a missing pickup are NOT collection signals.
    /// </summary>
    public readonly struct SearchObservation
    {
        public SearchObservation(
            bool ready,
            bool? collected,
            SearchIdentity identity,
            bool targetKnown,
            float targetX,
            float targetY,
            float targetZ)
        {
            Ready = ready;
            Collected = collected;
            Identity = identity;
            TargetKnown = targetKnown;
            TargetX = targetX;
            TargetY = targetY;
            TargetZ = targetZ;
        }

        /// <summary>Load readiness confirmed (world loaded, manager alive, reads contained).</summary>
        public bool Ready { get; }

        /// <summary>Reliable collection state; null = unknown.</summary>
        public bool? Collected { get; }

        /// <summary>Adapter-verified world/target identity; unknown when IsKnown is false.</summary>
        public SearchIdentity Identity { get; }

        public bool TargetKnown { get; }

        public float TargetX { get; }

        public float TargetY { get; }

        public float TargetZ { get; }

        /// <summary>An all-unknown observation: not ready, unknown collection, unknown identity.</summary>
        public static SearchObservation Unknown => default;
    }
}
// ==== END original: Source/Mods/Minimap/src/Search/Core/SearchObservation.cs ====
