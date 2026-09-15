using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoBuilder
{
    public static void BuildPrototypBackpacks()
    {
        string exportDir = "Assets/Prefabs";
        if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

        CreateBackpackPrefab("Tier1_Backpack", exportDir, new Vector3(0.4f, 0.4f, 0.2f), Color.blue);
        CreateBackpackPrefab("Tier2_Backpack", exportDir, new Vector3(0.5f, 0.5f, 0.25f), Color.green);
        CreateBackpackPrefab("Tier3_Backpack", exportDir, new Vector3(0.6f, 0.6f, 0.3f), Color.gray);

        string bundleDir = "Assets/AssetBundles";
        if (!Directory.Exists(bundleDir)) Directory.CreateDirectory(bundleDir);

        BuildPipeline.BuildAssetBundles(bundleDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("AssetBundles built successfully.");
    }

    private static void CreateBackpackPrefab(string name, string dir, Vector3 scale, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.localScale = scale;

        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        go.GetComponent<Renderer>().sharedMaterial = mat;

        string matPath = $"{dir}/{name}_mat.mat";
        AssetDatabase.CreateAsset(mat, matPath);

        string prefabPath = $"{dir}/{name}.prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        Object.DestroyImmediate(go);

        AssetImporter importer = AssetImporter.GetAtPath(prefabPath);
        importer.assetBundleName = "backpacks.bundle";
    }
}
