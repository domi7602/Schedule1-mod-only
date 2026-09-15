namespace BackpackMod.Patches;

/// <summary>
/// Pure layout math for the injected StorageMenu "Sort" button. Lives in a
/// separate file from SortUIInjector so unit tests can target it without
/// pulling in MelonLoader IL2CPP bindings or Unity game types.
///
/// Background (Echtspiel-Report 2026-09-13): the injector used to clone the
/// FIRST Button found under CloseButtonContainer and inherited its rect
/// unchecked. When that hit is a container-level panel instead of the small
/// close button, the clone becomes a giant overlay covering the menu. The
/// injector now picks the smallest Button and enforces explicit geometry
/// computed here: clamped size + left-adjacent placement in the close
/// cluster's anchor space.
/// </summary>
public static class SortButtonLayout
{
    /// <summary>Minimum readable size for the "Sort" label.</summary>
    public const float MinWidth = 110f;
    public const float MinHeight = 36f;

    /// <summary>Maximum sane size — anything larger is a container, not a button.</summary>
    public const float MaxWidth = 220f;
    public const float MaxHeight = 60f;

    /// <summary>Fallback size when the template reports garbage (matches HUD button).</summary>
    public const float DefaultWidth = 150f;
    public const float DefaultHeight = 44f;

    /// <summary>Horizontal gap between the close cluster and the Sort button.</summary>
    public const float Gap = 12f;

    /// <summary>
    /// Clamps a template rect to sane button bounds. Tiny templates (e.g. a
    /// 36px X button) grow to <see cref="MinWidth"/>/<see cref="MinHeight"/> so
    /// the label stays readable; giant templates (container panels) shrink to
    /// <see cref="MaxWidth"/>/<see cref="MaxHeight"/>; NaN/infinity/zero fall
    /// back to <see cref="DefaultWidth"/>/<see cref="DefaultHeight"/>.
    /// </summary>
    public static (float width, float height) ClampButtonSize(float templateWidth, float templateHeight)
    {
        float w = templateWidth;
        if (!float.IsFinite(w) || w <= 0f) w = DefaultWidth;
        else if (w < MinWidth) w = MinWidth;
        else if (w > MaxWidth) w = MaxWidth;

        float h = templateHeight;
        if (!float.IsFinite(h) || h <= 0f) h = DefaultHeight;
        else if (h < MinHeight) h = MinHeight;
        else if (h > MaxHeight) h = MaxHeight;

        return (w, h);
    }

    /// <summary>
    /// anchoredPosition.x for a clone parked left-adjacent to the close
    /// cluster (same anchors + pivot as the cluster). Correct for any pivot:
    /// the clone's right edge lands exactly <see cref="Gap"/> left of the
    /// cluster's left edge.
    /// </summary>
    public static float LeftAdjacentX(float containerAnchoredX, float containerWidth, float cloneWidth, float pivotX)
    {
        return containerAnchoredX - (pivotX * containerWidth + Gap + (1f - pivotX) * cloneWidth);
    }
}
