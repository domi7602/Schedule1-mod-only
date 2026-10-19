// ============================================================================
// Consolidated domain file: PocketShop Unity-free policy helpers
//
// Mechanically merged from the previously separate sources listed below.
// Each original file is preserved verbatim inside its own block namespace, so
// every local import binding stays scoped exactly as before. Only the file-scoped
// namespace declarations became block namespaces; no type, initializer, method
// body or using directive was rewritten.
//
//   - Source/Mods/PocketShop/src/Services/PurchaseRules.cs
//   - Source/Mods/PocketShop/src/UI/StoreGridLayout.cs
//
// Domain: the two Unity-free policy helpers shared by runtime code and focused
// tests (purchase/stock/pricing/refund rules and the directory grid layout math).
// ============================================================================

// ==== BEGIN original: Source/Mods/PocketShop/src/Services/PurchaseRules.cs ====
namespace PocketShop.Services
{
    using System;


    public enum StockKind
    {
        Unknown,
        NotOffered,
        Unlimited,
        LimitedAvailable,
        LimitedEmpty
    }

    public readonly record struct StockState(StockKind Kind, int Quantity, int QuantityLimit)
    {
        public bool CanBuy(int quantity)
        {
            if (quantity < 1) return false;
            return Kind switch
            {
                StockKind.Unlimited => true,
                StockKind.LimitedAvailable => quantity <= Quantity,
                _ => false
            };
        }
    }

    public readonly record struct PurchasePricing(float UnitPrice, float Subtotal, float DeliveryFee, float Total);

    public enum PaymentPreference
    {
        Cash,
        Online,
        PreferCash,
        PreferOnline
    }

    public enum PaymentAccount
    {
        Cash,
        Bank
    }

    /// <summary>
    /// One observation of a money balance against an expected delta. The bank path needs this
    /// tri-state: a ServerRpc transaction is applied on a later network tick, so "not moved yet"
    /// (Unchanged) is neither success nor failure until the bounded confirmation window ends.
    /// </summary>
    public enum BalanceObservation
    {
        /// <summary>The balance has not changed yet; a deferred transaction may still apply.</summary>
        Unchanged,
        /// <summary>The observed change equals the expected representable delta exactly.</summary>
        Confirmed,
        /// <summary>The balance moved, but not by the expected amount (or was unreadable).</summary>
        Mismatch
    }

    /// <summary>Pure validation shared by the live purchase path and UI stock/price presentation.</summary>
    public static class PurchaseRules
    {
        public const int UnlimitedStockSentinel = -1;

        public static StockState ResolveStock(bool listingValid, bool shouldShow, bool limitedStock, int currentStock)
        {
            if (!listingValid) return new StockState(StockKind.Unknown, 0, 0);
            if (!shouldShow) return new StockState(StockKind.NotOffered, 0, 0);
            if (!limitedStock) return new StockState(StockKind.Unlimited, UnlimitedStockSentinel, UnlimitedStockSentinel);
            if (currentStock < 0) return new StockState(StockKind.Unknown, 0, 0);
            if (currentStock == 0) return new StockState(StockKind.LimitedEmpty, 0, 0);
            return new StockState(StockKind.LimitedAvailable, currentStock, currentStock);
        }

        /// <summary>
        /// Uses the existing float formula (price * (1 + percent / 100), then quantity + flat delivery)
        /// without adding a rounding rule. Rejects invalid inputs and non-representable results.
        /// </summary>
        public static bool TryCalculatePricing(float price, float serviceFeePercent, float deliveryFee, int quantity, out PurchasePricing pricing)
        {
            pricing = default;
            if (quantity < 1 || !IsFiniteNonNegative(price) || !IsFiniteNonNegative(serviceFeePercent)
                || !IsFiniteNonNegative(deliveryFee))
            {
                return false;
            }

            float multiplier = 1f + serviceFeePercent / 100f;
            if (!IsFiniteNonNegative(multiplier)) return false;

            float unitPrice = price * multiplier;
            if (!IsFiniteNonNegative(unitPrice)) return false;

            float subtotal = unitPrice * quantity;
            if (!IsFiniteNonNegative(subtotal)) return false;

            float total = subtotal + deliveryFee;
            if (!IsFiniteNonNegative(total)) return false;

            pricing = new PurchasePricing(unitPrice, subtotal, deliveryFee, total);
            return true;
        }

        public static bool TryCalculateRefund(float unitPrice, float deliveryFee, int quantity, int deliveredCount, out float refund)
        {
            refund = 0f;
            if (!IsFiniteNonNegative(unitPrice) || !IsFiniteNonNegative(deliveryFee)
                || quantity < 1 || deliveredCount < 0 || deliveredCount > quantity)
            {
                return false;
            }

            int undelivered = quantity - deliveredCount;
            refund = unitPrice * undelivered + (deliveredCount == 0 ? deliveryFee : 0f);
            if (!IsFiniteNonNegative(refund))
            {
                refund = 0f;
                return false;
            }
            return true;
        }

