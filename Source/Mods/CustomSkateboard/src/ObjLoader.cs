using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MelonLoader.Utils;
using UnityEngine;

namespace CustomSkateboard.Visuals;

/// <summary>
/// Lightweight, zero-dependency Wavefront OBJ parser that builds Unity Meshes at runtime.
/// </summary>
public static class ObjLoader
{
    private static Mesh? _cachedCustomMesh;
    private static DateTime _lastLoadedFileTime = DateTime.MinValue;
    private static string? _lastLoadedPath;

    public static void InvalidateCache()
    {
        if (_cachedCustomMesh != null)
        {
            try { if (_cachedCustomMesh.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(_cachedCustomMesh); } catch { }
        }
        _cachedCustomMesh = null;
        _lastLoadedFileTime = DateTime.MinValue;
        _lastLoadedPath = null;
    }

    /// <summary>
    /// Searches for a custom skateboard deck OBJ model and loads/caches it if found.
    /// </summary>
    public static Mesh? TryGetOrLoadDeckMesh()
    {
        string[] possiblePaths = new[]
        {
            Path.Combine(MelonEnvironment.UserDataDirectory, "CustomSkateboard", "custom_deck.obj"),
            Path.Combine(MelonEnvironment.ModsDirectory, "CustomSkateboard", "models", "skateboard.obj"),
            Path.Combine(MelonEnvironment.ModsDirectory, "models", "skateboard.obj")
        };

        foreach (var p in possiblePaths)
        {
            if (File.Exists(p))
            {
                DateTime mtime = File.GetLastWriteTimeUtc(p);
                if (_cachedCustomMesh != null && _lastLoadedPath == p && _lastLoadedFileTime == mtime)
                {
                    return _cachedCustomMesh;
                }

                Mesh? loaded = LoadMeshFromObj(p);
                if (loaded != null)
                {
                    if (_cachedCustomMesh != null && _cachedCustomMesh != loaded)
                    {
                        try { if (_cachedCustomMesh.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(_cachedCustomMesh); } catch { }
                    }
                    _cachedCustomMesh = loaded;
                    _lastLoadedPath = p;
                    _lastLoadedFileTime = mtime;
                    Mod.Log.Info($"Loaded custom 3D deck mesh from '{p}'.");
                    return _cachedCustomMesh;
                }
            }
        }

        return _cachedCustomMesh;
    }

    public static Mesh? LoadMeshFromObj(string objPath)
    {
        try
        {
            if (!File.Exists(objPath))
            {
                Mod.Log.Warn($"OBJ file not found: {objPath}");
                return null;
            }

            List<Vector3> rawVertices = new();
            List<Vector3> rawNormals = new();
            List<Vector2> rawUVs = new();

            List<Vector3> outVertices = new();
            List<Vector3> outNormals = new();
            List<Vector2> outUVs = new();
            List<int> outTriangles = new();

            Dictionary<string, int> vertexIndexMap = new();
            int validNormalCount = 0;

            string[] lines = File.ReadAllLines(objPath);
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                    continue;

                string[] parts = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                switch (parts[0])
                {
                    case "v":
                        if (parts.Length >= 4)
                        {
                            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                            // Convert Blender Z-up to Unity Y-up: (X, Z, -Y) — fixes mirrored deck (M10)
                            rawVertices.Add(new Vector3(x, z, -y));
                        }
                        break;

                    case "vn":
                        if (parts.Length >= 4)
                        {
                            float nx = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float ny = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            float nz = float.Parse(parts[3], CultureInfo.InvariantCulture);
                            Vector3 n = new Vector3(nx, nz, -ny).normalized;
                            rawNormals.Add(n);
                        }
                        break;

                    case "vt":
                        if (parts.Length >= 3)
                        {
                            float u = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float v = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            rawUVs.Add(new Vector2(u, v));
                        }
                        break;

                    case "f":
                        if (parts.Length >= 4)
                        {
                            // Triangulate n-gons / quads with correct CCW front-facing winding: (parts[1], parts[i-1], parts[i])
                            for (int i = 3; i < parts.Length; i++)
                            {
                                if (!TryParseVertexIndex(parts[1], rawVertices.Count, out _) ||
                                    !TryParseVertexIndex(parts[i - 1], rawVertices.Count, out _) ||
                                    !TryParseVertexIndex(parts[i], rawVertices.Count, out _))
                                    continue;
                                AddFaceVertex(parts[1], rawVertices, rawNormals, rawUVs, outVertices, outNormals, outUVs, outTriangles, vertexIndexMap, ref validNormalCount);
                                AddFaceVertex(parts[i - 1], rawVertices, rawNormals, rawUVs, outVertices, outNormals, outUVs, outTriangles, vertexIndexMap, ref validNormalCount);
                                AddFaceVertex(parts[i], rawVertices, rawNormals, rawUVs, outVertices, outNormals, outUVs, outTriangles, vertexIndexMap, ref validNormalCount);
                            }
                        }
                        break;
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = Path.GetFileNameWithoutExtension(objPath);
            mesh.vertices = outVertices.ToArray();
            if (outUVs.Count == outVertices.Count)
                mesh.uv = outUVs.ToArray();
            mesh.triangles = outTriangles.ToArray();

            bool hasImportedNormals = outNormals.Count == outVertices.Count && validNormalCount > 0 && validNormalCount == outVertices.Count;
            if (hasImportedNormals)
            {
                mesh.normals = outNormals.ToArray();
            }
            else
            {
                mesh.RecalculateNormals();
            }

            mesh.RecalculateBounds();
            try { mesh.RecalculateTangents(); } catch { }

            Mod.Log.Info($"Successfully parsed OBJ '{mesh.name}' ({mesh.vertexCount} vertices, {mesh.triangles.Length / 3} triangles).");
            return mesh;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to load OBJ mesh from '{objPath}': {ex.Message}");
            return null;
        }
    }

    private static bool TryParseVertexIndex(string token, int vertexCount, out int vIdx)
    {
        vIdx = -1;
        if (string.IsNullOrWhiteSpace(token)) return false;
        string[] indices = token.Split('/');
        if (indices.Length == 0 || string.IsNullOrWhiteSpace(indices[0])) return false;
        if (!int.TryParse(indices[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int vRaw)) return false;
        int idx = vRaw > 0 ? vRaw - 1 : (vRaw < 0 ? vertexCount + vRaw : -1);
        if (idx < 0 || idx >= vertexCount) return false;
        vIdx = idx;
        return true;
    }

    private static void AddFaceVertex(
        string token,
        List<Vector3> rawV, List<Vector3> rawVN, List<Vector2> rawVT,
        List<Vector3> outV, List<Vector3> outVN, List<Vector2> outVT,
        List<int> outTri,
        Dictionary<string, int> indexMap,
        ref int validNormalCount)
    {
        if (indexMap.TryGetValue(token, out int existingIndex))
        {
            outTri.Add(existingIndex);
            return;
        }

        string[] indices = token.Split('/');
        if (indices.Length == 0 || string.IsNullOrWhiteSpace(indices[0])) return;
        if (!int.TryParse(indices[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int vRaw)) return;
        int vIdx = vRaw > 0 ? vRaw - 1 : (vRaw < 0 ? rawV.Count + vRaw : -1);
        int vtIdx = -1;
        if (indices.Length > 1 && !string.IsNullOrEmpty(indices[1]))
        {
            if (!int.TryParse(indices[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int vtRaw)) vtIdx = -1;
            else vtIdx = vtRaw > 0 ? vtRaw - 1 : (vtRaw < 0 ? rawVT.Count + vtRaw : -1);
        }
        int vnIdx = -1;
        if (indices.Length > 2 && !string.IsNullOrEmpty(indices[2]))
        {
            if (!int.TryParse(indices[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int vnRaw)) vnIdx = -1;
            else vnIdx = vnRaw > 0 ? vnRaw - 1 : (vnRaw < 0 ? rawVN.Count + vnRaw : -1);
        }

        int newIndex = outV.Count;
        outV.Add(vIdx >= 0 && vIdx < rawV.Count ? rawV[vIdx] : Vector3.zero);

        bool hasValidNormal = vnIdx >= 0 && vnIdx < rawVN.Count;
        if (hasValidNormal)
        {
            validNormalCount++;
            outVN.Add(rawVN[vnIdx]);
        }
        else
        {
            outVN.Add(Vector3.up);
        }

        if (rawVT.Count > 0)
            outVT.Add(vtIdx >= 0 && vtIdx < rawVT.Count ? rawVT[vtIdx] : Vector2.zero);

        indexMap[token] = newIndex;
        outTri.Add(newIndex);
    }
}
