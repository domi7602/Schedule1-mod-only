using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace StorageScanner
{
    public sealed class StorageScannerApp : PhoneApp, IStorageScannerView
    {
        private static StorageScannerApp? _active;
        private static bool _updateSubscribed;

        private readonly GameStorageSource _source = new GameStorageSource();
        private StorageScannerController? _controller;
        private GameObject? _mainBG;
        private RectTransform? _chipContent;
        private RectTransform? _listContent;
        private InputField? _searchInput;
        private Text? _statusText;
        private Text? _filterText;
        private Text? _sortText;
        private IReadOnlyList<StockRow>? _lastRows;
        private string _lastPropertySignature = string.Empty;
        private string? _lastSelectedPropertyId;
        private bool _showingDetails;
        private float _lastScanTime;

        protected override string AppName => Constants.AppName;
        protected override string AppTitle => Constants.AppTitle;
        protected override string IconLabel => "Storage";
        protected override string IconFileName => Constants.IconFileName;
        protected override EOrientation Orientation => EOrientation.Vertical;

        protected override void OnCreated()
        {
            base.OnCreated();
            _active = this;
            _controller = new StorageScannerController(_source, this);

            if (!_updateSubscribed)
            {
                MelonEvents.OnUpdate.Unsubscribe(DispatchUpdate);
                MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
                _updateSubscribed = true;
            }

            MelonLogger.Msg("[StorageScanner] Registered with S1API PhoneApp system.");
        }

        internal static void TearDownForSceneUnload() => _active = null;

        internal static void ResetForNewSave()
        {
            StorageScannerApp? app = _active;
            if (app == null) return;
            app._source.ResetForNewSave();
            app._controller?.ResetForNewSave();
            app._lastRows = null;
            app._lastPropertySignature = string.Empty;
        }

        private static void DispatchUpdate()
        {
            StorageScannerApp? app = _active;
            if (app == null) return;
            app.UpdateApp();
        }

        protected override void OnPhoneClosed()
        {
            base.OnPhoneClosed();
            if (NetworkGuard.IsAlive(_mainBG)) _mainBG!.SetActive(false);
        }

        private void UpdateApp()
        {
            if (!NetworkGuard.IsAlive(_mainBG) || _controller == null) return;

            bool open = IsOpen();
            if (_mainBG!.activeSelf != open)
            {
                _mainBG.SetActive(open);
                if (open)
                {
                    _lastScanTime = Time.unscaledTime;
                    _controller.Open();
                }
            }

            if (!open) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_showingDetails) _controller.CloseItemDetails();
                else CloseApp();
                return;
            }

            if (Time.unscaledTime - _lastScanTime >= Constants.AutoRefreshIntervalSeconds)
            {
                _lastScanTime = Time.unscaledTime;
                _controller.Refresh();
            }
        }

        protected override void OnCreatedUI(GameObject container)
        {
            RectTransform? rect = container.GetComponent<RectTransform>();
            if (rect != null) UITheme.InitializeForDashboard(rect);

            _mainBG = UIFactory.Panel("StorageScanner_MainBG", container.transform, GamePalette.Bg, null, null, true);
            _mainBG.SetActive(false);
            VerticalLayoutGroup root = _mainBG.AddComponent<VerticalLayoutGroup>();
            root.childControlWidth = true;
            root.childControlHeight = true;
            root.childForceExpandWidth = true;
            root.childForceExpandHeight = false;
            root.spacing = UITheme.Dp(6f);
            root.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f),
                (int)UITheme.Dp(8f), (int)UITheme.Dp(8f));

            BuildHeader(_mainBG.transform);
            BuildChipScroller(_mainBG.transform);
            BuildSearchField(_mainBG.transform);
            BuildFilterLine(_mainBG.transform);
            BuildList(_mainBG.transform);
            BuildStatus(_mainBG.transform);

            StorageScannerInputFocus focus = container.AddComponent<StorageScannerInputFocus>();
            focus.searchInput = _searchInput;
        }

        private void BuildHeader(Transform parent)
        {
            GameObject header = new GameObject("Header");
            header.transform.SetParent(parent, false);
            header.AddComponent<RectTransform>();
            LayoutElement height = header.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(34f);
            height.minHeight = UITheme.Dp(34f);

            HorizontalLayoutGroup layout = header.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = UITheme.Dp(6f);
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            Text title = UIFactory.Text("Title", "STORAGE SCANNER", header.transform,
                UITheme.Sp(15f), TextAnchor.MiddleLeft, FontStyle.Bold);
            title.color = GamePalette.TextPrimary;
            title.raycastTarget = false;
            title.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;

            (GameObject, Button, Text) sort = UIFactory.RoundedButtonWithLabel("Sort", "SORT", header.transform,
                GamePalette.CardAlt, UITheme.Dp(72f), UITheme.Dp(30f), UITheme.Sp(11f), GamePalette.TextPrimary);
            _sortText = sort.Item3;
            EventHelper.AddListener(() => _controller?.CycleSortMode(), sort.Item2.onClick);

            (GameObject, Button, Text) refresh = UIFactory.RoundedButtonWithLabel("Refresh", "REFRESH", header.transform,
                GamePalette.CardAlt, UITheme.Dp(76f), UITheme.Dp(30f), UITheme.Sp(11f), GamePalette.TextPrimary);
            EventHelper.AddListener(() =>
            {
                _lastScanTime = Time.unscaledTime;
                _controller?.Refresh();
            }, refresh.Item2.onClick);
        }

        private void BuildChipScroller(Transform parent)
        {
            GameObject scroller = new GameObject("PropertyChips");
            scroller.transform.SetParent(parent, false);
            scroller.AddComponent<RectTransform>();
            LayoutElement height = scroller.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(30f);
            height.minHeight = UITheme.Dp(30f);

            ScrollRect scroll = scroller.AddComponent<ScrollRect>();
            scroll.horizontal = true;
            scroll.vertical = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;

            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scroller.transform, false);
            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            viewport.AddComponent<RectMask2D>();
            scroll.viewport = viewportRect;

            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            _chipContent = content.AddComponent<RectTransform>();
            _chipContent.anchorMin = new Vector2(0f, 0f);
            _chipContent.anchorMax = new Vector2(0f, 1f);
            _chipContent.pivot = new Vector2(0f, 0.5f);

            HorizontalLayoutGroup chips = content.AddComponent<HorizontalLayoutGroup>();
            chips.spacing = UITheme.Dp(6f);
            chips.childAlignment = TextAnchor.MiddleLeft;
            chips.childControlWidth = true;
            chips.childControlHeight = true;
            chips.childForceExpandWidth = false;
            chips.childForceExpandHeight = false;
            content.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.content = _chipContent;
        }

        private void BuildSearchField(Transform parent)
        {
            GameObject search = new GameObject("Search");
            search.transform.SetParent(parent, false);
            search.AddComponent<RectTransform>();
            LayoutElement height = search.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(32f);
            height.minHeight = UITheme.Dp(32f);

            InputField input = search.AddComponent<InputField>();
            GameObject background = UIFactory.Panel("Background", search.transform, GamePalette.Card, null, null, false);
            Image backgroundImage = background.GetComponent<Image>();
            backgroundImage.sprite = UISprites.Rounded(8f, 32);
            backgroundImage.type = Image.Type.Sliced;
            RectTransform backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            Text text = UIFactory.Text("Text", string.Empty, search.transform, UITheme.Sp(13f), TextAnchor.MiddleLeft, FontStyle.Normal);
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = new Vector2(UITheme.Dp(10f), 0f);
            text.rectTransform.offsetMax = new Vector2(-UITheme.Dp(10f), 0f);
            text.supportRichText = false;
            text.raycastTarget = false;

            Text placeholder = UIFactory.Text("Placeholder", "Search item, property or container...", search.transform,
                UITheme.Sp(12f), TextAnchor.MiddleLeft, FontStyle.Normal);
            placeholder.rectTransform.anchorMin = Vector2.zero;
            placeholder.rectTransform.anchorMax = Vector2.one;
            placeholder.rectTransform.offsetMin = new Vector2(UITheme.Dp(10f), 0f);
            placeholder.rectTransform.offsetMax = new Vector2(-UITheme.Dp(10f), 0f);
            placeholder.color = GamePalette.TextDim;
            placeholder.raycastTarget = false;

            input.targetGraphic = backgroundImage;
            input.textComponent = text;
            input.placeholder = placeholder;
            input.caretWidth = 2;
            input.caretColor = Color.white;
            _searchInput = input;
            EventHelper.AddListener<string>(value => _controller?.SetSearch(value), input.onValueChanged);
        }

        private void BuildFilterLine(Transform parent)
        {
            _filterText = UIFactory.Text("FilterLine", "ALL PROPERTIES", parent, UITheme.Sp(10f),
                TextAnchor.MiddleLeft, FontStyle.Normal);
            _filterText.color = GamePalette.TextMuted;
            _filterText.raycastTarget = false;
            LayoutElement height = _filterText.gameObject.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(18f);
            height.minHeight = UITheme.Dp(18f);
        }

        private void BuildList(Transform parent)
        {
            RectTransform content = UIFactory.ScrollableVerticalList("Items", parent, out ScrollRect scroll);
            scroll.movementType = ScrollRect.MovementType.Clamped;
            _listContent = content;
            LayoutElement element = scroll.gameObject.AddComponent<LayoutElement>();
            element.flexibleHeight = 1f;
            element.minHeight = UITheme.Dp(80f);

            VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
        }

        private void BuildStatus(Transform parent)
        {
            _statusText = UIFactory.Text("Status", string.Empty, parent, UITheme.Sp(10f),
                TextAnchor.MiddleLeft, FontStyle.Normal);
            _statusText.color = GamePalette.TextMuted;
            _statusText.raycastTarget = false;
            LayoutElement height = _statusText.gameObject.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(30f);
            height.minHeight = UITheme.Dp(30f);
        }

        public void RenderInventory(StorageSnapshot snapshot, IReadOnlyList<StockRow> rows,
            string? selectedPropertyId, StockSortMode sortMode)
        {
            if (!NetworkGuard.IsAlive(_mainBG) || _listContent == null || _chipContent == null) return;

            _showingDetails = false;
            string signature = BuildPropertySignature();
            if (!string.Equals(signature, _lastPropertySignature, StringComparison.Ordinal) ||
                !string.Equals(selectedPropertyId, _lastSelectedPropertyId, StringComparison.Ordinal))
            {
                RebuildChips(selectedPropertyId);
                _lastPropertySignature = signature;
                _lastSelectedPropertyId = selectedPropertyId;
            }

            if (!StockAggregator.RowsEqual(_lastRows, rows))
            {
                RebuildRows(rows);
                _lastRows = rows.ToArray();
            }

            if (_filterText != null)
                _filterText.text = selectedPropertyId == null ? "ALL PROPERTIES" : PropertyNameFor(selectedPropertyId);
            if (_sortText != null) _sortText.text = SortLabel(sortMode);
            UpdateStatus(snapshot);
        }

        public void RenderItemDetails(StorageSnapshot snapshot, ItemDetails details, string? selectedPropertyId)
        {
            if (!NetworkGuard.IsAlive(_mainBG) || _listContent == null) return;

            _showingDetails = true;
            _lastRows = null;
            ClearChildren(_listContent);

            (GameObject, Button, Text) back = UIFactory.RoundedButtonWithLabel("Back", "< BACK", _listContent,
                GamePalette.CardAlt, UITheme.Dp(90f), UITheme.Dp(32f), UITheme.Sp(11f), GamePalette.TextPrimary);
            back.Item1.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(32f);
            EventHelper.AddListener(() => _controller?.CloseItemDetails(), back.Item2.onClick);

            CreateDetailHeader(details);
            for (int i = 0; i < details.Locations.Count; i++) CreateLocationRow(details.Locations[i]);

            if (_filterText != null)
                _filterText.text = details.ItemName.ToUpperInvariant() + "  |  " +
                    (selectedPropertyId == null ? "ALL PROPERTIES" : PropertyNameFor(selectedPropertyId));
            UpdateStatus(snapshot);
        }

        public void ShowError(string message)
        {
            if (_statusText == null) return;
            _statusText.color = GamePalette.Red;
            _statusText.text = message;
        }

        private void RebuildChips(string? selectedPropertyId)
        {
            if (_chipContent == null) return;
            ClearChildren(_chipContent);
            AddChip("ALL", selectedPropertyId == null, null);
            IReadOnlyList<PropertyOption> properties = _source.Properties;
            for (int i = 0; i < properties.Count; i++)
                AddChip(properties[i].Name,
                    string.Equals(properties[i].Code, selectedPropertyId, StringComparison.Ordinal), properties[i].Code);
        }

        private void AddChip(string label, bool selected, string? propertyId)
        {
            if (_chipContent == null) return;
            GameObject chip = new GameObject("Chip");
            chip.transform.SetParent(_chipContent, false);
            chip.AddComponent<RectTransform>();
            LayoutElement size = chip.AddComponent<LayoutElement>();
            size.preferredHeight = UITheme.Dp(26f);
            size.preferredWidth = UITheme.Dp(18f) + label.Length * UITheme.Dp(7f);

            Image image = chip.AddComponent<Image>();
            image.sprite = UISprites.Rounded(8f, 32);
            image.type = Image.Type.Sliced;
            image.color = selected ? GamePalette.Blue : GamePalette.Card;
            Button button = chip.AddComponent<Button>();
            button.targetGraphic = image;

            Text text = UIFactory.Text("Label", label, chip.transform, UITheme.Sp(12f), TextAnchor.MiddleCenter, FontStyle.Bold);
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = new Vector2(UITheme.Dp(6f), 0f);
            text.rectTransform.offsetMax = new Vector2(-UITheme.Dp(6f), 0f);
            text.color = selected ? Color.white : GamePalette.TextPrimary;
            text.raycastTarget = false;
            EventHelper.AddListener(() => _controller?.SelectProperty(propertyId), button.onClick);
        }

        private void RebuildRows(IReadOnlyList<StockRow> rows)
        {
            if (_listContent == null) return;
            ClearChildren(_listContent);

            if (rows.Count == 0)
            {
                Text empty = UIFactory.Text("Empty", "No items found.", _listContent,
                    UITheme.Sp(13f), TextAnchor.MiddleCenter, FontStyle.Normal);
                empty.color = GamePalette.TextMuted;
                empty.raycastTarget = false;
                return;
            }

            for (int i = 0; i < rows.Count; i++) CreateStockRow(rows[i]);
        }

        private void CreateStockRow(StockRow row)
        {
            if (_listContent == null) return;
            GameObject rowObject = UIFactory.Panel("Row", _listContent, GamePalette.Card, null, null, false);
            Image image = rowObject.GetComponent<Image>();
            image.sprite = UISprites.Rounded(9f, 32);
            image.type = Image.Type.Sliced;
            Button button = rowObject.AddComponent<Button>();
            button.targetGraphic = image;
            EventHelper.AddListener(() => _controller?.SelectItem(row.ItemId), button.onClick);

            LayoutElement height = rowObject.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(42f);
            height.minHeight = UITheme.Dp(42f);
            HorizontalLayoutGroup layout = rowObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), 0, 0);
            layout.spacing = UITheme.Dp(8f);
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            string nameText = row.HasCachedData ? row.ItemName + "  *" : row.ItemName;
            Text name = UIFactory.Text("Name", nameText, rowObject.transform, UITheme.Sp(13f), TextAnchor.MiddleLeft, FontStyle.Bold);
            name.color = row.HasCachedData ? GamePalette.Orange : GamePalette.TextPrimary;
            name.raycastTarget = false;
            name.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;

            Text qty = UIFactory.Text("Qty", "x" + row.Quantity.ToString(CultureInfo.InvariantCulture), rowObject.transform,
                UITheme.Sp(13f), TextAnchor.MiddleRight, FontStyle.Bold);
            qty.color = GamePalette.Teal;
            qty.raycastTarget = false;
            qty.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(64f);

            Text containers = UIFactory.Text("Containers", row.ContainerCount.ToString(CultureInfo.InvariantCulture) + "x",
                rowObject.transform, UITheme.Sp(11f), TextAnchor.MiddleRight, FontStyle.Normal);
            containers.color = GamePalette.TextMuted;
            containers.raycastTarget = false;
            containers.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(34f);
        }

        private void CreateDetailHeader(ItemDetails details)
        {
            if (_listContent == null) return;
            GameObject card = UIFactory.Panel("DetailHeader", _listContent, GamePalette.CardAlt, null, null, false);
            card.GetComponent<Image>().sprite = UISprites.Rounded(9f, 32);
            LayoutElement height = card.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(54f);
            height.minHeight = UITheme.Dp(54f);

            Text title = UIFactory.Text("Title", details.ItemName, card.transform, UITheme.Sp(15f),
                TextAnchor.UpperLeft, FontStyle.Bold);
            title.rectTransform.anchorMin = Vector2.zero;
            title.rectTransform.anchorMax = Vector2.one;
            title.rectTransform.offsetMin = new Vector2(UITheme.Dp(12f), UITheme.Dp(5f));
            title.rectTransform.offsetMax = new Vector2(-UITheme.Dp(100f), -UITheme.Dp(5f));
            title.color = GamePalette.TextPrimary;
            title.raycastTarget = false;

            Text total = UIFactory.Text("Total", "TOTAL  x" + details.TotalQuantity.ToString(CultureInfo.InvariantCulture),
                card.transform, UITheme.Sp(12f), TextAnchor.MiddleRight, FontStyle.Bold);
            total.rectTransform.anchorMin = new Vector2(0.55f, 0f);
            total.rectTransform.anchorMax = Vector2.one;
            total.rectTransform.offsetMin = Vector2.zero;
            total.rectTransform.offsetMax = new Vector2(-UITheme.Dp(12f), 0f);
            total.color = GamePalette.Teal;
            total.raycastTarget = false;
        }

        private void CreateLocationRow(ItemLocationRow location)
        {
            if (_listContent == null) return;
            GameObject row = UIFactory.Panel("Location", _listContent, GamePalette.Card, null, null, false);
            row.GetComponent<Image>().sprite = UISprites.Rounded(8f, 32);
            LayoutElement height = row.AddComponent<LayoutElement>();
            height.preferredHeight = UITheme.Dp(46f);
            height.minHeight = UITheme.Dp(46f);

            string left = location.PropertyName + "  /  " + location.ContainerName;
            if (location.IsCached) left += "  * cached";
            Text label = UIFactory.Text("LocationName", left, row.transform, UITheme.Sp(11f),
                TextAnchor.MiddleLeft, FontStyle.Normal);
            label.rectTransform.anchorMin = Vector2.zero;
            label.rectTransform.anchorMax = new Vector2(0.78f, 1f);
            label.rectTransform.offsetMin = new Vector2(UITheme.Dp(12f), 0f);
            label.rectTransform.offsetMax = Vector2.zero;
            label.color = location.IsCached ? GamePalette.Orange : GamePalette.TextPrimary;
            label.raycastTarget = false;

            Text qty = UIFactory.Text("Qty", "x" + location.Quantity.ToString(CultureInfo.InvariantCulture), row.transform,
                UITheme.Sp(12f), TextAnchor.MiddleRight, FontStyle.Bold);
            qty.rectTransform.anchorMin = new Vector2(0.78f, 0f);
            qty.rectTransform.anchorMax = Vector2.one;
            qty.rectTransform.offsetMin = Vector2.zero;
            qty.rectTransform.offsetMax = new Vector2(-UITheme.Dp(12f), 0f);
            qty.color = GamePalette.Teal;
            qty.raycastTarget = false;
        }

        private void UpdateStatus(StorageSnapshot snapshot)
        {
            if (_statusText == null) return;
            string time = snapshot.CapturedAt.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            int distinctItems = snapshot.Items.Select(x => x.ItemId).Distinct(StringComparer.Ordinal).Count();
            string scan = $"{distinctItems} items | {snapshot.ContainerCount} containers | " +
                $"{snapshot.LivePropertyCount}/{snapshot.Properties.Count} live | {snapshot.ScanDurationMilliseconds} ms";

            if (snapshot.IsComplete)
            {
                _statusText.color = GamePalette.TextMuted;
                _statusText.text = scan + "\nUpdated " + time;
            }
            else
            {
                _statusText.color = GamePalette.Orange;
                _statusText.text = scan + $" | {snapshot.CachedPropertyCount} cached\nUpdated " + time;
            }
        }

        private string BuildPropertySignature()
        {
            IReadOnlyList<PropertyOption> properties = _source.Properties;
            return string.Join("\u001f", properties.Select(x => x.Code + "=" + x.Name));
        }

        private string PropertyNameFor(string code)
        {
            IReadOnlyList<PropertyOption> properties = _source.Properties;
            for (int i = 0; i < properties.Count; i++)
                if (string.Equals(properties[i].Code, code, StringComparison.Ordinal)) return properties[i].Name;
            return code;
        }

        private static string SortLabel(StockSortMode mode) => mode switch
        {
            StockSortMode.NameDescending => "Z-A",
            StockSortMode.QuantityDescending => "QTY v",
            StockSortMode.QuantityAscending => "QTY ^",
            StockSortMode.ContainersDescending => "BOX v",
            StockSortMode.ContainersAscending => "BOX ^",
            _ => "A-Z",
        };

        private static void ClearChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
        }
    }
}
