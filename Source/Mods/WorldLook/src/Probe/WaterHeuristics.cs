using System;
using System.Collections.Generic;

namespace GraphicsProbe;

internal static class WaterHeuristics
{
    // Deliberately lexical. These clues are NOT identification of actual water.
    private static readonly string[] Terms = { "water", "ocean", "river", "lake", "pond", "sea", "canal" };

    public static List<string> Evidence(string? rendererName, string? materialName, string? shaderName)
    {
        var result = new List<string>();
        Add(result, "renderer-name", rendererName);
        Add(result, "material-name", materialName);
        Add(result, "shader-name", shaderName);
        return result;
    }

    private static void Add(List<string> result, string source, string? text)
    {
        if (text == null) return;
        foreach (string term in Terms)
        {
            int start = 0;
            while (start < text.Length)
            {
                int index = text.IndexOf(term, start, StringComparison.OrdinalIgnoreCase);
                if (index < 0) break;
                int end = index + term.Length;
                bool seaToken = term != "sea" || ((index == 0 || !char.IsLetterOrDigit(text[index - 1])) && (end == text.Length || !char.IsLetterOrDigit(text[end])));
                if (seaToken) { result.Add(source + ":" + term); break; }
                start = end;
            }
        }
    }
}
