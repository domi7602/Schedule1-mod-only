extern alias il2cpp;

using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using UnityEngine;

namespace PocketShop.Services;

/// <summary>
/// Provides high-resolution avatars for each shop by prioritizing the real
/// 3D NPC shopkeeper mugshots (Dan, Hank, Oscar, etc.) from the game,
/// falling back to crisp themed vector icons for non-NPC stores (Gas-Mart, etc.).
/// Known vanilla gap (verified 2026-10-01 via log diagnostics): Stan Carney carries
/// no MugshotSprite in the game data although his ShopInterface correctly points at
/// 'armsdealer'. Per user decision (v0.3.7) the arms card shows Manny the Fixer's
/// portrait until TVGS ships a mugshot — Stan stays first in line and wins
/// automatically once his mugshot exists.
/// </summary>
public static class NPCPortraitService
{
    private static readonly Dictionary<string, Sprite> _cache = new();
    private static readonly HashSet<Sprite> _proceduralSprites = new();
    private static Sprite? _circleSprite;

    public static Sprite GetCircleSprite(int size = 64)
    {
        if (_circleSprite != null && _circleSprite.Pointer != IntPtr.Zero && !_circleSprite.WasCollected)
        {
            return _circleSprite;
        }

        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        var pixels = new Color32[size * size];
        int center = size / 2;
        float radiusSq = (size * 0.48f) * (size * 0.48f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                pixels[y * size + x] = (dx * dx + dy * dy <= radiusSq)
                    ? new Color32(255, 255, 255, 255)
                    : new Color32(0, 0, 0, 0);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply(false, true);
        _circleSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _circleSprite;
    }

    public static Sprite GetAvatar(string shopCode, string shopName, int size = 128)
    {
        // Gas-Marts swap clerks at 6:00/18:00 — keep the day and night portraits in
        // separate cache entries so a still-open store list picks the right face (v0.3.3).
        string shiftToken = IsGasMart(shopCode) ? (IsDayShiftNow() ? "_d" : "_n") : string.Empty;
        string key = $"{shopCode}_{shopName}_{size}{shiftToken}";
        if (_cache.TryGetValue(key, out var cached) && cached != null && cached.Pointer != IntPtr.Zero && !cached.WasCollected)
        {
            return cached;
        }

        // 1. Try to fetch the real 3D NPC MugshotSprite from the game
        var npcSprite = TryGetNpcMugshot(shopCode, shopName);
        if (npcSprite != null && npcSprite.Pointer != IntPtr.Zero && !npcSprite.WasCollected)
        {
            _cache[key] = npcSprite;
            return npcSprite;
        }

        // 2. Fallback to crisp procedural themed icon (e.g. Gas-Mart, Boutique)
        var iconSprite = CreateThemedStoreIcon(shopCode, shopName, size);
        _cache[key] = iconSprite;
        _proceduralSprites.Add(iconSprite);
        return iconSprite;
    }

    private static Sprite? TryGetNpcMugshot(string shopCode, string shopName)
    {
        try
        {
            if (!NPCManager.InstanceExists || NPCManager.NPCRegistry == null || NPCManager.NPCRegistry.Count == 0)
            {
                return null;
            }

            var registry = NPCManager.NPCRegistry;
            string sCode = (shopCode ?? string.Empty).ToLowerInvariant();
            string sName = (shopName ?? string.Empty).ToLowerInvariant();

            // 0a. Ground truth (v0.3.3): the shopkeeper classes carry their shop as a
            // direct ShopInterface reference — binding via that field beats name guessing.
            var shopkeeperSprite = TryGetShopkeeperMugshot(shopCode, shopName, registry);
            if (shopkeeperSprite != null)
            {
                return shopkeeperSprite;
            }

            // 0b. Gas-Marts are staffed by shift clerks — pick the one on duty (v0.3.3).
            var gasClerkSprite = TryGetGasMartMugshot(shopCode, registry);
            if (gasClerkSprite != null)
            {
                return gasClerkSprite;
            }

            // 1. Known shopkeeper ID mapping
            string? targetNpcId = null;
            string? fallbackNpcId = null;
            if (sCode.Contains("dan") || sName.Contains("dan")) targetNpcId = "dan_samwell";
            else if (sCode.Contains("hank") || sName.Contains("hank")) targetNpcId = "hank_stevenson";
            else if (sCode.Contains("oscar") || sName.Contains("oscar")) targetNpcId = "oscar_holland";
            else if (sCode.Contains("salvador") || sName.Contains("salvador")) targetNpcId = "salvador_moreno";
            else if (sCode.Contains("shirley") || sName.Contains("shirley")) targetNpcId = "shirley_watts";
            else if (sCode.Contains("albert") || sName.Contains("albert")) targetNpcId = "albert_hoover";
            else if (sCode.Contains("fungal") || sCode.Contains("phil") || sName.Contains("phil")) targetNpcId = "philip_wentworth";
            else if (sCode.Contains("arm") || sCode.Contains("weapon") || sName.Contains("arm") || sName.Contains("weapon"))
            {
                // v0.3.7: Stan is the correct dealer but ships no mugshot in vanilla
                // data — Manny the Fixer's portrait is the visible fallback (wrong
                // person, real face) per user decision. Stan wins automatically once
                // TVGS ships his mugshot (shopkeeper-field path runs first).
                targetNpcId = "stan_carney";
                fallbackNpcId = "manny_oakfield";
            }
            else if (sCode.Contains("herbert") || sName.Contains("herbert") || sCode.Contains("bleuball") || sName.Contains("bleuball")) targetNpcId = "herbert_bleuball";
            else if (sCode.Contains("fiona") || sName.Contains("fiona") || sCode.Contains("thrifty") || sName.Contains("thrifty")) targetNpcId = "fiona_hancock";
            else if (sCode.Contains("igor") || sName.Contains("igor")) targetNpcId = "igor_romanovich";

            if (!string.IsNullOrEmpty(targetNpcId))
            {
                for (int i = 0; i < registry.Count; i++)
                {
                    var npc = registry[i];
                    if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) continue;

                    if (string.Equals(npc.ID, targetNpcId, StringComparison.OrdinalIgnoreCase))
                    {
                        var sprite = npc.MugshotSprite;
                        if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                        {
                            MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {targetNpcId} (keyword)");
                            return sprite;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(fallbackNpcId))
            {
                for (int i = 0; i < registry.Count; i++)
                {
                    var npc = registry[i];
                    if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) continue;

                    if (string.Equals(npc.ID, fallbackNpcId, StringComparison.OrdinalIgnoreCase))
                    {
                        var sprite = npc.MugshotSprite;
                        if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                        {
                            MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {fallbackNpcId} (keyword-fallback)");
                            return sprite;
                        }
                    }
                }
            }

            // 2. Component inspection: find NPC with a matching ShopInterface
            for (int i = 0; i < registry.Count; i++)
            {
                var npc = registry[i];
                if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) continue;

                var shopInterface = npc.GetComponentInChildren<ShopInterface>();
                if (shopInterface != null && shopInterface.Pointer != IntPtr.Zero && !shopInterface.WasCollected)
                {
                    if (string.Equals(shopInterface.ShopCode, shopCode, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(shopInterface.ShopName, shopName, StringComparison.OrdinalIgnoreCase))
                    {
                        var sprite = npc.MugshotSprite;
                        if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                        {
                            MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {npc.ID} (component-scan)");
                            return sprite;
                        }
                    }
                }
            }

            // 3. Fuzzy search on ID / FullName
            for (int i = 0; i < registry.Count; i++)
            {
                var npc = registry[i];
                if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) continue;

                string npcId = npc.ID?.ToLowerInvariant() ?? string.Empty;
                string npcName = npc.FullName?.ToLowerInvariant() ?? string.Empty;

                if (!string.IsNullOrEmpty(npcId) && (sCode.Contains(npcId) || sName.Contains(npcId)))
                {
                    var sprite = npc.MugshotSprite;
                    if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                    {
                        MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {npc.ID} (name-match)");
                        return sprite;
                    }
                }

                if (!string.IsNullOrEmpty(npcName) && sName.Contains(npcName))
                {
                    var sprite = npc.MugshotSprite;
                    if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                    {
                        MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {npc.ID} (name-match)");
                        return sprite;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[PocketShop] Failed to retrieve NPC mugshot for '{shopCode}': {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// Ground-truth binding (v0.3.3): the six shopkeeper classes in the game carry a
    /// direct <see cref="ShopInterface"/> reference to the shop they run. Match that
    /// reference against the catalog's shop code/name and use the shopkeeper's mugshot.
    /// Covers Dan's Hardware, Thrifty Threads (Fiona), Bleuball's Boutique (Herbert),
    /// Oscar, the Warehouse Arms Dealer (Stan) and Handy Hank's (Steve class).
    /// </summary>
    private static Sprite? TryGetShopkeeperMugshot(string? shopCode, string? shopName, il2cpp::Il2CppSystem.Collections.Generic.List<NPC> registry)
    {
        try
        {
            for (int i = 0; i < registry.Count; i++)
            {
                var npc = registry[i];
                if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) continue;

                var si = GetShopInterfaceRef(npc);
                if (si == null || si.Pointer == IntPtr.Zero || si.WasCollected) continue;

                string siCode = si.ShopCode ?? string.Empty;
                string siName = si.ShopName ?? string.Empty;
                bool codeMatch = !string.IsNullOrEmpty(shopCode) && !string.IsNullOrEmpty(siCode) &&
                                 string.Equals(siCode, shopCode, StringComparison.OrdinalIgnoreCase);
                bool nameMatch = !string.IsNullOrEmpty(shopName) && !string.IsNullOrEmpty(siName) &&
                                 string.Equals(siName, shopName, StringComparison.OrdinalIgnoreCase);
                if (!codeMatch && !nameMatch) continue;

                var sprite = npc.MugshotSprite;
                if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                {
                    MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {npc.ID} (shopkeeper-field)");
                    return sprite;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[PocketShop] shopkeeper-field binding failed for '{shopCode}': {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// Returns the shop reference a shopkeeper NPC carries, or null for every other NPC.
    /// Only these six classes hold the field (verified against the game's CharacterClasses).
    /// </summary>
    private static ShopInterface? GetShopInterfaceRef(NPC npc)
    {
        try
        {
            var dan = npc.TryCast<Il2CppScheduleOne.NPCs.CharacterClasses.Dan>();
            if (dan != null && dan.Pointer != IntPtr.Zero && !dan.WasCollected) return dan.ShopInterface;

            var fiona = npc.TryCast<Il2CppScheduleOne.NPCs.CharacterClasses.Fiona>();
            if (fiona != null && fiona.Pointer != IntPtr.Zero && !fiona.WasCollected) return fiona.ShopInterface;

            var herbert = npc.TryCast<Il2CppScheduleOne.NPCs.CharacterClasses.Herbert>();
            if (herbert != null && herbert.Pointer != IntPtr.Zero && !herbert.WasCollected) return herbert.ShopInterface;

            var oscar = npc.TryCast<Il2CppScheduleOne.NPCs.CharacterClasses.Oscar>();
            if (oscar != null && oscar.Pointer != IntPtr.Zero && !oscar.WasCollected) return oscar.ShopInterface;

            var steve = npc.TryCast<Il2CppScheduleOne.NPCs.CharacterClasses.Steve>();
            if (steve != null && steve.Pointer != IntPtr.Zero && !steve.WasCollected) return steve.ShopInterface;

            var stan = npc.TryCast<Il2CppScheduleOne.NPCs.Stan>();
            if (stan != null && stan.Pointer != IntPtr.Zero && !stan.WasCollected) return stan.ShopInterface;
        }
        catch { }

        return null;
    }

    private static bool IsGasMart(string shopCode)
    {
        string code = (shopCode ?? string.Empty).ToLowerInvariant();
        return code.Contains("gas_mart_west") || code.Contains("gas_mart_central");
    }

    /// <summary>
    /// True while the gas clerks' day shift (06:00-18:00 in-game time) is running.
    /// Defaults to day when no TimeManager instance exists (menus, scene loads).
    /// </summary>
    private static bool IsDayShiftNow()
    {
        try
        {
            int t = S1API.GameTime.TimeManager.CurrentTime; // 24-hour, e.g. 1330
            return t >= 600 && t < 1800;
        }
        catch
        {
            return true;
        }
    }

    private static Sprite? TryGetMugshotById(string? npcId, il2cpp::Il2CppSystem.Collections.Generic.List<NPC> registry)
    {
        for (int i = 0; i < registry.Count; i++)
        {
            var npc = registry[i];
            if (npc == null || npc.Pointer == IntPtr.Zero || npc.WasCollected) continue;
            if (!string.Equals(npc.ID, npcId, StringComparison.OrdinalIgnoreCase)) continue;

            var sprite = npc.MugshotSprite;
            if (sprite != null && sprite.Pointer != IntPtr.Zero && !sprite.WasCollected)
                return sprite;

            return null; // NPC exists but has no usable mugshot
        }

        return null;
    }

    /// <summary>
    /// Gas-Marts run two clerks per location on a shift rotation (v0.3.3):
    /// West = Chloe Bowers day / Charles Rowland night, Central = Meg Cooley day /
    /// Javier Pérez night. Day = 06:00-18:00 in-game time; the other clerk is the
    /// fallback when the on-duty one cannot be resolved.
    /// </summary>
    private static Sprite? TryGetGasMartMugshot(string? shopCode, il2cpp::Il2CppSystem.Collections.Generic.List<NPC> registry)
    {
        try
        {
            string code = (shopCode ?? string.Empty).ToLowerInvariant();
            string dayId;
            string nightId;
            if (code.Contains("gas_mart_west"))
            {
                dayId = "chloe_bowers";
                nightId = "charles_rowland";
            }
            else if (code.Contains("gas_mart_central"))
            {
                dayId = "meg_cooley";
                nightId = "javier_perez";
            }
            else
            {
                return null;
            }

            bool day = IsDayShiftNow();
            string primary = day ? dayId : nightId;
            string secondary = day ? nightId : dayId;
            string shift = day ? "gas-day" : "gas-night";

            var sprite = TryGetMugshotById(primary, registry);
            if (sprite != null)
            {
                MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {primary} ({shift})");
                return sprite;
            }

            sprite = TryGetMugshotById(secondary, registry);
            if (sprite != null)
            {
                MelonLogger.Msg($"[PocketShop] portrait {shopCode} -> {secondary} ({shift}, other-shift fallback)");
                return sprite;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[PocketShop] gas-mart portrait failed for '{shopCode}': {ex.Message}");
        }

        return null;
    }

    private static Sprite CreateThemedStoreIcon(string shopCode, string shopName, int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        var pixels = new Color32[size * size];

        var baseColor = ShopColorScheme.ColorFor(shopCode);
        string lowerCode = (shopCode ?? string.Empty).ToLowerInvariant();
        string lowerName = (shopName ?? string.Empty).ToLowerInvariant();

        // 1. Fill circle background
        int center = size / 2;
        float radius = size * 0.44f;
        float radiusSq = radius * radius;
        float innerRadiusSq = (radius - 2.5f) * (radius - 2.5f);

        Color32 bgC32 = new Color(baseColor.r * 0.85f, baseColor.g * 0.85f, baseColor.b * 0.85f, 0.40f);
        Color32 borderC32 = new Color(1f, 1f, 1f, 0.35f);
        Color32 fgC32 = Color.white;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                float distSq = dx * dx + dy * dy;

                if (distSq <= innerRadiusSq)
                {
                    pixels[y * size + x] = bgC32;
                }
                else if (distSq <= radiusSq)
                {
                    pixels[y * size + x] = borderC32;
                }
                else
                {
                    pixels[y * size + x] = new Color32(0, 0, 0, 0);
                }
            }
        }

        // 2. Draw specific crisp vector silhouette based on store category
        if (lowerCode.Contains("gas") || lowerName.Contains("gas"))
        {
            DrawGasPump(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("thread") || lowerCode.Contains("cloth") || lowerName.Contains("thread") || lowerName.Contains("cloth"))
        {
            DrawTShirt(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("arm") || lowerCode.Contains("weapon") || lowerName.Contains("arm") || lowerName.Contains("weapon"))
        {
            DrawCrosshair(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("boutique") || lowerCode.Contains("playball") || lowerName.Contains("boutique") || lowerName.Contains("playball"))
        {
            DrawDiamond(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("fungal") || lowerCode.Contains("phil") || lowerName.Contains("fungal") || lowerName.Contains("phil"))
        {
            DrawMushroom(pixels, size, fgC32);
        }
        else if (lowerCode.Contains("hardware") || lowerCode.Contains("hank") || lowerCode.Contains("dan") || lowerName.Contains("hardware"))
        {
            DrawTools(pixels, size, fgC32);
        }
        else
        {
            // Stylized NPC Portrait Silhouette (Head & Shoulders)
            DrawCharacterSilhouette(pixels, size, fgC32);
        }

        tex.SetPixels32(pixels);
        tex.Apply(false, false);

        var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        sprite.name = $"PocketShop_ThemedIcon_{shopCode}";
        return sprite;
    }

    private static void DrawGasPump(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillRect(pixels, size, c - 14, c - 18, 20, 32, fg);
        FillRect(pixels, size, c - 11, c + 2, 14, 8, new Color32(30, 40, 55, 220));
        DrawThickLine(pixels, size, c + 6, c + 8, c + 14, c + 4, 3, fg);
        DrawThickLine(pixels, size, c + 14, c + 4, c + 14, c - 12, 3, fg);
        FillRect(pixels, size, c + 11, c - 16, 6, 6, fg);
    }

    private static void DrawTShirt(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillRect(pixels, size, c - 14, c - 16, 28, 24, fg);
        FillRect(pixels, size, c - 22, c - 2, 10, 10, fg);
        FillRect(pixels, size, c + 12, c - 2, 10, 10, fg);
        FillCircle(pixels, size, c, c + 8, 7, new Color32(0, 0, 0, 0));
    }

    private static void DrawCrosshair(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        DrawRing(pixels, size, c, c, 16, 3, fg);
        DrawRing(pixels, size, c, c, 8, 2, fg);
        FillCircle(pixels, size, c, c, 3, fg);
        FillRect(pixels, size, c - 20, c - 1, 8, 3, fg);
        FillRect(pixels, size, c + 12, c - 1, 8, 3, fg);
        FillRect(pixels, size, c - 1, c - 20, 3, 8, fg);
        FillRect(pixels, size, c - 1, c + 12, 3, 8, fg);
    }

    private static void DrawDiamond(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        int r = 18;
        for (int y = c - r; y <= c + r; y++)
        {
            for (int x = c - r; x <= c + r; x++)
            {
                int dx = Mathf.Abs(x - c);
                int dy = Mathf.Abs(y - c);
                if (dx + dy <= r)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = fg;
                    }
                }
            }
        }
    }

    private static void DrawMushroom(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillRect(pixels, size, c - 6, c - 16, 12, 18, fg);
        for (int y = c - 2; y <= c + 18; y++)
        {
            for (int x = c - 22; x <= c + 22; x++)
            {
                float dx = (x - c) / 22f;
                float dy = (y - c + 2) / 16f;
                if (dx * dx + dy * dy <= 1f && y >= c - 2)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = fg;
                    }
                }
            }
        }
    }

    private static void DrawTools(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        DrawThickLine(pixels, size, c - 14, c - 14, c + 14, c + 14, 5, fg);
        DrawThickLine(pixels, size, c - 14, c + 14, c + 14, c - 14, 5, fg);
        FillRect(pixels, size, c + 10, c + 8, 8, 10, fg);
        FillCircle(pixels, size, c - 14, c - 14, 7, fg);
        FillCircle(pixels, size, c - 14, c - 14, 3, new Color32(0, 0, 0, 0));
    }

    private static void DrawCharacterSilhouette(Color32[] pixels, int size, Color32 fg)
    {
        int c = size / 2;
        FillCircle(pixels, size, c, c + 6, 11, fg);
        for (int y = c - 18; y <= c - 1; y++)
        {
            for (int x = c - 20; x <= c + 20; x++)
            {
                float dx = (x - c) / 20f;
                float dy = (y - (c - 18)) / 16f;
                if (dx * dx + (1f - dy) * (1f - dy) <= 1f)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = fg;
                    }
                }
            }
        }
    }

    private static void FillRect(Color32[] pixels, int size, int x, int y, int w, int h, Color32 c)
    {
        for (int py = y; py < y + h; py++)
        {
            for (int px = x; px < x + w; px++)
            {
                if (px >= 0 && px < size && py >= 0 && py < size)
                {
                    pixels[py * size + px] = c;
                }
            }
        }
    }

    private static void FillCircle(Color32[] pixels, int size, int cx, int cy, int r, Color32 c)
    {
        int rSq = r * r;
        for (int y = cy - r; y <= cy + r; y++)
        {
            for (int x = cx - r; x <= cx + r; x++)
            {
                if ((x - cx) * (x - cx) + (y - cy) * (y - cy) <= rSq)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = c;
                    }
                }
            }
        }
    }

    private static void DrawRing(Color32[] pixels, int size, int cx, int cy, int r, int thickness, Color32 c)
    {
        int outerSq = r * r;
        int innerSq = (r - thickness) * (r - thickness);
        for (int y = cy - r; y <= cy + r; y++)
        {
            for (int x = cx - r; x <= cx + r; x++)
            {
                int distSq = (x - cx) * (x - cx) + (y - cy) * (y - cy);
                if (distSq <= outerSq && distSq >= innerSq)
                {
                    if (x >= 0 && x < size && y >= 0 && y < size)
                    {
                        pixels[y * size + x] = c;
                    }
                }
            }
        }
    }

    private static void DrawThickLine(Color32[] pixels, int size, int x0, int y0, int x1, int y1, int thickness, Color32 c)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int steps = Mathf.Max(dx, dy);
        if (steps == 0) { FillCircle(pixels, size, x0, y0, thickness / 2, c); return; }

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            int x = Mathf.RoundToInt(Mathf.Lerp(x0, x1, t));
            int y = Mathf.RoundToInt(Mathf.Lerp(y0, y1, t));
            FillCircle(pixels, size, x, y, thickness / 2, c);
        }
    }

    public static void Reset()
    {
        foreach (var s in _proceduralSprites)
        {
            if (s != null && s.Pointer != IntPtr.Zero && !s.WasCollected)
            {
                try
                {
                    if (s.texture != null && s.texture.Pointer != IntPtr.Zero && !s.texture.WasCollected)
                    {
                        UnityEngine.Object.Destroy(s.texture);
                    }
                    UnityEngine.Object.Destroy(s);
                }
                catch { }
            }
        }
        _proceduralSprites.Clear();
        _cache.Clear();

        if (_circleSprite != null)
        {
            try
            {
                if (_circleSprite.Pointer != IntPtr.Zero && !_circleSprite.WasCollected)
                {
                    if (_circleSprite.texture != null && _circleSprite.texture.Pointer != IntPtr.Zero && !_circleSprite.texture.WasCollected)
                    {
                        UnityEngine.Object.Destroy(_circleSprite.texture);
                    }
                    UnityEngine.Object.Destroy(_circleSprite);
                }
            }
            catch { }
            _circleSprite = null;
        }
    }
}

/// <summary>
/// Deterministic color palette for shop cards matching the reference mockup.
/// </summary>
public static class ShopColorScheme
{
    private static readonly Color[] Palette = new[]
    {
        new Color(0.85f, 0.40f, 0.40f, 1f), // Crimson / Coral (Albert Hoover)
        new Color(0.70f, 0.85f, 0.35f, 1f), // Olive Lime (Dan's Hardware)
        new Color(0.35f, 0.80f, 0.45f, 1f), // Emerald Green (Fungal Phil)
        new Color(0.30f, 0.60f, 0.85f, 1f), // Ocean Blue (Handy Hank)
        new Color(0.45f, 0.50f, 0.85f, 1f), // Slate Indigo (Oscar's Store)
        new Color(0.65f, 0.45f, 0.80f, 1f), // Orchid Purple (Salvador Moreno)
        new Color(0.80f, 0.45f, 0.60f, 1f), // Berry Pink (Shirley Watts)
        new Color(0.88f, 0.58f, 0.28f, 1f), // Orange Ochre (Arms Dealer)
        new Color(0.90f, 0.72f, 0.30f, 1f), // Amber Gold (Playball's Boutique)
        new Color(0.85f, 0.35f, 0.55f, 1f), // Magenta Rose (Thrifty Threads)
        new Color(0.25f, 0.75f, 0.70f, 1f), // Teal (Gas Mart Central)
        new Color(0.30f, 0.70f, 0.80f, 1f), // Cyan Blue (Gas Mart West)
    };

    public static Color ColorFor(string shopCode)
    {
        if (string.IsNullOrEmpty(shopCode)) return Palette[0];
        string lower = shopCode.ToLowerInvariant();
        if (lower.Contains("albert")) return Palette[0];
        if (lower.Contains("dan")) return Palette[1];
        if (lower.Contains("fungal") || lower.Contains("phil")) return Palette[2];
        if (lower.Contains("hank")) return Palette[3];
        if (lower.Contains("oscar")) return Palette[4];
        if (lower.Contains("salvador")) return Palette[5];
        if (lower.Contains("shirley")) return Palette[6];
        if (lower.Contains("arm")) return Palette[7];
        if (lower.Contains("boutique") || lower.Contains("playball")) return Palette[8];
        if (lower.Contains("thread") || lower.Contains("cloth")) return Palette[9];
        if (lower.Contains("central")) return Palette[10];
        if (lower.Contains("west")) return Palette[11];

        uint hash = 2166136261;
        foreach (char c in shopCode) hash = (hash ^ c) * 16777619;
        return Palette[hash % Palette.Length];
    }
}
