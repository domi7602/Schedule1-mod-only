namespace PropertyLEDs.Core;

/// <summary>Pure minimum and bounded responsive sizing; no native or settings dependencies.</summary>
public static class AppLayout
{
    public const float SelectorHeight = 68f;
    public const float SliderHeadingHeight = 24f;
    public const float SliderTrackHeight = 28f;
    public const float SliderHeight = SliderHeadingHeight + SliderTrackHeight;
    public const float PreviewHeight = 58f;
    public const float ActionHeight = 52f;
    public const float PowerHeight = 64f;
    public const float AdvancedHeight = 32f;
    public const float Gap = 12f;
    public const float Padding = 4f;

    public readonly record struct Sizing(float Selector, float Preview, float Slider, float Advanced,
        float Power, float Action, float Gap, float Content);

    public const float InlineStateHeight = 32f;

    public static bool ShouldApply(float width, float height, float appliedWidth, float appliedHeight, bool dirty) =>
        float.IsFinite(width) && float.IsFinite(height) && width > 1 && height > 1 &&
        (dirty || System.Math.Abs(width - appliedWidth) > 0.5f || System.Math.Abs(height - appliedHeight) > 0.5f);

    public static Sizing Measure(float viewport, float scale, bool expanded, bool inlineState)
    {
        // Stretch usable controls, not a spacer. Keep minimum hit areas on small screens.
        float state = inlineState ? InlineStateHeight * scale : 0;
        int gaps = 6 + (expanded ? 1 : 0) + (inlineState ? 1 : 0);
        float minimum = ContentHeight(scale, expanded) + state + (inlineState ? Gap * scale : 0);
        float stretch = System.Math.Clamp((viewport - state) / (minimum - state), 1f, 1.5f);
        float gap = System.Math.Clamp(Gap * scale * stretch + System.Math.Max(0, viewport - state - (minimum - state) * stretch) / gaps,
            Gap * scale, Gap * scale * 3f);
        float content = (minimum - state - gaps * Gap * scale) * stretch + gaps * gap + state;
        return new(SelectorHeight * scale * stretch, PreviewHeight * scale * stretch,
            SliderHeight * scale * stretch, AdvancedHeight * scale * stretch,
            PowerHeight * scale * stretch, ActionHeight * scale * stretch, gap, content);
    }

    public readonly record struct Tone(float R, float G, float B, float A);

    public static Tone InlineColor(string? message)
    {
        if (!string.IsNullOrEmpty(message) && message.Contains("Unverified", System.StringComparison.Ordinal))
            return new(0.95f, 0.78f, 0.28f, 1f);
        if (!string.IsNullOrEmpty(message) && message.Contains("failed", System.StringComparison.OrdinalIgnoreCase))
            return new(0.91f, 0.29f, 0.27f, 1f);
        return new(0.80f, 0.85f, 0.90f, 1f);
    }

    // Property card, three sliders, advanced row, power card, presets: seven children.
    public static float ContentHeight(float scale) => ContentHeight(scale, false);

    public static float ContentHeight(float scale, bool advancedExpanded) =>
        (SelectorHeight + 3 * SliderHeight + ActionHeight + AdvancedHeight + PowerHeight +
         6 * Gap + 2 * Padding + (advancedExpanded ? SliderHeight + Gap : 0)) * scale;
}
