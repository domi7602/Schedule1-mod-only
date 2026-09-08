using System;
using HomelessMod.Building;
using HomelessMod.Quests;
using MelonLoader;
using Il2CppScheduleOne;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Shop;
using S1API.Console;
using S1API.Items;
using S1API.Items.Buildable;
using UnityEngine;

namespace HomelessMod.Items;

/// <summary>
/// Component attached to placed world sleeping bags providing Sleep and Pack-Up interactions.
/// </summary>
[RegisterTypeInIl2Cpp]
public class SleepingBagInteractable : MonoBehaviour
{
    public SleepingBagInteractable(IntPtr ptr) : base(ptr) { }

    private float _interactionRange = 3.2f;
    private bool _isHovered = false;
    private float _rmbHoldProgress = 0f;
    private bool _lastHitMatch = false;
    private string _guid = "";
    // Gatekeeper-fix L1: throttle per-frame Physics.Raycast (see OutdoorItemInteractable — TODO central HoverManager)
    private float _nextHoverCheckTime = 0f;
    private const float HoverCheckInterval = 0.15f;

    public string Guid
    {
        get => _guid;
        set => _guid = value;
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(_guid))
        {
            _guid = System.Guid.NewGuid().ToString();
        }
    }

    private void Update()
    {
        try
        {
            bool isTyping = false;
            try { isTyping = S1Mods.Shared.HotkeyManager.IsInputFieldFocused(); } catch { }
            if (isTyping || Cursor.lockState != CursorLockMode.Locked || this.WasCollected || gameObject.WasCollected)
            {
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            var player = PlayerSingleton<PlayerMovement>.Instance;
            if (player == null || player.Pointer == IntPtr.Zero)
            {
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > _interactionRange)
            {
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            // Gatekeeper-fix L1: throttle expensive raycast to ~6-7 Hz; reuse last _isHovered between ticks (TODO central HoverManager)
            bool doHoverCheck = Time.time >= _nextHoverCheckTime;
            if (doHoverCheck) _nextHoverCheckTime = Time.time + HoverCheckInterval;
            if (doHoverCheck)
            {
                // Resolve player camera safely
            Transform? camTransform = null;
            try
            {
                var playerCam = PlayerSingleton<PlayerCamera>.Instance;
                if (playerCam != null && playerCam.Pointer != IntPtr.Zero && playerCam.Camera != null)
                {
                    camTransform = playerCam.Camera.transform;
                }
            }
            catch { }

            if (camTransform == null && Camera.main != null)
            {
                camTransform = Camera.main.transform;
            }

            if (camTransform != null)
            {
                // Check if player is facing the sleeping bag
                Vector3 toBag = (transform.position - camTransform.position).normalized;
                float dot = Vector3.Dot(camTransform.forward, toBag);

                // Raycast check or directional proximity
                bool hitMatch = false;
                Ray ray = new Ray(camTransform.position, camTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, _interactionRange + 1.0f))
                {
                    if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform)))
                    {
                        hitMatch = true;
                    }
                }

                _lastHitMatch = hitMatch;
                _isHovered = hitMatch || (dot > 0.45f && distance <= 2.5f);
            }
            else
            {
                // Fallback purely on proximity
                _lastHitMatch = false;
                _isHovered = distance <= 2.2f;
            }
            } // end doHoverCheck

            if (_isHovered)
            {
                // Focus guard: don't allow RMB-Hold to pack up while typing in phone/backpack or paused.
                if (HomelessInputFocus.IsBlockingInput)
                {
                    _rmbHoldProgress = 0f;
                    return;
                }

                // [E] Key: Sleep — L10: require raycast hitMatch to avoid competing with vanilla interact when hovering overlaps via dot fallback
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!_lastHitMatch)
                    {
                        Mod.Log.Debug("[L10] [E] Sleep ignored — raycast did not hit this bag (dot fallback only), avoiding vanilla interact conflict.");
                        return;
                    }
                    TrySleep();
                }

                // Hold Right Mouse Button (RMB) to Pack Up (Vanilla-style dismantle).
                // F-key path was removed in v0.1.2 — F is the vanilla flashlight and we must not steal it.
                if (Input.GetMouseButton(1))
                {
                    _rmbHoldProgress += Time.deltaTime / 0.40f; // 0.4s hold duration (vanilla-style)
                    if (_rmbHoldProgress >= 1.0f)
                    {
                        PackUp();
                        _rmbHoldProgress = 0f;
                    }
                }
                else
                {
                    _rmbHoldProgress = 0f;
                }
            }
            else
            {
                _rmbHoldProgress = 0f;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"SleepingBagInteractable Update error: {ex.Message}");
        }
    }

    private static GUIStyle? _cachedLabelStyle;
    private static readonly Color BoxBgColor = new(0.08f, 0.09f, 0.11f, 0.92f);

    private static GUIStyle GetLabelStyle(int fontSize)
    {
        if (_cachedLabelStyle == null)
        {
            _cachedLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            _cachedLabelStyle.normal.textColor = Color.white;
        }
        _cachedLabelStyle.fontSize = fontSize;
        return _cachedLabelStyle;
    }

    private void OnGUI()
    {
        if (!_isHovered) return;
        if (!Mod.CurrentConfig.ShowInteractionHud) return;

        // Compact, discreet interaction hint at bottom-center of screen
        float screenW = Screen.width;
        float screenH = Screen.height;
        float scale = Mathf.Clamp(screenH / 900f, 0.80f, 1.25f);
        float boxW = 250f * scale;
        float boxH = 26f * scale;
        int fontSize = Mathf.RoundToInt(12f * scale);
        Rect boxRect = new Rect((screenW - boxW) * 0.5f, screenH - boxH - (90f * scale), boxW, boxH);

        Color prevBg = GUI.backgroundColor;
        Color prevContent = GUI.contentColor;

        GUI.backgroundColor = BoxBgColor;
        GUI.Box(boxRect, "");

        var style = GetLabelStyle(fontSize);

        if (_rmbHoldProgress > 0.05f)
        {
            int filled = Mathf.RoundToInt(_rmbHoldProgress * 10);
            int empty = 10 - filled;
            string bar = new string('█', filled) + new string('░', Mathf.Max(0, empty));
            GUI.Label(boxRect, $"<color=#e67e22>[{bar}]</color> Packing Up...", style);
        }
        else
        {
            GUI.Label(boxRect, "<color=#2ecc71>[E] Sleep</color>  ·  <color=#b0b0b8>Hold RMB</color> <color=#e0e0e0>Pack Up</color>", style);
        }

        GUI.backgroundColor = prevBg;
        GUI.contentColor = prevContent;
    }

    public void TrySleep()
    {
        // Gatekeeper-fix M6: wrap native StartSleep in try/catch so exceptions surface as Warn instead of vanishing in Update's Debug catch
        try
        {
            var timeMgr = NetworkSingleton<TimeManager>.Instance;
            if (timeMgr == null || timeMgr.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("TimeManager not found. Cannot sleep.");
                return;
            }

            if (timeMgr.IsSleepInProgress)
            {
                Mod.Log.Info("Sleep already in progress.");
                return;
            }

            // Night detection uses the vanilla IsNight flag (18:00 - 05:59) so we
            // never have to interpret Schedule I's time encoding ourselves.
            bool isNightTime = timeMgr.IsNight;
            string timeStr = FormatCurrentTime(timeMgr);

            if (Mod.CurrentConfig.AllowAnytimeSleep || isNightTime)
            {
                Mod.Log.Info($"Sleeping in sleeping bag at {timeStr}...");
                timeMgr.StartSleep();
                // Gatekeeper-fix M3: NotifyPlayerSlept is currently fired at sleep START, not wake. If the player aborts
                // sleep (movement/attack), the quest still counts as completed. Ideal fix is to move this to a wake
                // hook (TimeManager.onSleepEnd callback or poll IsSleepInProgress falling edge). Kept at START for now
                // for compatibility; TODO: subscribe to TimeManager wake event when API exposes it and gate completion there.
                HomelessQuestManager.NotifyPlayerSlept();
            }
            else
            {
                Mod.Log.Info($"Too early to sleep (current time {timeStr}). Sleeping is available from 6 PM to 6 AM.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TrySleep failed: {ex.Message}");
        }
    }

    private static string FormatCurrentTime(TimeManager timeMgr)
    {
        try
        {
            // Vanilla formatter for the player-facing 12-hour display
            string formatted = TimeManager.Get12HourTime(timeMgr.CurrentTime);
            if (!string.IsNullOrEmpty(formatted)) return formatted;
        }
        catch
        {
            // fall through to raw fallback
        }

        int currentTime = timeMgr.CurrentTime;
        int hour = currentTime / 100;
        int mins = currentTime % 100;
        return $"{hour}:{mins:D2}";
    }

    public void PackUp()
    {
        Mod.Log.Info("Packing up sleeping bag...");
        string itemId = Mod.CurrentConfig.SleepingBagItemId;

        try
        {
            var itemDef = Registry.GetItem(itemId);
            var inv = PlayerInventory.Instance;
            if (itemDef != null && inv != null && inv.Pointer != IntPtr.Zero && !inv.WasCollected)
            {
                var inst = itemDef.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero && !inst.WasCollected)
                {
                    if (!inv.CanItemFitInInventory(inst, 1))
                    {
                        Mod.Log.Warn($"PackUp blocked — inventory full for '{itemId}'. Free a slot first.");
                        return;
                    }
                    inv.AddItemToInventory(inst);
                    Mod.Log.Info($"Returned 1x '{itemId}' to PlayerInventory.");
                }
                else
                {
                    Mod.Log.Warn($"Could not create item instance for '{itemId}'.");
                    return;
                }
            }
            else
            {
                Mod.Log.Warn($"Cannot pack up '{itemId}' — item definition or PlayerInventory unavailable.");
                return;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"PackUp error: {ex.Message}");
            return;
        }

        StreetPropertyManager.UnregisterStreetItem(gameObject);
        Destroy(gameObject);
    }
}

