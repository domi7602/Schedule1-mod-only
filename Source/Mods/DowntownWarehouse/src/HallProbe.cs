using System;
using System.IO;
using System.Text;
using Il2CppScheduleOne.Property;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;

namespace DowntownWarehouse;

/// <summary>
/// One-shot in-game survey of the native hall behind Hank's shop.
/// Writes hierarchy, world bounds and collider sizes so the property host can
/// bind to the real shell instead of guessed coordinates. Read-only: it does not
/// create, move or destroy any native object.
/// </summary>
internal static class HallProbe
{
    private static bool _done;

    internal static void RunOnce()
    {
        if (_done) return;
        _done = true;

        var sb = new StringBuilder();
        sb.AppendLine("# DowntownWarehouse hall probe");
        sb.AppendLine("time=" + DateTime.UtcNow.ToString("O"));

        DumpProperties(sb);
        DumpStore(sb);
        DumpBlueDoor(sb);
        DumpNearby(sb);

        string dir = Path.Combine(MelonEnvironment.UserDataDirectory, "DowntownWarehouse");
        Directory.CreateDirectory(dir);
        string path = Path.Combine(dir, "hall-probe.txt");
        File.WriteAllText(path, sb.ToString());
        MelonLogger.Msg("[DowntownWarehouse] Probe geschrieben: " + path);
    }

    private static void DumpProperties(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("## registered properties");
        try
        {
            var list = Property.Properties;
            if (list == null) { sb.AppendLine("(null)"); return; }
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                if (p == null) continue;
                string code = Safe(() => p.propertyCode);
                string name = Safe(() => p.propertyName);
                string owned = Safe(() => p.IsOwned.ToString());
                string pos = Safe(() => Fmt(p.transform.position));
                sb.AppendLine($"- code={code} name={name} owned={owned} pos={pos}");
            }
        }
        catch (Exception ex) { sb.AppendLine("ERR " + ex.Message); }
    }

    private static void DumpStore(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("## store root " + HallPlan.StoreRootPath);
        var root = GameObject.Find(HallPlan.StoreRootPath);
        if (root == null)
        {
            sb.AppendLine("NOT FOUND by exact path; searching Region_Downtown children");
            var region = GameObject.Find("Map/Hyland Point/Region_Downtown");
            if (region != null) DumpTree(sb, region.transform, 0, 2);
            return;
        }
        DumpTree(sb, root.transform, 0, 4);
    }

    private static void DumpBlueDoor(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("## blue door components " + HallPlan.BlueDoorPath);
        var door = GameObject.Find(HallPlan.BlueDoorPath);
        if (door == null) { sb.AppendLine("NOT FOUND"); return; }
        try
        {
            var comps = door.GetComponentsInChildren<Component>(true);
            for (int i = 0; i < comps.Length; i++)
            {
                var c = comps[i];
                if (c == null) continue;
                string go = c.gameObject.name;
                string active = c.gameObject.activeSelf ? "on" : "off";
                string enabled = "";
                if (c is Behaviour b) enabled = b.enabled ? " enabled" : " disabled";
                else if (c is Renderer r) enabled = r.enabled ? " enabled" : " disabled";
                else if (c is Collider col) enabled = col.enabled ? " enabled" : " disabled";
                sb.AppendLine($"- {c.GetType().Name} on '{go}' active={active}{enabled}");
            }
        }
        catch (Exception ex) { sb.AppendLine("ERR " + ex.Message); }
    }

    private static void DumpNearby(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("## renderers near hall hypothesis center");
        Vector3 center = new(HallPlan.CenterX, HallPlan.FloorTopY + 3f, HallPlan.CenterZ);
        try
        {
            var cols = Physics.OverlapBox(center, new Vector3(12f, 6f, 14f));
            if (cols == null) { sb.AppendLine("(null)"); return; }
            var seen = new System.Collections.Generic.HashSet<int>();
            int n = 0;
            for (int i = 0; i < cols.Length && n < 120; i++)
            {
                var c = cols[i];
                if (c == null) continue;
                var go = c.gameObject;
                if (!seen.Add(go.GetInstanceID())) continue;
                n++;
                string path = PathOf(go.transform);
                string bounds = "";
                var r = go.GetComponent<Renderer>();
                if (r != null) bounds = " bounds=" + Fmt(r.bounds.center) + "/" + Fmt(r.bounds.size);
                string col = c is BoxCollider b ? " box=" + Fmt(b.size) : c.GetType().Name;
                sb.AppendLine($"- {path} layer={go.layer} {col}{bounds}");
            }
        }
        catch (Exception ex) { sb.AppendLine("ERR " + ex.Message); }
    }

    private static void DumpTree(StringBuilder sb, Transform t, int depth, int maxDepth)
    {
        if (depth > maxDepth) return;
        var go = t.gameObject;
        string indent = new string(' ', depth * 2);
        string pos = Fmt(t.position);
        string extra = "";
        var r = go.GetComponent<Renderer>();
        if (r != null) extra += " render=" + Fmt(r.bounds.size);
        var col = go.GetComponent<Collider>();
        if (col is BoxCollider b) extra += " box=" + Fmt(b.size);
        sb.AppendLine($"{indent}- {go.name} active={go.activeSelf} pos={pos}{extra}");
        for (int i = 0; i < t.childCount; i++)
            DumpTree(sb, t.GetChild(i), depth + 1, maxDepth);
    }

    private static string PathOf(Transform t)
    {
        var sb = new StringBuilder();
        while (t != null)
        {
            if (sb.Length > 0) sb.Insert(0, '/');
            sb.Insert(0, t.name);
            t = t.parent;
        }
        return sb.ToString();
    }

    private static string Fmt(Vector3 v) => $"({v.x:0.##},{v.y:0.##},{v.z:0.##})";
    private static string Safe(Func<string> f) { try { return f(); } catch { return "?"; } }
}
