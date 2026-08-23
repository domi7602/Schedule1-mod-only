using UnityEngine;

namespace BackpackMod
{
    public static class BackpackIconGenerator
    {
        private static Sprite? _t1Icon;
        private static Sprite? _t2Icon;
        private static Sprite? _t3Icon;
        private static Sprite? _slotIcon;

        public static Sprite GetOrCreateIcon(int tier)
        {
            return tier switch
            {
                1 => _t1Icon ??= CreateBackpackIcon(new Color(0.22f, 0.45f, 0.70f), new Color(0.15f, 0.30f, 0.50f), "T1"),
                2 => _t2Icon ??= CreateBackpackIcon(new Color(0.25f, 0.60f, 0.35f), new Color(0.18f, 0.42f, 0.25f), "T2"),
                3 => _t3Icon ??= CreateBackpackIcon(new Color(0.70f, 0.25f, 0.25f), new Color(0.48f, 0.15f, 0.15f), "T3"),
                _ => _t1Icon ??= CreateBackpackIcon(new Color(0.4f, 0.4f, 0.4f), new Color(0.25f, 0.25f, 0.25f), "BP")
            };
        }

        public static Sprite GetSlotIcon()
        {
            return _slotIcon ??= CreateSlotSilhouetteIcon();
        }

        private static Sprite CreateBackpackIcon(Color mainColor, Color darkColor, string label)
        {
            int size = 128;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = $"Backpack_Icon_{label}",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            Color transparent = new Color(0f, 0f, 0f, 0f);
            Color strapColor = new Color(0.12f, 0.12f, 0.12f, 1f);
            Color zipColor = new Color(0.9f, 0.8f, 0.3f, 1f);
            Color pocketColor = mainColor * 0.85f;
            pocketColor.a = 1f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (float)x / size;
                    float v = (float)y / size;

                    // Main backpack body (rounded rectangle)
                    bool inMainBody = (u >= 0.22f && u <= 0.78f && v >= 0.18f && v <= 0.82f);
                    // Top handle
                    bool inHandle = (u >= 0.38f && u <= 0.62f && v >= 0.82f && v <= 0.92f && !(u >= 0.44f && u <= 0.56f && v >= 0.82f && v <= 0.88f));
                    // Front pocket
                    bool inPocket = (u >= 0.28f && u <= 0.72f && v >= 0.24f && v <= 0.52f);
                    // Straps (sides)
                    bool inStraps = ((u >= 0.15f && u <= 0.22f) || (u >= 0.78f && u <= 0.85f)) && (v >= 0.35f && v <= 0.75f);
                    // Zipper lines
                    bool inPocketZip = (u >= 0.30f && u <= 0.70f && Mathf.Abs(v - 0.52f) < 0.018f);
                    bool inMainZip = (u >= 0.24f && u <= 0.76f && Mathf.Abs(v - 0.75f) < 0.018f);

                    if (inHandle || inStraps)
                    {
                        tex.SetPixel(x, y, strapColor);
                    }
                    else if (inPocketZip || inMainZip)
                    {
                        tex.SetPixel(x, y, zipColor);
                    }
                    else if (inPocket)
                    {
                        tex.SetPixel(x, y, pocketColor);
                    }
                    else if (inMainBody)
                    {
                        // Add subtle vertical gradient
                        float grad = Mathf.Lerp(0.85f, 1.15f, v);
                        Color c = mainColor * grad;
                        c.a = 1f;
                        tex.SetPixel(x, y, c);
                    }
                    else
                    {
                        tex.SetPixel(x, y, transparent);
                    }
                }
            }

            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        private static Sprite CreateSlotSilhouetteIcon()
        {
            int size = 64;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "Backpack_Slot_Silhouette",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            Color transparent = new Color(0f, 0f, 0f, 0f);
            Color white = new Color(1f, 1f, 1f, 0.85f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (float)x / size;
                    float v = (float)y / size;

                    bool inBody = (u >= 0.25f && u <= 0.75f && v >= 0.20f && v <= 0.80f);
                    bool inHandle = (u >= 0.40f && u <= 0.60f && v >= 0.80f && v <= 0.90f && !(u >= 0.45f && u <= 0.55f && v >= 0.80f && v <= 0.86f));

                    if (inBody || inHandle)
                    {
                        tex.SetPixel(x, y, white);
                    }
                    else
                    {
                        tex.SetPixel(x, y, transparent);
                    }
                }
            }

            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }
    }
}