/// <summary>
/// Registers the Sleeping Bag item definition with S1API and sets up its visual prefabs and components.
/// </summary>
public static class SleepingBagItemFactory
{
    private static bool _isRegistered = false;

    public static void RegisterItem()
    {
        if (_isRegistered) return;

        try
        {
            string itemId = Mod.CurrentConfig.SleepingBagItemId;
            if (Registry.ItemExists(itemId))
            {
                Mod.Log.Info($"Item '{itemId}' already exists in Registry.");
                _isRegistered = true;
                return;
            }

            Mod.Log.Info($"Registering custom buildable item '{itemId}'...");

            // Look for existing buildable base item to inherit GridItem and footprint tiles
            string[] candidateBases = new[] { "bed", "singlebed", "cheapbed", "pot", "moisturepot", "dryingrack", "packagingstation", "chair", "table", "storage" };
            string? baseId = null;
            foreach (var candidate in candidateBases)
            {
                if (Registry.ItemExists(candidate))
                {
                    var def = Registry.GetItem(candidate);
                    if (def != null && def.TryCast<Il2CppScheduleOne.ItemFramework.BuildableItemDefinition>() != null)
                    {
                        baseId = candidate;
                        break;
                    }
                }
            }

            S1API.Items.Buildable.BuildableItemDefinitionBuilder builder;
            if (!string.IsNullOrEmpty(baseId))
            {
                Mod.Log.Info($"Cloning sleeping bag definition from base item '{baseId}' (inherits GridItem & footprint)...");
                builder = S1API.Items.Buildable.BuildableItemCreator.CloneFrom(baseId);
            }
            else
            {
                Mod.Log.Info("Creating sleeping bag builder from scratch...");
                builder = S1API.Items.Buildable.BuildableItemCreator.CreateBuilder();
            }

            // NOTE: "Press [F] to pack up" was removed in v0.1.2 — F is the vanilla flashlight,
            // so we describe the vanilla-correct RMB-hold gesture instead.
            builder.WithBasicInfo(itemId, "Sleeping Bag", "A portable sleeping bag with a built-in mat. Allows sleeping and saving anywhere in the city. Hold Right Mouse Button to pack up.", ItemCategory.Furniture)
                   .WithPricing(150f, 0.5f)
                   .WithIcon(SleepingBagMeshGenerator.GetOrCreateIconSprite())
                   .WithGhostVisual((Transform parent) =>
                   {
                       var ghost = CreateSleepingBagPrefab();
                       ghost.transform.SetParent(parent, false);
                       ghost.name = "SleepingBag_Ghost";

                       // Strip interactable from ghost
                       var comp = ghost.GetComponent<SleepingBagInteractable>();
                       if (comp != null) GameObject.Destroy(comp);

                       return ghost;
                   }, replaceExistingVisual: true);

            builder.Build();
            _isRegistered = true;
            Mod.Log.Info($"Successfully registered item '{itemId}' into game Registry.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to register SleepingBag item: {ex}");
        }
    }

