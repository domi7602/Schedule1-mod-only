using System;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace TownHallInterior;

/// <summary>
/// Integrates the Town Hall into Ray's Real Estate Office whiteboard ("Businesses for sale"),
/// cloning an existing business card, positioning it in the top row, and syncing visibility with ownership.
/// </summary>
public static class TownHallWhiteboardIntegration
{
    private static GameObject? _townHallCard;
    private static Material? _clonedPhotoMaterial;
    private static Transform? _laundromatCard;
    private static Vector3 _laundromatOriginalLocalPos;
    private static bool _laundromatPosSaved;
    private static Transform? _postOfficeCard;
    private static Vector3 _postOfficeOriginalLocalPos;
    private static bool _postOfficePosSaved;
    private static Vector3? _whiteboardPosition;

    public static Vector3? WhiteboardPosition => _whiteboardPosition;
    public static bool IsCardInstalled => _townHallCard != null && (UnityEngine.Object)_townHallCard != null;

    public static bool TryInstallListing(bool isOwned)
    {
        try
        {
            var allTexts = UnityEngine.Object.FindObjectsOfType<TMP_Text>();
            if (allTexts == null || allTexts.Length == 0) return false;

            TMP_Text? header = null;
            foreach (var t in allTexts)
            {
                if (t != null && !string.IsNullOrEmpty(t.text) &&
                    t.text.IndexOf("Businesses for sale", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    header = t;
                    break;
                }
            }

            if (header == null) return false;

            Vector3 boardPos = header.transform.position;
            _whiteboardPosition = boardPos;

            // Only consider text labels within 2.0m of the whiteboard header to prevent
            // matching distant world buildings/signs across town!
            TMP_Text? postOfficeText = null;
            TMP_Text? laundromatText = null;
            TMP_Text? hylandMedicalText = null;
            TMP_Text? carWashText = null;
            TMP_Text? tacoTicklersText = null;

            foreach (var t in allTexts)
            {
                if (t == null || string.IsNullOrEmpty(t.text)) continue;
                if (Vector3.Distance(t.transform.position, boardPos) > 2.0f) continue;

                string txt = t.text.Trim();
                if (postOfficeText == null && txt.Equals("Post Office", StringComparison.OrdinalIgnoreCase))
                {
                    postOfficeText = t;
                }
                else if (laundromatText == null && txt.Equals("Laundromat", StringComparison.OrdinalIgnoreCase))
                {
                    laundromatText = t;
                }
                else if (hylandMedicalText == null && txt.Equals("Hyland Medical", StringComparison.OrdinalIgnoreCase))
                {
                    hylandMedicalText = t;
                }
                else if (carWashText == null && txt.Equals("Car Wash", StringComparison.OrdinalIgnoreCase))
                {
                    carWashText = t;
                }
                else if (tacoTicklersText == null && txt.Equals("Taco Ticklers", StringComparison.OrdinalIgnoreCase))
                {
                    tacoTicklersText = t;
                }
            }

            // Resolve card roots directly from the whiteboard TextMeshPro label hierarchy
            Transform? postOfficeCard = postOfficeText != null ? FindCardRoot(postOfficeText.transform) : null;
            Transform? laundromatCard = laundromatText != null ? FindCardRoot(laundromatText.transform) : null;
            Transform? hylandMedicalCard = hylandMedicalText != null ? FindCardRoot(hylandMedicalText.transform) : null;
            Transform? carWashCard = carWashText != null ? FindCardRoot(carWashText.transform) : null;
            Transform? tacoTicklersCard = tacoTicklersText != null ? FindCardRoot(tacoTicklersText.transform) : null;

            // Prefer Hyland Medical as the donor card: Hyland Medical is the standard 3-column card
            // sitting directly below our slot in Column 3. Its width, mesh, and photo dimensions
            // define the exact fixed size for a 3-column layout!
            Transform? donorCard = hylandMedicalCard ?? tacoTicklersCard ?? carWashCard ?? postOfficeCard ?? laundromatCard;
            if (donorCard == null)
            {
                MelonLogger.Warning("TownHallWhiteboardIntegration: Businesses for sale header found, but no donor card identified on whiteboard.");
                return false;
            }

            // Align the top row with the bottom row to form a clean, symmetrical 3x2 grid:
            // Top Row:    [Laundromat]    [Post Office]    [Town Hall]
            // Bottom Row: [Car Wash]      [Taco Ticklers]  [Hyland Medical]
            //
            // In vanilla, the top row only had 2 cards (Laundromat & Post Office) centered across the board.
            // By aligning:
            // Column 1: Laundromat directly above Car Wash
            // Column 2: Post Office directly above Taco Ticklers
            // Column 3: Town Hall directly above Hyland Medical
            // There is zero overlap, ample spacing between all cards, and no flickering/z-fighting!

            if (laundromatCard != null && carWashCard != null && laundromatCard.parent == donorCard.parent)
            {
                if (!_laundromatPosSaved || _laundromatCard != laundromatCard)
                {
                    _laundromatCard = laundromatCard;
                    _laundromatOriginalLocalPos = laundromatCard.localPosition;
                    _laundromatPosSaved = true;
                }
                float carWashX = donorCard.parent.InverseTransformPoint(carWashCard.position).x;
                var lp = laundromatCard.localPosition;
                laundromatCard.localPosition = new Vector3(carWashX, lp.y, lp.z);
            }

            if (postOfficeCard != null && tacoTicklersCard != null && postOfficeCard.parent == donorCard.parent)
            {
                if (!_postOfficePosSaved || _postOfficeCard != postOfficeCard)
                {
                    _postOfficeCard = postOfficeCard;
                    _postOfficeOriginalLocalPos = postOfficeCard.localPosition;
                    _postOfficePosSaved = true;
                }
                float tacoX = donorCard.parent.InverseTransformPoint(tacoTicklersCard.position).x;
                var pp = postOfficeCard.localPosition;
                postOfficeCard.localPosition = new Vector3(tacoX, pp.y, pp.z);
            }

            // Determine Row 1 (top row) Y coordinate from Post Office or Laundromat
            float row1Y = postOfficeCard != null ? postOfficeCard.localPosition.y :
                          laundromatCard != null ? laundromatCard.localPosition.y :
                          (donorCard.localPosition.y + 0.35f);

            // Column 3 X coordinate directly from Hyland Medical
            float targetX;
            if (hylandMedicalCard != null)
            {
                targetX = donorCard.parent.InverseTransformPoint(hylandMedicalCard.position).x;
            }
            else if (tacoTicklersCard != null && carWashCard != null)
            {
                float tX = donorCard.parent.InverseTransformPoint(tacoTicklersCard.position).x;
                float cX = donorCard.parent.InverseTransformPoint(carWashCard.position).x;
                targetX = tX + (tX - cX);
            }
            else
            {
                targetX = donorCard.localPosition.x;
            }

            Vector3 targetLocalPos = new Vector3(
                targetX,
                row1Y,
                donorCard.localPosition.z);

            // Clean up any previously instantiated clone to ensure a fresh clone with Hyland Medical's exact dimensions
            var existing = donorCard.parent.Find("BusinessCard_TownHall");
            if (existing != null)
            {
                try { UnityEngine.Object.Destroy(existing.gameObject); } catch { }
                _townHallCard = null;
            }

            // Instantiate cloned card from donorCard (Hyland Medical)
            _townHallCard = UnityEngine.Object.Instantiate(donorCard.gameObject, donorCard.parent, false);
            _townHallCard.name = "BusinessCard_TownHall";

            _townHallCard.transform.localPosition = targetLocalPos;
            _townHallCard.transform.localRotation = donorCard.localRotation;
            _townHallCard.transform.localScale = donorCard.localScale;

            ApplyListingVisuals(_townHallCard, donorCard);

            _townHallCard.SetActive(!isOwned);
            MelonLogger.Msg($"TownHallWhiteboardIntegration: Successfully aligned fixed-size 3x2 grid on Real Estate whiteboard (Town Hall at local {targetLocalPos}, active={!isOwned}).");
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallWhiteboardIntegration failed: {ex.Message}");
            return false;
        }
    }

    private static void ApplyListingVisuals(GameObject card, Transform? donorCard)
    {
        // 1. Update TextMeshPro labels
        TMP_Text? titleLabel = null;
        TMP_Text? priceLabel = null;

        foreach (var label in card.GetComponentsInChildren<TMP_Text>(true))
        {
            if (label == null || string.IsNullOrEmpty(label.text)) continue;

            string txt = label.text.Trim();
            if (IsKnownListingName(txt))
            {
                label.text = "Town Hall";
                titleLabel = label;
            }
            else if (txt.StartsWith("$"))
            {
                label.text = "$250,000";
                priceLabel = label;
            }
        }

        // Adjust title label position to sit comfortably down in the white text area
        if (titleLabel != null && priceLabel != null)
        {
            var titlePos = titleLabel.transform.localPosition;
            var pricePos = priceLabel.transform.localPosition;

            float refY = titlePos.y;
            if (donorCard != null)
            {
                foreach (var dt in donorCard.GetComponentsInChildren<TMP_Text>(true))
                {
                    if (dt != null && IsKnownListingName(dt.text.Trim()))
                    {
                        refY = dt.transform.localPosition.y;
                        break;
                    }
                }
            }

            // Ensure title sits down towards price, clear of the top photo box
            float targetY = Mathf.Min(titlePos.y, refY);
            if (targetY > pricePos.y + 0.05f)
            {
                targetY -= 0.02f;
            }

            titleLabel.transform.localPosition = new Vector3(titlePos.x, targetY, titlePos.z);
        }

        // 2. Update preview image ONLY on the photo element (preserving white card background!)
        var sprite = TownHallIcon.Get();
        if (sprite == null) return;

        // Restore donor materials on all renderers first (so card background is pure white)
        if (donorCard != null)
        {
            var donorRenderers = donorCard.GetComponentsInChildren<Renderer>(true);
            var cardRenderers = card.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < Math.Min(donorRenderers.Length, cardRenderers.Length); i++)
            {
                if (donorRenderers[i] != null && cardRenderers[i] != null &&
                    cardRenderers[i].GetComponent<TMP_Text>() == null)
                {
                    try
                    {
                        cardRenderers[i].material = donorRenderers[i].material;
                    }
                    catch { }
                }
            }

            var donorImages = donorCard.GetComponentsInChildren<Image>(true);
            var cardImages = card.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < Math.Min(donorImages.Length, cardImages.Length); i++)
            {
                if (donorImages[i] != null && cardImages[i] != null)
                {
                    cardImages[i].sprite = donorImages[i].sprite;
                    cardImages[i].color = donorImages[i].color;
                }
            }
        }

