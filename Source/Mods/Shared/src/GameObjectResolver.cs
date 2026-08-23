using System;
using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Resiliente Suche und Auflösung von GameObjects und UI-Komponenten.
/// Verhindert NullReferenceExceptions, wenn Entwickler in Spiel-Updates
/// GameObject-Hierarchien verschieben, umbenennen oder neu strukturieren.
/// </summary>
public static class GameObjectResolver
{
    /// <summary>
    /// Wird von SceneGate bei Szenenwechseln aufgerufen, um etwaige gecachte UI-Referenzen zu invalidieren.
    /// </summary>
    public static void InvalidateCache()
    {
        // Hook für zukünftige UI-Referenzcaches
    }

    /// <summary>
    /// Sucht rekursiv nach einer Komponente vom Typ T unterhalb von root.
    /// Wenn hintName angegeben ist, wird nur die Komponente mit passendem Namen zurückgegeben (oder null, falls nicht gefunden).
    /// Wenn hintName leer ist, wird die erste gefundene Komponente zurückgegeben.
    /// </summary>
    public static T? FindComponentDeep<T>(GameObject? root, string hintName = "", ModLogger? log = null) where T : Component
    {
        if (root == null)
            return null;

        var components = root.GetComponentsInChildren<T>(true);
        if (components == null || components.Length == 0)
        {
            if (!string.IsNullOrEmpty(hintName))
                log?.Warn($"GameObjectResolver: Keine Komponente '{typeof(T).Name}' unter '{root.name}' gefunden.");
            return null;
        }

        if (string.IsNullOrEmpty(hintName))
            return components[0];

        foreach (var comp in components)
        {
            if (comp != null && comp.gameObject.name.IndexOf(hintName, StringComparison.OrdinalIgnoreCase) >= 0)
                return comp;
        }

        // Bei explizitem hintName kein blindes Raten des ersten Kindes!
        log?.Warn($"GameObjectResolver: Keine Komponente '{typeof(T).Name}' mit Namen '{hintName}' unter '{root.name}' gefunden.");
        return null;
    }

    /// <summary>
    /// Sucht nach einem Kind-GameObject anhand eines Namens oder Teilnamens (rekursiv).
    /// </summary>
    public static GameObject? FindChildDeep(GameObject? root, string childName, bool exactMatch = false)
    {
        if (root == null || string.IsNullOrEmpty(childName))
            return null;

        return FindChildRecursive(root.transform, childName, exactMatch)?.gameObject;
    }

    /// <summary>
    /// Sucht über einen flexiblen Pfad (z. B. "Phone/Apps/Canvas") mit Toleranz gegenüber
    /// fehlenden Zwischenknoten.
    /// </summary>
    public static GameObject? FindByPathFuzzy(GameObject? root, string path)
    {
        if (root == null || string.IsNullOrEmpty(path))
            return null;

        string[] segments = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        Transform? current = root.transform;

        foreach (string segment in segments)
        {
            if (current == null)
                return null;

            Transform? next = FindChildRecursive(current, segment, false);
            if (next != null)
            {
                current = next;
            }
        }

        return current?.gameObject;
    }

    private static Transform? FindChildRecursive(Transform parent, string name, bool exactMatch)
    {
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child == null)
                continue;

            bool matches = exactMatch
                ? string.Equals(child.name, name, StringComparison.OrdinalIgnoreCase)
                : child.name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;

            if (matches)
                return child;

            Transform? foundInSubtree = FindChildRecursive(child, name, exactMatch);
            if (foundInSubtree != null)
                return foundInSubtree;
        }

        return null;
    }
}
