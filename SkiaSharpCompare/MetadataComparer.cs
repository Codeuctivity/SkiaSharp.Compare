using SkiaSharpCompare.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Codeuctivity.SkiaSharpCompare
{
    internal static class MetadataComparer
    {
        internal static Dictionary<string, (string? ValueA, string? ValueB)>? CompareMetadata(string pathImageActual, string pathImageExpected)
        {
            var metaA = MetadataExtractorAdapter.Extract(pathImageActual);
            var metaB = MetadataExtractorAdapter.Extract(pathImageExpected);

            return GetMetadataDifferences(metaA, metaB);
        }

        internal static Dictionary<string, (string? ValueA, string? ValueB)>? CompareMetadata(Stream imageActual, Stream imageExpected)
        {
            var metaA = MetadataExtractorAdapter.Extract(imageActual);
            var metaB = MetadataExtractorAdapter.Extract(imageExpected);

            return GetMetadataDifferences(metaA, metaB);
        }

        private static Dictionary<string, (string? ValueA, string? ValueB)>? GetMetadataDifferences(IReadOnlyDictionary<string, string> a, IReadOnlyDictionary<string, string> b)
        {
            var diffs = new List<(string, string?, string?)>();
            var allKeys = new HashSet<string>(a.Keys, StringComparer.OrdinalIgnoreCase);
            allKeys.UnionWith(b.Keys);

            foreach (var key in allKeys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase))
            {
                a.TryGetValue(key, out var valA);
                b.TryGetValue(key, out var valB);

                // Normalize whitespace for comparison
                var normA = valA?.Trim() ?? string.Empty;
                var normB = valB?.Trim() ?? string.Empty;

                if (!string.Equals(normA, normB, StringComparison.Ordinal))
                {
                    diffs.Add((key, valA, valB));
                }
            }

            return diffs.ToDictionary(diff => diff.Item1, diff => (diff.Item2, diff.Item3)
            );
        }
    }
}