        // Find the specific photo element to apply the Town Hall sprite
        ApplyPhotoSprite(card, sprite);
    }

    private static void ApplyPhotoSprite(GameObject card, Sprite sprite)
    {
        // 1. 3D MeshRenderer / Renderer (the primary renderer for whiteboard card photos)
        var renderers = card.GetComponentsInChildren<Renderer>(true);
        Renderer? bestRenderer = null;
        float highestRendY = float.MinValue;

        foreach (var rend in renderers)
        {
            if (rend == null || rend.transform == card.transform) continue;
            if (rend is SpriteRenderer) continue;
            if (rend.GetComponent<TMP_Text>() != null) continue;

            string name = rend.name.ToLowerInvariant();
            if (name.Contains("background") || name.Contains("bg") || name.Contains("card") || name.Contains("frame") || name.Contains("paper")) continue;

            float y = rend.transform.localPosition.y;
            if (y > highestRendY)
            {
                highestRendY = y;
                bestRenderer = rend;
            }
        }

        if (bestRenderer != null)
        {
            try
            {
                if (_clonedPhotoMaterial != null)
                {
                    try { UnityEngine.Object.Destroy(_clonedPhotoMaterial); } catch { }
                    _clonedPhotoMaterial = null;
                }
                var mat = new Material(bestRenderer.material);
                mat.mainTexture = sprite.texture;
                bestRenderer.material = mat;
                _clonedPhotoMaterial = mat;
                MelonLogger.Msg($"TownHallWhiteboardIntegration: Successfully applied Town Hall photo to photo renderer '{bestRenderer.name}'.");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"TownHallWhiteboardIntegration: Failed to set texture on photo renderer: {ex.Message}");
            }
        }

        // 2. UI Image components (photo must be at least 35x35 px, skipping small icons like car/person!)
        var images = card.GetComponentsInChildren<Image>(true);
        Image? bestImage = null;
        float highestImageY = float.MinValue;

        foreach (var img in images)
        {
            if (img == null || img.transform == card.transform) continue;
            string name = img.name.ToLowerInvariant();
            if (name.Contains("background") || name.Contains("bg") || name.Contains("card") || name.Contains("frame")) continue;

            var rect = img.rectTransform != null ? img.rectTransform.rect : default;
            if (rect.width < 35f || rect.height < 35f) continue;

            float y = img.transform.localPosition.y;
            if (y > highestImageY)
            {
                highestImageY = y;
                bestImage = img;
            }
        }

        if (bestImage != null)
        {
            bestImage.sprite = sprite;
            bestImage.color = Color.white;
        }

        // 3. UI RawImage components (photo must be at least 35x35 px)
        var rawImages = card.GetComponentsInChildren<RawImage>(true);
        RawImage? bestRaw = null;
        float highestRawY = float.MinValue;

        foreach (var raw in rawImages)
        {
            if (raw == null || raw.transform == card.transform) continue;
            string name = raw.name.ToLowerInvariant();
            if (name.Contains("background") || name.Contains("bg") || name.Contains("card") || name.Contains("frame")) continue;

            var rect = raw.rectTransform != null ? raw.rectTransform.rect : default;
            if (rect.width < 35f || rect.height < 35f) continue;

            float y = raw.transform.localPosition.y;
            if (y > highestRawY)
            {
                highestRawY = y;
                bestRaw = raw;
            }
        }

        if (bestRaw != null)
        {
            bestRaw.texture = sprite.texture;
            bestRaw.color = Color.white;
        }

        // 4. Fallback: SpriteRenderer
        var spriteRenderers = card.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var sr in spriteRenderers)
        {
            if (sr == null || sr.transform == card.transform) continue;
            sr.sprite = sprite;
            sr.color = Color.white;
        }
    }

    public static void SyncOwnership(bool isOwned)
    {
        if (_townHallCard != null && (UnityEngine.Object)_townHallCard != null)
        {
            _townHallCard.SetActive(!isOwned);
        }
    }

    public static void Teardown()
    {
        if (_laundromatPosSaved && _laundromatCard != null && (UnityEngine.Object)_laundromatCard != null)
        {
            try { _laundromatCard.localPosition = _laundromatOriginalLocalPos; } catch { }
        }
        _laundromatCard = null;
        _laundromatPosSaved = false;

        if (_postOfficePosSaved && _postOfficeCard != null && (UnityEngine.Object)_postOfficeCard != null)
        {
            try { _postOfficeCard.localPosition = _postOfficeOriginalLocalPos; } catch { }
        }
        _postOfficeCard = null;
        _postOfficePosSaved = false;

        if (_clonedPhotoMaterial != null)
        {
            try { UnityEngine.Object.Destroy(_clonedPhotoMaterial); } catch { }
            _clonedPhotoMaterial = null;
        }

        if (_townHallCard != null)
        {
            try { UnityEngine.Object.Destroy(_townHallCard); } catch { }
            _townHallCard = null;
        }
        _whiteboardPosition = null;
    }

    private static Transform? FindCardRoot(Transform textTransform)
    {
        Transform current = textTransform;
        Transform? bestCandidate = null;

        while (current != null && current.parent != null)
        {
            var texts = current.GetComponentsInChildren<TMP_Text>(true);
            bool hasPrice = false;
            int businessNameCount = 0;

            foreach (var t in texts)
            {
                if (t == null || string.IsNullOrEmpty(t.text)) continue;
                string s = t.text.Trim();
                if (s.Contains("$")) hasPrice = true;
                if (IsKnownListingName(s)) businessNameCount++;
            }

            if (businessNameCount == 1 && hasPrice)
            {
                bestCandidate = current;
            }
            else if (businessNameCount > 1)
            {
                break;
            }

            current = current.parent;
        }

        return bestCandidate ?? (textTransform.parent != null ? textTransform.parent : textTransform);
    }

    private static bool IsKnownListingName(string name)
    {
        return name.Equals("Post Office", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Laundromat", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Car Wash", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Taco Ticklers", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Hyland Medical", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Storage Unit", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Bungalow", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Barn", StringComparison.OrdinalIgnoreCase) ||
               name.Equals("Docks Warehouse", StringComparison.OrdinalIgnoreCase);
    }
}
