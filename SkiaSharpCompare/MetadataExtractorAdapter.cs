using MetadataExtractor;
using System.Collections.Generic;
using System.IO;

namespace SkiaSharpCompare.Metadata
{
    /// <summary>
    /// Provides static methods for extracting image metadata and returning it as a normalized dictionary of tag names
    /// and values.
    /// </summary>
    /// <remarks>This class serves as an adapter for reading metadata from image files or streams using the
    /// underlying MetadataExtractor library. All returned tag keys are normalized in the format "Directory:TagName" for
    /// consistency. The class is thread-safe as it contains only stateless static methods.</remarks>
    public static class MetadataExtractorAdapter
    {
        /// <summary>
        /// Extracts metadata tags from the specified image stream and returns them as a normalized, case-insensitive
        /// dictionary.
        /// </summary>
        /// <remarks>The caller is responsible for ensuring that the provided stream is positioned at the
        /// start of the image data and remains open for the duration of the operation. The returned dictionary uses
        /// case-insensitive keys. If the stream is not seekable, consider passing a seekable copy to avoid
        /// errors.</remarks>
        /// <param name="imageStream">A seekable stream containing image data from which to read metadata. The stream must support reading and
        /// seeking.</param>
        /// <returns>A read-only dictionary containing metadata tag names and their corresponding values extracted from the
        /// image. The dictionary is empty if no metadata is found.</returns>
        public static IReadOnlyDictionary<string, string> Extract(Stream imageStream)
        {
            var map = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            // MetadataExtractor reads from stream
            var directories = ImageMetadataReader.ReadMetadata(imageStream);
            return CreateResult(map, directories);
        }

        /// <summary>
        /// Extracts metadata from the specified image file and returns it as a read-only dictionary of tag names and
        /// values.
        /// </summary>
        /// <param name="imagePath">The path to the image file from which to extract metadata. Cannot be null or empty.</param>
        /// <returns>A read-only dictionary containing metadata tag names and their corresponding values. The dictionary is empty
        /// if no metadata is found.</returns>
        public static IReadOnlyDictionary<string, string> Extract(string imagePath)
        {
            var map = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            // MetadataExtractor reads from stream
            var directories = ImageMetadataReader.ReadMetadata(imagePath);
            return CreateResult(map, directories);
        }

        private static Dictionary<string, string> CreateResult(Dictionary<string, string> map, IReadOnlyList<MetadataExtractor.Directory> directories)
        {
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    // Normalize key as "Directory:TagName"
                    var key = $"{directory.Name}:{tag.Name}";
                    if (map.TryGetValue(key, out var existingValue))
                    {
                        // If duplicate keys occur, append with a separator
                        map[key] = existingValue + "; " + (tag.Description ?? string.Empty);
                    }
                    else
                    {
                        map[key] = tag.Description ?? string.Empty;
                    }
                }
            }

            return map;
        }
    }
}