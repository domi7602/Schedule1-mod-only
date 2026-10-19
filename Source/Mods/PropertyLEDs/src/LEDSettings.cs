using System;

namespace PropertyLEDs.Core
{
    public readonly struct Rgb
    {
        public Rgb(float r, float g, float b) { R = r; G = g; B = b; }
        public float R { get; }
        public float G { get; }
        public float B { get; }
    }

    public sealed class LEDSettings
    {
        public LEDSettings Clone() => (LEDSettings)MemberwiseClone();

        public static LEDSettings White() => new LEDSettings { Enabled = true };

        public void Normalize()
        {
            Hue = FiniteClamp(Hue, 0f, 1f, 0f);
            Saturation = FiniteClamp(Saturation, 0f, 1f, 0f);
            ColorIntensity = FiniteClamp(ColorIntensity, 0f, 1f, 1f);
            Brightness = FiniteClamp(Brightness, 0f, 5f, 1f);
        }

        private static float FiniteClamp(float value, float min, float max, float fallback)
            => float.IsFinite(value) ? Math.Clamp(value, min, max) : fallback;

        /// <summary>
        /// An edit to a colour parameter switches the lights on so the change is visible immediately:
        /// an explicit "off" only holds until the effective settings change. Brightness 0 (and
        /// non-finite brightness) stays off — that is the documented "no visible effect" state — and
        /// an existing explicit "on" is never undone by this call.
        /// </summary>
        public void EnableForEdit()
        {
            if (float.IsFinite(Brightness) && Brightness > 0f) Enabled = true;
        }

        public Rgb ToRgb()
        {
            var value = Clone();
            value.Normalize();
            float h = (value.Hue % 1f) * 6f;
            float chroma = value.ColorIntensity * value.Saturation;
            float x = chroma * (1f - Math.Abs(h % 2f - 1f));
            float m = value.ColorIntensity - chroma;
            return (int)h switch
            {
                0 => new Rgb(chroma + m, x + m, m),
                1 => new Rgb(x + m, chroma + m, m),
                2 => new Rgb(m, chroma + m, x + m),
                3 => new Rgb(m, x + m, chroma + m),
                4 => new Rgb(x + m, m, chroma + m),
                _ => new Rgb(chroma + m, m, x + m)
            };
        }

        public bool Enabled { get; set; }
        public float Hue { get; set; }
        public float Saturation { get; set; }
        public float ColorIntensity { get; set; } = 1f;
        public float Brightness { get; set; } = 1f;
    }
}
