using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkiaSharpCompare.Metadata
{
    public static class MetadataExtractorAdapter
    {
        // Reads metadata tags from the provided image stream and returns a normalized dictionary.
        // Caller must ensure the stream is seekable or pass a copy.
        public static IReadOnlyDictionary<string, string> Extract(Stream imageStream)
        {
            var map = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            // MetadataExtractor reads from stream
            var directories = ImageMetadataReader.ReadMetadata(imageStream);
            return CreateResult(map, directories);
        }

        public static IReadOnlyDictionary<string, string> Extract(string imagePath)
        {
            var map = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            // MetadataExtractor reads from stream
            var directories = ImageMetadataReader.ReadMetadata(imagePath);
            return CreateResult(map, directories);
        }

        private static IReadOnlyDictionary<string, string> CreateResult(Dictionary<string, string> map, IReadOnlyList<MetadataExtractor.Directory> directories)
        {
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    // Normalize key as "Directory:TagName"
                    var key = $"{directory.Name}:{tag.Name}";
                    if (!map.ContainsKey(key))
                    {
                        map[key] = tag.Description ?? string.Empty;
                    }
                    else
                    {
                        // If duplicate keys occur, append with a separator
                        map[key] = map[key] + "; " + (tag.Description ?? string.Empty);
                    }
                }
            }

            return map;
        }
    }
}