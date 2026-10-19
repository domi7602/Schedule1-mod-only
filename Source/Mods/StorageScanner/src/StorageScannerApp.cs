using System;
using System.Collections.Generic;
using System.Globalization;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace StorageScanner
{
    /// <summary>Readable portrait stock overview with a collapsible property/category drawer.</summary>
    public sealed class StorageScannerApp : PhoneApp, IStorageScannerView
    {
        private static StorageScannerApp? _active;
        private readonly GameStorageSource _source = new GameStorageSource();
        private readonly List<RowUI> _rows = new List<RowUI>();
        private readonly List<FilterUI> _filters = new List<FilterUI>();
        private readonly List<ButtonBinding> _buttonBindings = new List<ButtonBinding>();
        private UnityEngine.Events.UnityEvent<string>? _searchChangedEvent;

        private sealed class ButtonBinding
        {
            public Transform Owner;
            public UnityEngine.Events.UnityEvent Event;
            public Action Callback;
        }
        private StorageScannerController _controller;
        private GameObject _mainBG;
        private GameObject _filterDrawer;
        private RectTransform _filterContent;
        private RectTransform _listContent;
        private ScrollRect _itemScroll;
        private InputField _searchInput;
        private Text _filterLabel;
        private Text _filterSummary;
        private Text _sortLabel;
        private Text _noticeText;
        private Text _statusText;
        private Text _emptyText;
        private GameObject _detailRoot;
        private Text _detailTitle;
        private Text _detailTotal;
        private RectTransform _detailLocations;
        private IReadOnlyList<ItemLocationRow>? _lastLocations;
        private string[]? _lastFilterOptions;
        private string? _selectedPropertyId;
        private string? _selectedCategoryId;
        private string _search = string.Empty;
        private bool _filtersExpanded;
        private float _lastAutoRefreshTime;
        private StockSortMode _lastSortMode = StockSortMode.NameAscending;

        private sealed class RowUI
        {
            public GameObject Root;
            public Text Name;
            public Text Quantity;
            public Text Metadata;
            public string ItemId = string.Empty;
        }

        private sealed class FilterUI
        {
            public Image Surface;
            public string? Id;
            public bool IsCategory;
        }

        internal static GameStorageSource? ActiveSource => _active?._source;
        protected override string AppName => Constants.AppName;
        protected override string AppTitle => Constants.AppTitle;
        protected override string IconLabel => "Storage Scanner";
        protected override string IconFileName => Constants.IconFileName;
        protected override EOrientation Orientation => EOrientation.Vertical;

        protected override void OnCreated()
        {
            base.OnCreated();
            _active = this;
            _controller = new StorageScannerController(_source, this,
                exception => MelonLogger.Error("[StorageScanner] Scan failed: " + exception));
            MelonEvents.OnUpdate.Unsubscribe(new LemonAction(Update));
            MelonEvents.OnUpdate.Subscribe(new LemonAction(Update));
            MelonLogger.Msg("[StorageScanner] Registered with S1API PhoneApp system.");
        }

        internal static void TearDownForSceneUnload()
        {
            if (_active == null) return;
            _active.ReleaseAllListeners();
            MelonEvents.OnUpdate.Unsubscribe(new LemonAction(_active.Update));
            _active = null;
        }

        internal static void ResetForNewSave()
        {
            if (_active == null) return;
            _active._filtersExpanded = false;
            _active._controller.ResetForNewSave();
        }

        protected override void OnPhoneClosed()
        {
            base.OnPhoneClosed();
            if (NetworkGuard.IsAlive(_mainBG)) _mainBG.SetActive(false);
        }

        private void Update()
        {
            if (!NetworkGuard.IsAlive(_mainBG)) return;
            bool open = IsOpen();
            if (_mainBG.activeSelf != open)
            {
                _mainBG.SetActive(open);
                if (open)
                {
                    _lastAutoRefreshTime = Time.unscaledTime;
                    _controller.Open();
                }
            }
            if (!open) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_detailRoot.activeSelf) _controller.CloseItemDetails();
                else if (_filtersExpanded) ToggleFilters();
                else CloseApp();
                return;
            }
            if (Time.unscaledTime - _lastAutoRefreshTime >= Constants.AutoRefreshInterval)
            {
                _lastAutoRefreshTime = Time.unscaledTime;
                _controller.Refresh();
            }
        }

        protected override void OnCreatedUI(GameObject container)
        {
            ReleaseAllListeners();
            UITheme.InitializeForTextApp(container.GetComponent<RectTransform>());
            _rows.Clear();
            _filters.Clear();
            _lastFilterOptions = null;
            _lastLocations = null;
            _mainBG = UIFactory.Panel("StorageScanner_MainBG", container.transform, GamePalette.Bg, fullAnchor: true);
            _mainBG.SetActive(false);
            Vertical(_mainBG, 6f, 12f);
            BuildHeader(_mainBG.transform);
            BuildSearchField(_mainBG.transform);
            BuildFilterToolbar(_mainBG.transform);
            BuildFilterDrawer(_mainBG.transform);
            _noticeText = Label("Notice", "", _mainBG.transform, 18f, GamePalette.Orange);
            SetHeight(_noticeText.gameObject, 54f);
            _noticeText.gameObject.SetActive(false);
            _listContent = BuildScroll("Items", _mainBG.transform, out _itemScroll);
            var listElement = _itemScroll.gameObject.AddComponent<LayoutElement>();
            listElement.minHeight = UITheme.Dp(100f);
            listElement.flexibleHeight = 1f;
            _emptyText = Label("Empty", "No items found.\nTry another filter or clear your search.", _listContent, 16f, GamePalette.TextMuted);
            _emptyText.alignment = TextAnchor.MiddleCenter;
            SetHeight(_emptyText.gameObject, 90f);
            _emptyText.gameObject.SetActive(false);
            _statusText = Label("Footer", "", _mainBG.transform, 17f, GamePalette.TextMuted);
            SetHeight(_statusText.gameObject, 46f);
            BuildDetailPanel(_mainBG.transform);
            container.AddComponent<StorageScannerInputFocus>().searchInput = _searchInput;
        }

        private void BuildHeader(Transform parent)
        {
            GameObject header = Group("Header", parent);
            SetHeight(header, 46f);
            Horizontal(header, 12f);
            Text title = Label("Title", "Storage", header.transform, 24f, GamePalette.TextPrimary, FontStyle.Bold);
            FlexibleWidth(title.gameObject);
            CreateButton("Refresh", "Refresh", header.transform, 38f, 18f, () => _controller.Refresh(), 94f);
        }

        private void BuildSearchField(Transform parent)
        {
            GameObject search = UIFactory.Panel("Search", parent, GamePalette.Card);
            StyleSurface(search);
            SetHeight(search, 42f);
            InputField input = search.AddComponent<InputField>();
            Text text = Label("Text", "", search.transform, 20f, GamePalette.TextPrimary);
            Stretch(text.rectTransform, 12f, 0f);
            Text placeholder = Label("Placeholder", "Search items...", search.transform, 20f, GamePalette.TextMuted);
            Stretch(placeholder.rectTransform, 12f, 0f);
            input.targetGraphic = search.GetComponent<Image>();
            input.textComponent = text;
            input.placeholder = placeholder;
            input.lineType = InputField.LineType.SingleLine;
            input.caretWidth = 2;
            input.caretColor = Color.white;
            input.selectionColor = new Color(0.23f, 0.51f, 0.96f, 0.45f);
            _searchInput = input;
            _searchChangedEvent = input.onValueChanged;
            EventHelper.AddListener<string>(OnSearchChanged, input.onValueChanged);
        }

        private void BuildFilterToolbar(Transform parent)
        {
            GameObject toolbar = Group("FilterToolbar", parent);
            SetHeight(toolbar, 40f);
            Horizontal(toolbar, 8f);
            Button toggle = CreateButton("FilterToggle", "", toolbar.transform, 40f, 18f, ToggleFilters);
            _filterLabel = toggle.transform.GetChild(0).GetComponent<Text>();
            _filterLabel.fontStyle = FontStyle.Bold;
            // Selection is full-width and can wrap, rather than being clipped in a button.
            _filterSummary = Label("Selection", "", parent, 17f, GamePalette.TextMuted);
            SetHeight(_filterSummary.gameObject, 44f);
            _filterSummary.gameObject.SetActive(false);
            Button sort = CreateButton("Sort", "Name A-Z", toolbar.transform, 40f, 17f,
                () => _controller.SetSortMode(_lastSortMode.Next()), 146f);
            _sortLabel = sort.transform.GetChild(0).GetComponent<Text>();
            UpdateFilterToggle();
        }

        private void BuildFilterDrawer(Transform parent)
        {
            _filterDrawer = Group("FilterDrawer", parent);
            SetHeight(_filterDrawer, 180f);
            Vertical(_filterDrawer, 6f);
            GameObject header = Group("FilterDrawerHeader", _filterDrawer.transform);
            SetHeight(header, 32f);
            Horizontal(header, 8f);
            Text caption = Label("Caption", "Narrow your stock", header.transform, 14f, GamePalette.TextMuted);
            FlexibleWidth(caption.gameObject);
            CreateButton("Reset", "Reset", header.transform, 32f, 14f, () => _controller.ResetFilters(), 72f);
            _filterContent = BuildScroll("FilterOptions", _filterDrawer.transform, out ScrollRect scroll);
            var element = scroll.gameObject.AddComponent<LayoutElement>();
            element.minHeight = 0f;
            element.flexibleHeight = 1f;
            _filterDrawer.SetActive(_filtersExpanded);
        }

        private void ToggleFilters()
        {
            _filtersExpanded = !_filtersExpanded;
            _filterDrawer.SetActive(_filtersExpanded);
            UpdateFilterToggle();
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_mainBG.GetComponent<RectTransform>());
        }

        private void UpdateFilterToggle() => _filterLabel.text = _filtersExpanded ? "Filters  ^" : "Filters  v";

        public void Render(StorageScannerViewState state)
        {
            if (!NetworkGuard.IsAlive(_mainBG)) return;
            bool selectionChanged = !string.Equals(_selectedPropertyId, state.SelectedPropertyId, StringComparison.Ordinal)
                || !string.Equals(_selectedCategoryId, state.SelectedCategoryId, StringComparison.Ordinal)
                || !string.Equals(_search, state.Search, StringComparison.Ordinal) || _lastSortMode != state.SortMode;
            _selectedPropertyId = state.SelectedPropertyId;
            _selectedCategoryId = state.SelectedCategoryId;
            _search = state.Search;
            _lastSortMode = state.SortMode;
            _searchInput.SetTextWithoutNotify(state.Search);
            _sortLabel.text = SortLabel(state.SortMode);
            _filterDrawer.SetActive(_filtersExpanded);
            UpdateFilterToggle();
            _filterSummary.text = state.SelectedCategoryId == null && state.SelectedPropertyId == null
                ? "All stock" : (state.SelectedCategoryId == null ? "All categories" : CategoryLabel(state.SelectedCategoryId))
                    + " · " + (state.SelectedPropertyId == null ? "All properties" : PropertyNameFor(state.SelectedPropertyId));
            _filterSummary.gameObject.SetActive(state.SelectedCategoryId != null || state.SelectedPropertyId != null);
            RebuildFiltersIfNeeded(state);
            for (int i = 0; i < _filters.Count; i++)
            {
                FilterUI filter = _filters[i];
                filter.Surface.color = string.Equals(filter.Id, filter.IsCategory ? state.SelectedCategoryId : state.SelectedPropertyId, StringComparison.Ordinal)
                    ? GamePalette.Blue : GamePalette.Card;
            }
            RenderRows(state.Rows);
            _emptyText.gameObject.SetActive(state.HasScanned && state.Rows.Count == 0);
            if (selectionChanged) _itemScroll.verticalNormalizedPosition = 1f;
            bool showDetails = state.Page == StorageScannerPage.ItemDetails && state.ItemDetails != null;
            _detailRoot.SetActive(showDetails);
            if (showDetails) RenderDetails(state.ItemDetails!);
            RenderStatus(state);
        }

        private void RenderStatus(StorageScannerViewState state)
        {
            StorageSnapshot snapshot = state.Snapshot;
            _statusText.text = !state.HasScanned ? "No scan yet" : state.Rows.Count.ToString(CultureInfo.InvariantCulture)
                + (state.Rows.Count == 1 ? " item type shown" : " item types shown")
                + "\nLast scan " + snapshot.CapturedAt.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            // Raw snapshot.Status is diagnostic data and is deliberately never rendered.
            bool waiting = state.ScanState == StorageScanState.Waiting;
            bool failed = state.ScanState == StorageScanState.Failed;
            _noticeText.gameObject.SetActive(waiting || failed || !snapshot.IsComplete || snapshot.CachedPropertyCount > 0);
            _noticeText.color = failed ? GamePalette.Red : waiting ? GamePalette.TextMuted : GamePalette.Orange;
            if (failed) _noticeText.text = state.ErrorMessage;
            else if (waiting) _noticeText.text = "Waiting for storage\nLoad a save with an owned property.";
            else if (snapshot.CachedPropertyCount > 0)
                _noticeText.text = "Last known stock · " + snapshot.LivePropertyCount + "/" + snapshot.PropertyCount + " properties live. Visit others to update.";
            else if (!snapshot.IsComplete)
                _noticeText.text = "Partial stock · Visit other properties to update.";
        }

        private void OnSearchChanged(string value) => _controller.SetSearch(value);

        private string PropertyNameFor(string code)
        {
            IReadOnlyList<PropertyOption> properties = _source.Properties;
            for (int i = 0; i < properties.Count; i++)
                if (string.Equals(properties[i].Code, code, StringComparison.Ordinal)) return properties[i].Name;
            return code;
        }

        private void RebuildFiltersIfNeeded(StorageScannerViewState state)
        {
            var categorySet = new SortedSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < state.Snapshot.Items.Count; i++) categorySet.Add(state.Snapshot.Items[i].CategoryId);
            // Retain the selected category when the latest scan has no matching items.
            if (state.SelectedCategoryId != null) categorySet.Add(state.SelectedCategoryId);
            var categories = new List<string>(categorySet);
            IReadOnlyList<PropertyOption> properties = _source.Properties;
            var options = new List<string>();
            for (int i = 0; i < properties.Count; i++) options.Add(properties[i].Code + "\n" + properties[i].Name);
            options.Add("--categories--");
            options.AddRange(categories);
            if (SameOptions(_lastFilterOptions, options)) return;
            ClearChildren(_filterContent);
            _filters.Clear();
            _lastFilterOptions = options.ToArray();
            AddSectionLabel("Category");
            GameObject? row = null;
            AddFilterOption(ref row, 0, "All categories", null, true);
            for (int i = 0; i < categories.Count; i++) AddFilterOption(ref row, i + 1, CategoryLabel(categories[i]), categories[i], true);
            AddSectionLabel("Property");
            row = null;
            AddFilterOption(ref row, 0, "All properties", null, false);
            for (int i = 0; i < properties.Count; i++) AddFilterOption(ref row, i + 1, properties[i].Name, properties[i].Code, false);
            _filterContent.sizeDelta = new Vector2(0f, _filterContent.sizeDelta.y);
        }

        private void AddSectionLabel(string caption)
        {
            Text label = Label(caption, caption, _filterContent, 17f, GamePalette.TextMuted, FontStyle.Bold);
            SetHeight(label.gameObject, 24f);
        }

        private void AddFilterOption(ref GameObject? row, int index, string caption, string? id, bool isCategory)
        {
            if (index % 2 == 0)
            {
                row = Group("FilterRow", _filterContent);
                SetHeight(row, 40f);
                Horizontal(row, 6f);
            }
            Button button = CreateButton("FilterOption", caption, row!.transform, 40f, 18f,
                () => { if (isCategory) _controller.SelectCategory(id); else _controller.SelectProperty(id); });
            _filters.Add(new FilterUI { Surface = button.GetComponent<Image>(), Id = id, IsCategory = isCategory });
        }

        private static bool SameOptions(string[]? previous, List<string> current)
        {
            if (previous == null || previous.Length != current.Count) return false;
            for (int i = 0; i < previous.Length; i++) if (previous[i] != current[i]) return false;
            return true;
        }

        private void RenderRows(IReadOnlyList<StockRow> rows)
        {
            _listContent.sizeDelta = new Vector2(0f, _listContent.sizeDelta.y);
            _emptyText.gameObject.SetActive(rows.Count == 0);
            for (int i = 0; i < rows.Count; i++)
            {
                if (i == _rows.Count) _rows.Add(CreateRow());
                RowUI ui = _rows[i];
                StockRow row = rows[i];
                ui.Root.SetActive(true);
                ui.ItemId = row.ItemId;
                ui.Name.text = row.ItemName;
                ui.Quantity.text = row.Quantity.ToString("N0", CultureInfo.InvariantCulture);
                ui.Metadata.text = row.ContainerCount.ToString(CultureInfo.InvariantCulture)
                    + (row.ContainerCount == 1 ? " container" : " containers")
                    + (row.IncludesCachedData ? " / last known stock" : string.Empty);
                ui.Metadata.color = row.IncludesCachedData ? GamePalette.Orange : GamePalette.TextMuted;
            }
            for (int i = rows.Count; i < _rows.Count; i++) _rows[i].Root.SetActive(false);
        }

        private RowUI CreateRow()
        {
            GameObject root = UIFactory.Panel("StockRow", _listContent, GamePalette.Card);
            StyleSurface(root);
            root.AddComponent<LayoutElement>().minHeight = UITheme.Dp(72f);
            Vertical(root, 3f, 8f);
            GameObject main = Group("NameAndQuantity", root.transform);
            main.AddComponent<LayoutElement>().minHeight = UITheme.Dp(32f);
            Horizontal(main, 10f);
            Text name = Label("Name", "", main.transform, 24f, GamePalette.TextPrimary, FontStyle.Bold);
            FlexibleWidth(name.gameObject);
            Text quantity = Label("Quantity", "", main.transform, 25f, GamePalette.Teal, FontStyle.Bold);
            quantity.alignment = TextAnchor.MiddleRight;
            var qtyLayout = quantity.gameObject.AddComponent<LayoutElement>();
            qtyLayout.minWidth = UITheme.Dp(84f);
            // Text preferred width may grow for large totals; never wrap a number.
            quantity.horizontalOverflow = HorizontalWrapMode.Overflow;
            Text metadata = Label("Containers", "", root.transform, 18f, GamePalette.TextMuted);
            var ui = new RowUI { Root = root, Name = name, Quantity = quantity, Metadata = metadata };
            Button button = root.AddComponent<Button>();
            button.targetGraphic = root.GetComponent<Image>();
            BindButton(button, () => _controller.SelectItem(ui.ItemId));
            return ui;
        }

        private void BuildDetailPanel(Transform parent)
        {
            _detailRoot = UIFactory.Panel("ItemDetails", parent, GamePalette.Bg, fullAnchor: true);
            _detailRoot.AddComponent<LayoutElement>().ignoreLayout = true;
            Vertical(_detailRoot, 10f, 14f);
            CreateButton("Back", "< Back to stock", _detailRoot.transform, 40f, 16f, () => _controller.CloseItemDetails());
            _detailTitle = Label("ItemTitle", "", _detailRoot.transform, 24f, GamePalette.TextPrimary, FontStyle.Bold);
            _detailTitle.gameObject.AddComponent<LayoutElement>().minHeight = UITheme.Dp(38f);
            _detailTotal = Label("Total", "", _detailRoot.transform, 20f, GamePalette.Teal, FontStyle.Bold);
            SetHeight(_detailTotal.gameObject, 36f);
            _detailLocations = BuildScroll("Locations", _detailRoot.transform, out ScrollRect scroll);
            scroll.gameObject.AddComponent<LayoutElement>().flexibleHeight = 1f;
            _detailRoot.SetActive(false);
        }

        private void RenderDetails(ItemDetails details)
        {
            _detailTitle.text = details.ItemName;
            _detailTotal.text = "All properties: " + details.TotalQuantity.ToString("N0", CultureInfo.InvariantCulture);
            if (LocationsEquivalent(_lastLocations, details.Locations)) return;
            ClearChildren(_detailLocations);
            _detailLocations.sizeDelta = new Vector2(0f, _detailLocations.sizeDelta.y);
            _lastLocations = details.Locations;
            string? lastProperty = null;
            for (int i = 0; i < details.Locations.Count; i++)
            {
                ItemLocationRow location = details.Locations[i];
                if (!string.Equals(lastProperty, location.PropertyName, StringComparison.Ordinal))
                {
                    Text caption = Label("Property", location.PropertyName, _detailLocations, 16f, GamePalette.TextMuted, FontStyle.Bold);
                    caption.gameObject.AddComponent<LayoutElement>().minHeight = UITheme.Dp(30f);
                    lastProperty = location.PropertyName;
                }
                GameObject row = UIFactory.Panel("Location", _detailLocations, GamePalette.Card);
                StyleSurface(row);
                row.AddComponent<LayoutElement>().minHeight = UITheme.Dp(48f);
                Horizontal(row, 10f, 12f);
                Text name = Label("Name", location.ContainerName, row.transform, 17f, GamePalette.TextPrimary);
                FlexibleWidth(name.gameObject);
                Text quantity = Label("Quantity", location.Quantity.ToString("N0", CultureInfo.InvariantCulture), row.transform, 19f, GamePalette.Teal, FontStyle.Bold);
                quantity.alignment = TextAnchor.MiddleRight;
                var quantityLayout = quantity.gameObject.AddComponent<LayoutElement>();
                quantityLayout.minWidth = UITheme.Dp(84f);
                quantityLayout.preferredWidth = UITheme.Dp(84f);
            }
        }

        private static bool LocationsEquivalent(IReadOnlyList<ItemLocationRow>? left, IReadOnlyList<ItemLocationRow> right)
        {
            if (left == null || left.Count != right.Count) return false;
            for (int i = 0; i < left.Count; i++)
                if (left[i].PropertyName != right[i].PropertyName || left[i].ContainerName != right[i].ContainerName || left[i].Quantity != right[i].Quantity) return false;
            return true;
        }

        private static RectTransform BuildScroll(string name, Transform parent, out ScrollRect scroll)
        {
            RectTransform content = UIFactory.ScrollableVerticalList(name, parent, out scroll);
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = UITheme.Dp(28f);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = Vector2.zero;
            var layout = content.GetComponent<VerticalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.padding = new RectOffset();
            layout.spacing = UITheme.Dp(6f);
            var fitter = content.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            return content;
        }

        private static GameObject Group(string name, Transform parent)
        {
            var group = new GameObject(name);
            group.transform.SetParent(parent, false);
            group.AddComponent<RectTransform>();
            return group;
        }

        private static Text Label(string name, string text, Transform parent, float size, Color color, FontStyle style = FontStyle.Normal)
        {
            Text label = UIFactory.Text(name, text, parent, UITheme.Sp(size), TextAnchor.MiddleLeft, style);
            label.color = color;
            label.raycastTarget = false;
            label.supportRichText = false;
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Truncate;
            return label;
        }

        private Button CreateButton(string name, string caption, Transform parent, float height, float fontSize, Action action, float? width = null)
        {
            GameObject root = UIFactory.Panel(name, parent, GamePalette.CardAlt);
            StyleSurface(root);
            SetHeight(root, height);
            LayoutElement element = root.GetComponent<LayoutElement>();
            element.minWidth = width.HasValue ? UITheme.Dp(width.Value) : 0f;
            element.preferredWidth = width.HasValue ? UITheme.Dp(width.Value) : 0f;
            element.flexibleWidth = width.HasValue ? 0f : 1f;
            Text label = Label("Label", caption, root.transform, fontSize, GamePalette.TextPrimary);
            label.alignment = TextAnchor.MiddleCenter;
            Stretch(label.rectTransform, 8f, 0f);
            Button button = root.AddComponent<Button>();
            button.targetGraphic = root.GetComponent<Image>();
            BindButton(button, action);
            return button;
        }

        private static void StyleSurface(GameObject root)
        {
            Image image = root.GetComponent<Image>();
            image.sprite = UISprites.Rounded(8f, 32);
            image.type = Image.Type.Sliced;
        }

        private static void Stretch(RectTransform rect, float horizontalPadding, float verticalPadding)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(UITheme.Dp(horizontalPadding), UITheme.Dp(verticalPadding));
            rect.offsetMax = -rect.offsetMin;
        }

        private static void SetHeight(GameObject root, float height)
        {
            LayoutElement element = root.GetComponent<LayoutElement>() ?? root.AddComponent<LayoutElement>();
            element.minHeight = UITheme.Dp(height);
            element.preferredHeight = UITheme.Dp(height);
            // Fixed bands must override flexible size supplied by their layout group.
            element.flexibleHeight = 0f;
            element.layoutPriority = 1;
        }

        private static void FlexibleWidth(GameObject root)
        {
            var element = root.AddComponent<LayoutElement>();
            element.minWidth = 0f;
            element.preferredWidth = 0f;
            element.flexibleWidth = 1f;
        }

        private static void Vertical(GameObject root, float spacing, float padding = 0f)
        {
            var layout = root.AddComponent<VerticalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.spacing = UITheme.Dp(spacing);
            int inset = (int)UITheme.Dp(padding);
            layout.padding = new RectOffset(inset, inset, inset, inset);
        }

        private static void Horizontal(GameObject root, float spacing, float padding = 0f)
        {
            var layout = root.AddComponent<HorizontalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.spacing = UITheme.Dp(spacing);
            int inset = (int)UITheme.Dp(padding);
            layout.padding = new RectOffset(inset, inset, inset, inset);
        }

        private static string CategoryLabel(string id)
        {
            switch (id)
            {
                case "Product": return "Products";
                case "Consumable": return "Consumables";
                case "Ingredient": return "Ingredients";
                case "Decoration": return "Decorations";
                case "Other": return "Other items";
                default: return id;
            }
        }

        private static string SortLabel(StockSortMode mode)
        {
            switch (mode)
            {
                case StockSortMode.NameDescending: return "Name Z-A";
                case StockSortMode.QuantityDescending: return "Quantity high";
                case StockSortMode.QuantityAscending: return "Quantity low";
                case StockSortMode.ContainersDescending: return "Containers high";
                case StockSortMode.ContainersAscending: return "Containers low";
                default: return "Name A-Z";
            }
        }

        private void BindButton(Button button, Action callback)
        {
            EventHelper.AddListener(callback, button.onClick);
            _buttonBindings.Add(new ButtonBinding { Owner = button.transform, Event = button.onClick, Callback = callback });
        }

        private void ReleaseButtonListeners(Transform? subtree = null)
        {
            for (int i = _buttonBindings.Count - 1; i >= 0; i--)
            {
                ButtonBinding binding = _buttonBindings[i];
                if (subtree != null && !binding.Owner.IsChildOf(subtree)) continue;
                EventHelper.RemoveListener(binding.Callback, binding.Event);
                _buttonBindings.RemoveAt(i);
            }
        }

        private void ReleaseAllListeners()
        {
            ReleaseButtonListeners();
            if (_searchChangedEvent != null)
            {
                EventHelper.RemoveListener<string>(OnSearchChanged, _searchChangedEvent);
                _searchChangedEvent = null;
            }
        }

        private void ClearChildren(Transform parent)
        {
            // EventHelper keeps a static delegate registry: remove bindings before destroying controls.
            ReleaseButtonListeners(parent);
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject child = parent.GetChild(i).gameObject;
                child.SetActive(false);
                UnityEngine.Object.Destroy(child);
            }
        }
    }
}