        public static bool TryResolvePayment(
            PaymentPreference preference, float total, float cashBalance, float bankBalance, out PaymentAccount account)
        {
            account = preference == PaymentPreference.Online || preference == PaymentPreference.PreferOnline
                ? PaymentAccount.Bank
                : PaymentAccount.Cash;
            if (!IsFiniteNonNegative(total) || !IsFiniteNonNegative(cashBalance) || !IsFiniteNonNegative(bankBalance))
                return false;

            switch (preference)
            {
                case PaymentPreference.Cash:
                    account = PaymentAccount.Cash;
                    return cashBalance >= total;
                case PaymentPreference.Online:
                    account = PaymentAccount.Bank;
                    return bankBalance >= total;
                case PaymentPreference.PreferCash:
                    if (cashBalance >= total) { account = PaymentAccount.Cash; return true; }
                    if (bankBalance >= total) { account = PaymentAccount.Bank; return true; }
                    account = PaymentAccount.Cash;
                    return false;
                case PaymentPreference.PreferOnline:
                    if (bankBalance >= total) { account = PaymentAccount.Bank; return true; }
                    if (cashBalance >= total) { account = PaymentAccount.Cash; return true; }
                    account = PaymentAccount.Bank;
                    return false;
                default:
                    account = PaymentAccount.Bank;
                    return false;
            }
        }

        private static bool IsFiniteNonNegative(float value) => float.IsFinite(value) && value >= 0f;

        /// <summary>Checks that the exposed float balance changed by the exact representable amount.</summary>
        public static bool BalanceDeltaMatches(float before, float after, float expectedDelta)
        {
            if (!float.IsFinite(before) || !float.IsFinite(after) || !float.IsFinite(expectedDelta)) return false;
            if (expectedDelta == 0f) return after.Equals(before);

            float expectedAfter = before + expectedDelta;
            if (!float.IsFinite(expectedAfter) || expectedAfter.Equals(before)) return false;
            return after.Equals(expectedAfter)
                && (expectedDelta < 0f ? after < before : after > before);
        }

        /// <summary>
        /// Classifies one balance observation against the expected money delta. Used by the
        /// deferred bank confirmation: a ServerRpc-delivered transaction is applied on the
        /// server's next network tick, so "Unchanged" must be re-checked (bounded), never
        /// treated as failure or success by itself.
        /// </summary>
        public static BalanceObservation ClassifyBalanceObservation(float before, float after, float expectedDelta)
        {
            if (!float.IsFinite(before) || !float.IsFinite(after) || !float.IsFinite(expectedDelta))
                return BalanceObservation.Mismatch;
            if (after.Equals(before))
                return BalanceObservation.Unchanged;
            return BalanceDeltaMatches(before, after, expectedDelta)
                ? BalanceObservation.Confirmed
                : BalanceObservation.Mismatch;
        }

        /// <summary>
        /// True when a bank transaction captured from the vanilla MoneyManager matches the
        /// transaction this purchase initiated: same title, exactly the applied amount, and
        /// not older than the send time (small clock tolerance for ordering only — the
        /// amount comparison itself stays exact, no money tolerance).
        /// </summary>
        public static bool MatchesAppliedBankTransaction(
            string expectedTitle, float expectedAmount, float sentAt,
            string observedTitle, float observedAmount, float observedAt)
        {
            if (!float.IsFinite(expectedAmount) || !float.IsFinite(sentAt)
                || !float.IsFinite(observedAmount) || !float.IsFinite(observedAt))
                return false;
            if (!string.Equals(expectedTitle ?? string.Empty, observedTitle ?? string.Empty, StringComparison.Ordinal))
                return false;
            if (!observedAmount.Equals(expectedAmount))
                return false;
            return observedAt >= sentAt - AppliedTransactionClockToleranceSeconds;
        }

        /// <summary>Clock-order tolerance for captured-vs-sent timestamps; money amounts stay exact.</summary>
        public const float AppliedTransactionClockToleranceSeconds = 0.25f;

        /// <summary>True once the bounded confirmation window (realtime seconds) has elapsed.</summary>
        public static bool IsConfirmationExpired(float startedAt, float now, float windowSeconds)
        {
            if (!float.IsFinite(startedAt) || !float.IsFinite(now) || !float.IsFinite(windowSeconds) || windowSeconds <= 0f)
                return false;
            return now - startedAt >= windowSeconds;
        }

    }
}
// ==== END original: Source/Mods/PocketShop/src/Services/PurchaseRules.cs ====

// ==== BEGIN original: Source/Mods/PocketShop/src/UI/StoreGridLayout.cs ====
namespace PocketShop.UI
{
    using System;


