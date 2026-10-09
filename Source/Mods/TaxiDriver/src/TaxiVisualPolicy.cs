namespace TaxiDriver;

/// <summary>Pure gates for refusing empty mesh data and preserving vanilla visuals on failure.</summary>
internal static class TaxiVisualPolicy
{
    internal static bool HasUsableGeometry(int vertexCount, int subMeshCount, int indexedSubMeshCount) =>
        vertexCount > 0 && subMeshCount > 0 && indexedSubMeshCount > 0;

    internal static bool CanCommitSwap(int usableRendererCount) => usableRendererCount > 0;
}
