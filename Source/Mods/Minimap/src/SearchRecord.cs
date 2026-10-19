namespace Minimap;

/// <summary>
/// The only persisted search state. Deliberately carries world, target, area id/version and
/// the accepted flag - never a completed/found flag, which stays a world observation and not a
/// second authority. Immutable; changing acceptance yields a new instance.
/// </summary>
public sealed class SearchRecord
{
    public SearchRecord(string world, string target, string areaId, int areaVersion, bool accepted)
    {
        World = world ?? string.Empty;
        Target = target ?? string.Empty;
        AreaId = areaId ?? string.Empty;
        AreaVersion = areaVersion;
        Accepted = accepted;
    }

    public string World { get; }

    public string Target { get; }

    public string AreaId { get; }

    public int AreaVersion { get; }

    public bool Accepted { get; }
}
