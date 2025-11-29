using System.Collections.Generic;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// Dto - of compared images
    /// </summary>
    public interface ICompareResult
    {
        /// <summary>
        /// Mean relative pixel error
        /// </summary>
        double MeanError { get; }

        /// <summary>
        /// Absolute pixel error
        /// </summary>
        int AbsoluteError { get; }

        /// <summary>
        /// Number of pixels that differ between images
        /// </summary>
        int PixelErrorCount { get; }

        /// <summary>
        /// Percentage of pixels that differ between images
        /// </summary>
        double PixelErrorPercentage { get; }

        /// <summary>
        /// Gets a collection of metadata keys and their differing values between two sources.
        /// </summary>
        /// <remarks>Each entry in the dictionary represents a metadata key for which the values differ
        /// between the two compared sources. The tuple contains the value from the first source and the value from the
        /// second source. If there are no differences or metadata is unavailable, the property returns null.</remarks>
        Dictionary<string, (string? ValueA, string? ValueB)>? MetadataDifferences { get; }
    }
}