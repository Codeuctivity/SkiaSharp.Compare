using System.Collections.Generic;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// Dto - outcome of compared images
    /// </summary>
    /// <remarks>
    /// ctor for CompareResult
    /// </remarks>
    /// <param name="meanError">Mean error</param>
    /// <param name="absoluteError">Absolute error</param>
    /// <param name="pixelErrorCount">Number of pixels that differ between images</param>
    /// <param name="pixelErrorPercentage">Percentage of pixels that differ between images</param>
    /// <param name="metadataDifference">Metadata that differ between images</param>
    public class CompareResult(int absoluteError, double meanError, int pixelErrorCount, double pixelErrorPercentage, Dictionary<string, (string? ValueA, string? ValueB)>? metadataDifference = null) : ICompareResult
    {
        /// <summary>
        /// Mean pixel error of absolute pixel error
        /// </summary>
        /// <value>0-765</value>
        public double MeanError { get; } = meanError;

        /// <summary>
        /// Absolute pixel error, counts each color channel on every pixel the delta
        /// </summary>
        public int AbsoluteError { get; } = absoluteError;

        /// <summary>
        /// Number of pixels that differ between images
        /// </summary>
        public int PixelErrorCount { get; } = pixelErrorCount;

        /// <summary>
        /// Percentage of pixels that differ between images
        /// </summary>
        /// <value>0-100.0</value>
        public double PixelErrorPercentage { get; } = pixelErrorPercentage;

        /// <summary>
        /// Gets a collection of metadata keys and their differing values between two sources.
        /// </summary>
        /// <remarks>Each entry in the dictionary represents a metadata key for which the values differ.
        /// The tuple contains the value from the first source and the value from the second source, respectively. If a
        /// value is null, the key may be missing from that source.</remarks>
        public Dictionary<string, (string? ValueA, string? ValueB)>? MetadataDifferences { get; } = metadataDifference;
    }
}