    /// <summary>
    /// Adds the sleeping bag as a purchasable listing in the hardware store so
    /// the player can buy it without using the console. Idempotent.
    /// </summary>
    private static int _injectRetryCount = 0;
    private const int MaxInjectRetries = 10;

    public static void InjectHardwareStoreListing()
    {
        try
        {
            string itemId = Mod.CurrentConfig.SleepingBagItemId;
            var def = Registry.GetItem(itemId);
            if (def == null) return;
            var storable = def.TryCast<Il2CppScheduleOne.ItemFramework.StorableItemDefinition>();
            if (storable == null) return;

            var shops = ShopInterface.AllShops;
            if (shops == null || shops.Count == 0)
            {
                if (_injectRetryCount < MaxInjectRetries)
                {
                    _injectRetryCount++;
                    MelonLoader.MelonCoroutines.Start(DelayedRetry());
                }
                else
                {
                    Mod.Log.Warn("Shop registry not populated yet; sleeping bag store listing skipped after retries.");
                }
                return;
            }
            _injectRetryCount = 0;

            for (int i = 0; i < shops.Count; i++)
            {
                var shop = shops[i];
                if (shop == null || shop.Pointer == IntPtr.Zero) continue;

                string code = (shop.ShopCode ?? string.Empty).ToLowerInvariant();
                string shopName = (shop.ShopName ?? string.Empty).ToLowerInvariant();
                if (!code.Contains("hardware") && !shopName.Contains("hardware") && !code.Contains("dan") && !shopName.Contains("dan") && !code.Contains("hank") && !shopName.Contains("hank")) continue;

                var listings = shop.Listings;
                if (listings == null) continue;

                // Already injected?
                bool alreadyIn = false;
                for (int j = 0; j < listings.Count; j++)
                {
                    var existing = listings[j];
                    if (existing != null && existing.Item != null && existing.Item.ID == itemId)
                    {
                        alreadyIn = true;
                        break;
                    }
                }
                if (alreadyIn) continue;

                var listing = new ShopListing();
                listing.name = "SleepingBagListing";
                listing.Item = storable;
                listing.OverridePrice = true;
                listing.OverriddenPrice = 150f;
                listing.LimitedStock = false;
                listing.DefaultStock = 10;
                listing.CanBeDelivered = false;
                listings.Insert(0, listing);

                Mod.Log.Info($"Injected 'sleepingbag' into hardware store listing ('{shop.ShopName}').");
                continue;
            }

            Mod.Log.Debug("No hardware store found for sleeping bag listing.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to inject sleeping bag store listing: {ex.Message}");
        }
    }