    /// <summary>
    /// Pure, Unity-free layout policy for the store directory grid.
    ///
    /// The grid is a rectangle: a fixed number of columns (<see cref="Columns"/>) and only full rows.
    /// A catalog whose size is not a multiple of the column count gets trailing placeholder cells
    /// (<see cref="PlaceholderCount"/>) which the pane renders as subtle non-interactive fillers, so the
    /// last row keeps the rectangle shape instead of ending ragged.
    ///
    /// The tile height is sized for a fixed number of visible rows (<see cref="TargetVisibleRows"/>),
    /// never for the catalog size: the full directory, a search result and a one-shop catalog all use
    /// the same tile size, and a catalog that needs more rows scrolls instead of shrinking the tiles.
    /// </summary>
    public static class StoreGridLayout
    {
        /// <summary>Fixed column count of the directory grid.</summary>
        public const int Columns = 4;

        /// <summary>Rows the tile height is sized for, so three rows are visible without scrolling.</summary>
        public const int TargetVisibleRows = 3;

        /// <summary>Rendered line limit of the store-name band; longer names truncate, they are never rewritten.</summary>
        public const int NameLineLimit = 2;

        /// <summary>Grid rows needed for the given catalog size (0 when there is nothing to show).</summary>
        public static int RowCount(int itemCount)
        {
            if (itemCount <= 0) return 0;
            return (itemCount + Columns - 1) / Columns;
        }

        /// <summary>Cells of the complete rectangle: every row full, trailing cells are placeholders.</summary>
        public static int CellCount(int itemCount)
        {
            return RowCount(itemCount) * Columns;
        }

        /// <summary>Trailing non-interactive filler cells that complete the last row.</summary>
        public static int PlaceholderCount(int itemCount)
        {
            int items = itemCount > 0 ? itemCount : 0;
            return CellCount(itemCount) - items;
        }

        /// <summary>True for the padded cells that complete the last row; never true for a real item index.</summary>
        public static bool IsPlaceholderCell(int index, int itemCount)
        {
            return index >= 0 && index >= itemCount && index < CellCount(itemCount);
        }

        /// <summary>
        /// Rows the tile height is sized for. The grid keeps one tile size for every catalog size, so the
        /// visible row count never depends on how many entries are registered.
        /// </summary>
        public static int SizingRows(int itemCount)
        {
            return itemCount <= 0 ? 0 : TargetVisibleRows;
        }

        /// <summary>
        /// Tile height for the current viewport: the sizing rows fill the viewport exactly, floored at the
        /// minimum usable tile height. The item count only decides whether there is a grid at all.
        /// </summary>
        public static float TileHeight(float viewportHeight, int itemCount, float minTileHeight, float pad, float gap)
        {
            float floor = Sanitize(minTileHeight);
            int rows = SizingRows(itemCount);
            if (rows <= 0) return floor;
            if (!float.IsFinite(viewportHeight) || viewportHeight <= 0f) return floor;

            float spacing = Sanitize(gap) * (rows - 1);
            float available = viewportHeight - 2f * Sanitize(pad) - spacing;
            float target = available / rows;
            if (!float.IsFinite(target)) return floor;
            return target > floor ? target : floor;
        }

        /// <summary>
        /// Content height of the grid, never shorter than the viewport: more rows than the sizing target
        /// overflow into the scroller instead of compressing the tiles. An empty grid has no content
        /// height at all.
        /// </summary>
        public static float ContentHeight(int itemCount, float tileHeight, float pad, float gap, float viewportHeight)
        {
            float usableViewport = float.IsFinite(viewportHeight) && viewportHeight > 0f ? viewportHeight : 0f;
            int rows = RowCount(itemCount);
            if (rows <= 0) return 0f;

            float grid = 2f * Sanitize(pad) + rows * Sanitize(tileHeight) + Sanitize(gap) * (rows - 1);
            if (!float.IsFinite(grid)) return usableViewport;
            return grid > usableViewport ? grid : usableViewport;
        }

        /// <summary>Height for two name lines with 25% headroom for native font metrics.</summary>
        public static float NameBandHeight(float fontSize)
        {
            if (!float.IsFinite(fontSize) || fontSize <= 0f) return 0f;
            return fontSize * NameLineLimit * 1.25f;
        }

        /// <summary>
        /// Banner height that fits the two-line name band, the item-count line and the banner insets.
        /// The band is fixed, so a longer name wraps into the second line and truncates there.
        /// </summary>
        public static float BannerMinHeight(float nameFontSize, float countFontSize, float padTop, float padBottom, float spacing)
        {
            return NameBandHeight(nameFontSize) + Sanitize(countFontSize) + Sanitize(padTop) + Sanitize(padBottom) + Sanitize(spacing);
        }

        /// <summary>Minimum usable tile height: the avatar block plus its banner.</summary>
        public static float MinTileHeight(float avatarMinHeight, float bannerMinHeight)
        {
            return Sanitize(avatarMinHeight) + Sanitize(bannerMinHeight);
        }

        private static float Sanitize(float value)
        {
            return float.IsFinite(value) && value > 0f ? value : 0f;
        }
    }
}
// ==== END original: Source/Mods/PocketShop/src/UI/StoreGridLayout.cs ====
