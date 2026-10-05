namespace StorageScanner
{
    /// <summary>Mod-wide constants (single source for identity strings).</summary>
    public static class Constants
    {
        public const string ModName = "StorageScanner";
        public const string ModVersion = "0.2.0";
        public const string ModAuthor = "Dominik";
        public const string AppName = "StorageScanner";
        public const string AppTitle = "Storage Scanner";
        public const string IconFileName = "storage_scanner_icon.png";
        public const string GameplaySceneName = "Main";

        /// <summary>Seconds between automatic scans while the app is open (manual REFRESH scans immediately).</summary>
        public const float AutoRefreshInterval = 2.5f;
    }
}