    private static System.Collections.IEnumerator DelayedRetry()
    {
        yield return new UnityEngine.WaitForSeconds(2f);
        InjectHardwareStoreListing();
    }

    public static GameObject CreateSleepingBagPrefab()
    {
        var root = new GameObject("SleepingBag_Root");

        // 3D Mesh Filter & Renderer
        var mf = root.AddComponent<MeshFilter>();
        mf.sharedMesh = SleepingBagMeshGenerator.GetOrCreateMesh();

        var mr = root.AddComponent<MeshRenderer>();
        mr.sharedMaterial = SleepingBagMeshGenerator.GetOrCreateMaterial();

        // Box Collider for raycast interaction & physics
        var box = root.AddComponent<BoxCollider>();
        box.center = new Vector3(0f, 0.18f, 0f);
        box.size = new Vector3(0.95f, 0.36f, 2.05f);

        // Interaction Component
        root.AddComponent<SleepingBagInteractable>();

        return root;
    }

    public static void SetupPlacedSleepingBag(GameObject go)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;

        try
        {
            // 1. Hide original cloned base renderers (e.g. pot/bed meshes)
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].gameObject != go)
                {
                    renderers[i].enabled = false;
                }
            }

            // 2. Attach or configure Sleeping Bag MeshFilter & MeshRenderer
            var mf = go.GetComponent<MeshFilter>() ?? go.AddComponent<MeshFilter>();
            mf.sharedMesh = SleepingBagMeshGenerator.GetOrCreateMesh();

            var mr = go.GetComponent<MeshRenderer>() ?? go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = SleepingBagMeshGenerator.GetOrCreateMaterial();
            mr.enabled = true;

            // 3. Ensure BoxCollider exists
            var box = go.GetComponent<BoxCollider>() ?? go.AddComponent<BoxCollider>();
            box.center = new Vector3(0f, 0.18f, 0f);
            box.size = new Vector3(0.95f, 0.36f, 2.05f);

            // 4. Ensure SleepingBagInteractable is attached
            if (go.GetComponent<SleepingBagInteractable>() == null)
            {
                go.AddComponent<SleepingBagInteractable>();
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SetupPlacedSleepingBag error: {ex.Message}");
        }
    }
}
