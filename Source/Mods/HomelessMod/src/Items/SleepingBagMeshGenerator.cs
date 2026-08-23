using System;
using System.Collections.Generic;
using UnityEngine;

namespace HomelessMod.Items;

/// <summary>
/// Generates procedural 3D meshes and 2D inventory sprites for the portable Sleeping Bag.
/// Zero external asset dependencies — 100% native Unity mesh creation.
/// </summary>
public static class SleepingBagMeshGenerator
{
    private static Mesh? _sleepingBagMesh;
    private static Sprite? _sleepingBagSprite;
    private static Material? _bagMaterial;

    /// <summary>
    /// Gets or builds the 3D mesh for the unrolled Sleeping Bag.
    /// </summary>
    public static Mesh GetOrCreateMesh()
    {
        if (_sleepingBagMesh != null && _sleepingBagMesh.Pointer != IntPtr.Zero)
        {
            try
            {
                if (_sleepingBagMesh.isReadable)
                {
                    return _sleepingBagMesh;
                }
            }
            catch
            {
                _sleepingBagMesh = null;
            }
        }

        var mesh = new Mesh { name = "Procedural_SleepingBag_Mesh" };
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var triangles = new List<int>();

        // Dimensions (Meters): Length ~ 1.95m, Width ~ 0.85m, Height ~ 0.20m
        float length = 1.95f;
        float width = 0.85f;
        float halfW = width * 0.5f;

        // 1. Foam Ground Mat (Base Layer)
        float matHeight = 0.04f;
        AddBox(vertices, normals, uvs, triangles,
            new Vector3(0f, matHeight * 0.5f, 0f),
            new Vector3(width, matHeight, length));

        // 2. Head Pillow Section (Z: +0.65m to +0.95m)
        float pillowW = width * 0.88f;
        float pillowL = 0.35f;
        float pillowH = 0.10f;
        Vector3 pillowCenter = new Vector3(0f, matHeight + pillowH * 0.5f, length * 0.5f - pillowL * 0.5f - 0.05f);
        AddCurvedDome(vertices, normals, uvs, triangles, pillowCenter, pillowW * 0.5f, pillowL * 0.5f, pillowH, 6, 6);

        // 3. Cocoon Body Quilt (Z: -0.95m to +0.55m)
        float bagW = width * 0.85f;
        float bagL = length - pillowL - 0.15f;
        float bagH = 0.16f;
        Vector3 bagCenter = new Vector3(0f, matHeight + bagH * 0.5f, -length * 0.5f + bagL * 0.5f + 0.05f);
        AddQuiltedBody(vertices, normals, uvs, triangles, bagCenter, bagW * 0.5f, bagL * 0.5f, bagH, 8, 12);

        mesh.SetVertices(vertices.ToArray());
        mesh.SetNormals(normals.ToArray());
        mesh.SetUVs(0, uvs.ToArray());
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        _sleepingBagMesh = mesh;
        return _sleepingBagMesh;
    }

