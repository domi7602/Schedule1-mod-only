extern alias il2cpp;

using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

namespace SnackVendor.World;

/// <summary>
/// Deposit/extract panel for a placed SnackVendor station (v0.0.5).
/// Opens via the <c>Interacted</c> prefix on our machines (replaces the
/// vanilla pay-UI). Pure IMGUI following the HomelessMod HUD pattern:
/// statically cached <c>GUIStyle</c>s, damped screen scaling, zero per-frame
/// allocations while idle, cursor freed while open, auto-close on distance /
/// scene change / dead station.
///
/// Player inventory ops go through the vanilla <c>PlayerInventory</c> API
/// (<c>GetAmountOfItem</c>/<c>RemoveAmountOfItem</c>/<c>AddItemToInventory</c>)
/// — deposit captures a quality-preserving <c>GetCopy</c> rollback first,
/// extract probes capacity with a 1-unit instance (AutoPack qty² gotcha).
///
/// KNOWN GAP (MP, spike scope 2026-09-16): Deposit/Extract mutate the LOCAL
/// controller stock and persist to the LOCAL sidecar file — they run on every
/// client that opens the panel, while the NPC purchase path only decrements
/// stock host-side (SendPurchase host gate). In multiplayer this desyncs
/// host vs. client stock. Not yet decided whether the fix is host-routing of
/// panel ops or full stock sync; until then: singleplayer-authoritative.
/// </summary>
public static class SnackVendorPanel
{
    private static SnackVendorController? _open;

    public static bool IsOpen => _open != null;
    public static bool IsOpenFor(SnackVendorController controller) => _open == controller;

    // Fault-throttle: Draw() läuft pro Frame — ein persistierender Fehler darf
    // nur EINMAL pro Panel-Open geloggt werden, sonst flutet er den Log.
    private static bool _drawFaultLogged;

    // Frame-guard: when Close() fires (e.g. via SnackVendorPanel.Update E-key),
    // the same frame may still run SnackVendorController.Update which calls Toggle()
    // again and re-opens the panel. We block Toggle() for the remainder of that frame.
    private static int _closedFrame = -1;

    // ----- cached GUI (built once, HomelessMod pattern) -----
    private static GUIStyle? _boxStyle;
    private static GUIStyle? _headerStyle;
    private static GUIStyle? _labelStyle;
    private static GUIStyle? _buttonStyle;
    private static GUIStyle? _subStyle;

