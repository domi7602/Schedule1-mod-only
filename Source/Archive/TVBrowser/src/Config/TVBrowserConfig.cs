using System;
using System.Collections.Generic;

namespace TVBrowser.Config;

public sealed class TVBrowserConfig
{
    public string HomePage { get; set; } = "https://www.youtube.com";
    public int ResolutionWidth { get; set; } = 1280;
    public int ResolutionHeight { get; set; } = 720;
    public int TargetFps { get; set; } = 60;
    public float Volume { get; set; } = 1.0f;
    public bool AutoLoadHomePage { get; set; } = true;

    public List<BookmarkItem> Bookmarks { get; set; } = new()
    {
        new BookmarkItem { Title = "YouTube", Url = "https://www.youtube.com", Icon = "🔴" },
        new BookmarkItem { Title = "Twitch", Url = "https://www.twitch.tv", Icon = "🟣" },
        new BookmarkItem { Title = "Google", Url = "https://www.google.com", Icon = "🔍" },
        new BookmarkItem { Title = "Wikipedia", Url = "https://www.wikipedia.org", Icon = "📖" }
    };
}

public sealed class BookmarkItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Icon { get; set; } = "🌐";
}