    /// <summary>
    /// Gets or creates a weathered canvas fabric material for the sleeping bag.
    /// </summary>
    public static Material GetOrCreateMaterial()
    {
        if (_bagMaterial != null && _bagMaterial.Pointer != IntPtr.Zero)
        {
            return _bagMaterial;
        }

        // Find URP Lit or fallback shader in Unity 2022.3
        Shader shader = Shader.Find("Universal Render Pipeline/Lit")
            ?? Shader.Find("Universal Render Pipeline/Simple Lit")
            ?? Shader.Find("Universal Render Pipeline/Unlit")
            ?? Shader.Find("Standard")
            ?? Shader.Find("Diffuse")
            ?? Shader.Find("Unlit/Color");

        if (shader == null)
        {
            try
            {
                var renderers = UnityEngine.Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
                if (renderers != null)
                {
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        var r = renderers[i];
                        if (r != null && r.Pointer != IntPtr.Zero && r.sharedMaterial != null && r.sharedMaterial.Pointer != IntPtr.Zero && r.sharedMaterial.shader != null)
                        {
                            shader = r.sharedMaterial.shader;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        _bagMaterial = new Material(shader ?? Shader.Find("Sprites/Default"))
        {
            name = "M_SleepingBag_Fabric"
        };

        Color oliveGreen = new Color(0.24f, 0.40f, 0.28f, 1.0f); // Olive Green Outdoor Canvas
        if (_bagMaterial.HasProperty("_BaseColor"))
            _bagMaterial.SetColor("_BaseColor", oliveGreen);
        if (_bagMaterial.HasProperty("_Color"))
            _bagMaterial.SetColor("_Color", oliveGreen);
        try { _bagMaterial.color = oliveGreen; } catch { }

        if (_bagMaterial.HasProperty("_Smoothness"))
            _bagMaterial.SetFloat("_Smoothness", 0.25f);
        if (_bagMaterial.HasProperty("_Glossiness"))
            _bagMaterial.SetFloat("_Glossiness", 0.25f);
        if (_bagMaterial.HasProperty("_Metallic"))
            _bagMaterial.SetFloat("_Metallic", 0.05f);

        return _bagMaterial;
    }

    /// <summary>
    /// Generates a clean 64x64 item icon sprite for the Sleeping Bag.
    /// </summary>
    public static Sprite GetOrCreateIconSprite()
    {
        if (_sleepingBagSprite != null && _sleepingBagSprite.Pointer != IntPtr.Zero)
        {
            return _sleepingBagSprite;
        }

        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            name = "SleepingBag_Icon",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        Color transparent = new Color(0f, 0f, 0f, 0f);
        Color bagColor = new Color(0.18f, 0.30f, 0.22f, 1f); // Olive green
        Color matColor = new Color(0.12f, 0.12f, 0.12f, 1f); // Dark foam mat
        Color pillowColor = new Color(0.65f, 0.62f, 0.52f, 1f); // Tan headrest
        Color zipColor = new Color(0.85f, 0.55f, 0.15f, 1f); // Orange zipper stripe

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Draw rounded sleeping bag silhouette (centered diagonally or vertically)
                float u = (x - 8f) / (size - 16f);
                float v = (y - 8f) / (size - 16f);

                if (u >= 0f && u <= 1f && v >= 0f && v <= 1f)
                {
                    // Outer border / foam mat
                    float distFromEdge = Mathf.Min(u, 1f - u, v, 1f - v);
                    if (distFromEdge < 0.05f)
                    {
                        tex.SetPixel(x, y, matColor);
                    }
                    else if (v > 0.75f)
                    {
                        // Pillow area
                        tex.SetPixel(x, y, pillowColor);
                    }
                    else if (Mathf.Abs(u - 0.72f) < 0.04f)
                    {
                        // Zipper line
                        tex.SetPixel(x, y, zipColor);
                    }
                    else
                    {
                        // Quilted bag texture pattern
                        float quiltPattern = Mathf.Sin(v * 30f) * 0.05f;
                        Color c = bagColor + new Color(quiltPattern, quiltPattern, quiltPattern, 0f);
                        tex.SetPixel(x, y, c);
                    }
                }
                else
                {
                    tex.SetPixel(x, y, transparent);
                }
            }
        }

        tex.Apply();
        _sleepingBagSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _sleepingBagSprite;
    }

    private static void AddBox(List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<int> tris, Vector3 center, Vector3 size)
    {
        int start = verts.Count;
        Vector3 h = size * 0.5f;

        // 8 box vertices
        Vector3 p0 = center + new Vector3(-h.x, -h.y, -h.z);
        Vector3 p1 = center + new Vector3(h.x, -h.y, -h.z);
        Vector3 p2 = center + new Vector3(h.x, -h.y, h.z);
        Vector3 p3 = center + new Vector3(-h.x, -h.y, h.z);
        Vector3 p4 = center + new Vector3(-h.x, h.y, -h.z);
        Vector3 p5 = center + new Vector3(h.x, h.y, -h.z);
        Vector3 p6 = center + new Vector3(h.x, h.y, h.z);
        Vector3 p7 = center + new Vector3(-h.x, h.y, h.z);

        // Top Face (P4, P5, P6, P7)
        AddQuad(verts, norms, uvs, tris, p4, p5, p6, p7, Vector3.up);
        // Bottom Face
        AddQuad(verts, norms, uvs, tris, p1, p0, p3, p2, Vector3.down);
        // Front Face
        AddQuad(verts, norms, uvs, tris, p7, p6, p2, p3, Vector3.forward);
        // Back Face
        AddQuad(verts, norms, uvs, tris, p5, p4, p0, p1, Vector3.back);
        // Left Face
        AddQuad(verts, norms, uvs, tris, p4, p7, p3, p0, Vector3.left);
        // Right Face
        AddQuad(verts, norms, uvs, tris, p6, p5, p1, p2, Vector3.right);
    }

    private static void AddQuad(List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<int> tris,
        Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal)
    {
        int idx = verts.Count;
        verts.Add(v0); verts.Add(v1); verts.Add(v2); verts.Add(v3);
        norms.Add(normal); norms.Add(normal); norms.Add(normal); norms.Add(normal);
        uvs.Add(new Vector2(0, 0)); uvs.Add(new Vector2(1, 0)); uvs.Add(new Vector2(1, 1)); uvs.Add(new Vector2(0, 1));

        // Front Face
        tris.Add(idx + 0); tris.Add(idx + 1); tris.Add(idx + 2);
        tris.Add(idx + 0); tris.Add(idx + 2); tris.Add(idx + 3);

        // Double-sided back face for safety
        tris.Add(idx + 0); tris.Add(idx + 2); tris.Add(idx + 1);
        tris.Add(idx + 0); tris.Add(idx + 3); tris.Add(idx + 2);
    }

    private static void AddCurvedDome(List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<int> tris,
        Vector3 center, float rx, float rz, float height, int segX, int segZ)
    {
        int start = verts.Count;
        for (int z = 0; z <= segZ; z++)
        {
            float tz = (float)z / segZ;
            float posZ = Mathf.Lerp(-rz, rz, tz);
            float factorZ = 1f - Mathf.Pow(posZ / rz, 2f);

            for (int x = 0; x <= segX; x++)
            {
                float tx = (float)x / segX;
                float posX = Mathf.Lerp(-rx, rx, tx);
                float factorX = 1f - Mathf.Pow(posX / rx, 2f);

                float posY = Mathf.Max(0f, height * Mathf.Sqrt(Mathf.Clamp01(factorX * factorZ)));
                Vector3 p = center + new Vector3(posX, posY, posZ);
                Vector3 n = new Vector3(posX / rx, 1.5f, posZ / rz).normalized;

                verts.Add(p);
                norms.Add(n);
                uvs.Add(new Vector2(tx, tz));
            }
        }

        int stride = segX + 1;
        for (int z = 0; z < segZ; z++)
        {
            for (int x = 0; x < segX; x++)
            {
                int c = start + z * stride + x;
                // Clockwise (Top-facing normal)
                tris.Add(c);
                tris.Add(c + 1);
                tris.Add(c + stride);

                tris.Add(c + 1);
                tris.Add(c + stride + 1);
                tris.Add(c + stride);

                // Double-sided for complete visibility
                tris.Add(c);
                tris.Add(c + stride);
                tris.Add(c + 1);

                tris.Add(c + 1);
                tris.Add(c + stride);
                tris.Add(c + stride + 1);
            }
        }
    }

    private static void AddQuiltedBody(List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<int> tris,
        Vector3 center, float rx, float rz, float height, int segX, int segZ)
    {
        int start = verts.Count;
        for (int z = 0; z <= segZ; z++)
        {
            float tz = (float)z / segZ;
            float posZ = Mathf.Lerp(-rz, rz, tz);
            float factorZ = 1f - Mathf.Pow(Mathf.Abs(posZ / rz), 3f);

            // Subtle quilted ridges along the body
            float ridge = Mathf.Sin(tz * Mathf.PI * 6f) * (height * 0.08f);

            for (int x = 0; x <= segX; x++)
            {
                float tx = (float)x / segX;
                float posX = Mathf.Lerp(-rx, rx, tx);
                float factorX = 1f - Mathf.Pow(Mathf.Abs(posX / rx), 2.5f);

                float posY = Mathf.Max(0f, (height + ridge) * Mathf.Clamp01(factorX * factorZ));
                Vector3 p = center + new Vector3(posX, posY, posZ);
                Vector3 n = new Vector3(posX / rx, 2.0f, (posZ / rz) * 0.5f).normalized;

                verts.Add(p);
                norms.Add(n);
                uvs.Add(new Vector2(tx, tz * 3f));
            }
        }

        int stride = segX + 1;
        for (int z = 0; z < segZ; z++)
        {
            for (int x = 0; x < segX; x++)
            {
                int c = start + z * stride + x;
                // Clockwise (Top-facing normal)
                tris.Add(c);
                tris.Add(c + 1);
                tris.Add(c + stride);

                tris.Add(c + 1);
                tris.Add(c + stride + 1);
                tris.Add(c + stride);

                // Double-sided for complete visibility
                tris.Add(c);
                tris.Add(c + stride);
                tris.Add(c + 1);

                tris.Add(c + 1);
                tris.Add(c + stride);
                tris.Add(c + stride + 1);
            }
        }
    }
}