    /// <summary>Called from the <c>Interacted</c> prefix. Same station toggles closed.</summary>
    internal static void Toggle(SnackVendorController controller)
    {
        try
        {
            if (_open == controller)
            {
                Close();
                return;
            }
            // Block same-frame re-open: if Close() was just called this frame
            // (e.g. by SnackVendorPanel.Update E-key), don't open again.
            if (Time.frameCount == _closedFrame) return;
            _open = controller;
            _drawFaultLogged = false;
            SetCursorFree(true);
            Mod.Log.Info("SnackVendor panel opened.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("SnackVendorPanel.Toggle", ex);
        }
    }

    /// <summary>Closes the panel and restores the gameplay cursor.</summary>
    internal static void Close()
    {
        if (_open == null) return;
        _open = null;
        _closedFrame = Time.frameCount;
        SetCursorFree(false);
    }

    private static void SetCursorFree(bool free)
    {
        try
        {
            if (free)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (SceneGate.IsInMainScene)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        catch { /* cursor state is cosmetic — never break on it */ }
    }

    /// <summary>Per-frame housekeeping (called from MelonMod.OnUpdate).</summary>
    public static void Update()
    {
        try
        {
            if (_open == null || _open.Pointer == IntPtr.Zero || _open.WasCollected || !_open.gameObject.activeInHierarchy)
            {
                Close();
                return;
            }
            if (!SceneGate.IsInMainScene)
            {
                Close();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                Close();
                return;
            }
            var player = Player.Local;
            if (player == null || player.Pointer == IntPtr.Zero)
            {
                Close();
                return;
            }
            float dist = Vector3.Distance(player.transform.position, _open.transform.position);
            if (dist > Mod.CurrentConfig.PanelRange)
            {
                Close();
            }
        }
        catch
        {
            try { Close(); } catch { }
        }
    }

    /// <summary>IMGUI draw (called from MelonMod.OnGUI). No-op while closed.</summary>
    public static void Draw()
    {
        var station = _open;
        if (station == null) return;
        try
        {
            if (station.Pointer == IntPtr.Zero || station.WasCollected)
            {
                Close();
                return;
            }

            EnsureStyles();
            float scale = Mathf.Clamp(Screen.height / 900f, 0.80f, 1.25f);

            const float W = 420f;
            float rowH = 30f * scale;
            var stock = station.Stock;
            var allowed = SnackVendorController.GetAllowedIngredientDefs();

            int stockRows = Mathf.Min(stock.Count, 8);
            int depositRows = 0;
            for (int i = 0; i < allowed.Count && depositRows < 8; i++)
            {
                if (GetPlayerAmount(allowed[i].ID) > 0) depositRows++;
            }

            float headerH = 34f * scale;
            float sectionH = 26f * scale;
            bool isOutdoor = station.GetComponent<Outdoor.SnackVendorOutdoorInteractable>() != null
                || station.GetComponent("OutdoorItemInteractable") != null;
            float H = headerH + sectionH + stockRows * rowH + sectionH + depositRows * rowH + rowH + (isOutdoor ? rowH : 0f) + 16f * scale;
            float x = (Screen.width - W) * 0.5f;
            float y = Mathf.Max(20f, (Screen.height - H) * 0.5f);
            var rect = new Rect(x, y, W, H);

            GUI.Box(rect, GUIContent.none, _boxStyle);

            var invRect = new Rect(rect.x, rect.y, rect.width, headerH);
            GUI.Label(invRect, "SNACK VENDOR — STOCK", _headerStyle);
            float cy = rect.y + headerH;

            // ----- stock rows (extract) -----
            GUI.Label(new Rect(rect.x + 12f * scale, cy, rect.width - 24f * scale, sectionH), "Lager — herausnehmen:", _subStyle);
            cy += sectionH;
            for (int i = 0; i < stock.Count && i < 8; i++)
            {
                var slot = stock[i];
                string name = SnackVendorController.GetIngredientName(slot.IngredientId);
                GUI.Label(new Rect(rect.x + 12f * scale, cy, 190f * scale, rowH), $"{name}  ×{slot.Quantity}", _labelStyle);

                if (GUI.Button(new Rect(rect.x + 210f * scale, cy + 2f * scale, 70f * scale, rowH - 6f * scale), "Take 1", _buttonStyle))
                {
                    Extract(station, slot.IngredientId, 1);
                }
                if (GUI.Button(new Rect(rect.x + 288f * scale, cy + 2f * scale, 80f * scale, rowH - 6f * scale), "Take All", _buttonStyle))
                {
                    Extract(station, slot.IngredientId, slot.Quantity);
                }
                cy += rowH;
            }

            // ----- player inventory rows (deposit) -----
            GUI.Label(new Rect(rect.x + 12f * scale, cy, rect.width - 24f * scale, sectionH), "Inventar — einlagern:", _subStyle);
            cy += sectionH;
            int shown = 0;
            for (int i = 0; i < allowed.Count && shown < 8; i++)
            {
                var def = allowed[i];
                if (def == null || def.Pointer == IntPtr.Zero) continue;
                int have = GetPlayerAmount(def.ID);
                if (have <= 0) continue;
                shown++;
                string name = SnackVendorController.GetIngredientName(def.ID);
                GUI.Label(new Rect(rect.x + 12f * scale, cy, 190f * scale, rowH), $"{name}  ×{have}", _labelStyle);

                if (GUI.Button(new Rect(rect.x + 210f * scale, cy + 2f * scale, 50f * scale, rowH - 6f * scale), "+1", _buttonStyle))
                {
                    Deposit(station, def, 1);
                }
                if (GUI.Button(new Rect(rect.x + 266f * scale, cy + 2f * scale, 50f * scale, rowH - 6f * scale), "+5", _buttonStyle))
                {
                    Deposit(station, def, 5);
                }
                if (GUI.Button(new Rect(rect.x + 322f * scale, cy + 2f * scale, 64f * scale, rowH - 6f * scale), "All", _buttonStyle))
                {
                    Deposit(station, def, have);
                }
                cy += rowH;
            }
            if (shown == 0)
            {
                GUI.Label(new Rect(rect.x + 12f * scale, cy, rect.width - 24f * scale, rowH), "Keine passenden Zutaten im Inventar.", _subStyle);
                cy += rowH;
            }

            if (isOutdoor)
            {
                if (GUI.Button(new Rect(rect.x + 12f * scale, cy + 2f * scale, rect.width - 24f * scale, rowH - 4f * scale), "📦 Pack Up Station", _buttonStyle))
                {
                    Close();
                    station.RefundStockAndDismantle();
                    return;
                }
                cy += rowH;
            }

            if (GUI.Button(new Rect(rect.x + 12f * scale, cy + 4f * scale, rect.width - 24f * scale, rowH - 4f * scale), "Close  [ESC]", _buttonStyle))
            {
                Close();
            }
        }
        catch (Exception ex)
        {
            // Per-Frame-Draw: ohne Throttle würde ein persistierender Fehler
            // den Log jede Frame fluten — einmal pro Open genügt.
            if (!_drawFaultLogged)
            {
                _drawFaultLogged = true;
                Mod.Log.Warn("SnackVendorPanel.Draw", ex);
            }
            try { Close(); } catch { }
        }
    }

    // ----- inventory operations -----

    private static PlayerInventory? GetInv()
    {
        try
        {
            var inv = PlayerSingleton<PlayerInventory>.Instance;
            if (inv == null || inv.Pointer == IntPtr.Zero || inv.WasCollected) return null;
            return inv;
        }
        catch { return null; }
    }

    private static int GetPlayerAmount(string itemId)
    {
        try
        {
            var inv = GetInv();
            if (inv == null || string.IsNullOrEmpty(itemId)) return 0;
            long amount = inv.GetAmountOfItem(itemId);
            return (int)Mathf.Clamp(amount, 0, int.MaxValue);
        }
        catch { return 0; }
    }

    private static void Deposit(SnackVendorController station, StorableItemDefinition def, int want)
    {
        try
        {
            var inv = GetInv();
            if (inv == null || def == null || def.Pointer == IntPtr.Zero) return;
            string id = def.ID ?? string.Empty;
            if (id.Length == 0) return;

            int have = GetPlayerAmount(id);
            int n = Mathf.Clamp(want, 0, have);
            if (n <= 0) return;

            // Capture a quality-preserving copy BEFORE removal so a failed
            // deposit can hand the exact item back (BackpackMod v1.2.0 lesson:
            // GetCopy instead of GetDefaultInstance keeps Quality/Packaging).
            ItemInstance? rollbackCopy = null;
            try
            {
                var slots = inv.GetAllInventorySlots();
                for (int i = 0; i < slots.Count; i++)
                {
                    var s = slots[i];
                    if (s == null || s.Pointer == IntPtr.Zero) continue;
                    var inst = s.ItemInstance;
                    if (inst == null || inst.Pointer == IntPtr.Zero) continue;
                    var d = inst.Definition;
                    if (d == null || d.Pointer == IntPtr.Zero) continue;
                    if (!string.Equals(d.ID, id, StringComparison.OrdinalIgnoreCase)) continue;
                    rollbackCopy = inst.GetCopy(n);
                    break;
                }
            }
            catch { /* rollback stays null — fresh-instance fallback below */ }

            inv.RemoveAmountOfItem(id, (uint)n);
            if (station.TryDepositIngredient(id, n))
            {
                station.PersistSlotsToDisk();
                Mod.Log.Info($"[SnackVendor] deposited {n}× '{id}'.");
                return;
            }

            // Station full → hand the items straight back.
            Mod.Log.Warn("Station full — deposit rolled back.");
            try
            {
                inv.AddItemToInventory(rollbackCopy ?? def.GetDefaultInstance(n));
            }
            catch (Exception ex)
            {
                Mod.Log.Error("Deposit rollback failed — items may be lost", ex);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("SnackVendorPanel.Deposit", ex);
        }
    }

    private static void Extract(SnackVendorController station, string ingredientId, int want)
    {
        try
        {
            var inv = GetInv();
            if (inv == null) return;
            var def = SnackVendorController.GetIngredientDef(ingredientId);
            if (def == null || def.Pointer == IntPtr.Zero) return;

            int stockQty = station.GetStockQuantity(ingredientId);
            int n = Mathf.Clamp(want, 0, stockQty);
            if (n <= 0) return;

            // Capacity probe with a 1-unit instance — CanItemFitInInventory
            // tests qty² with larger quantities (AutoPack gotcha).
            try
            {
                var probe = def.GetDefaultInstance(1);
                if (probe != null && probe.Pointer != IntPtr.Zero && !inv.CanItemFitInInventory(probe, 1))
                {
                    Mod.Log.Warn("Inventory full — extract refused (stock untouched).");
                    return;
                }
            }
            catch { /* probe failed — attempt anyway, guarded below */ }

            if (!station.TryExtractIngredient(ingredientId, n)) return;
            try
            {
                inv.AddItemToInventory(def.GetDefaultInstance(n));
                station.PersistSlotsToDisk();
                Mod.Log.Info($"[SnackVendor] extracted {n}× '{ingredientId}'.");
            }
            catch (Exception ex)
            {
                station.TryDepositIngredient(ingredientId, n); // refund — stock must not eat the item
                Mod.Log.Error("Extract AddItem failed — stock refunded", ex);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("SnackVendorPanel.Extract", ex);
        }
    }

    // ----- style cache -----

    private static void EnsureStyles()
    {
        if (_boxStyle != null && _headerStyle != null && _labelStyle != null && _buttonStyle != null && _subStyle != null) return;
        _boxStyle = new GUIStyle(GUI.skin.box);
        _headerStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleLeft };
        _labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft };
        _buttonStyle = new GUIStyle(GUI.skin.button);
        _subStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = new Color(0.75f, 0.85f, 0.95f) },
        };
    }
}
