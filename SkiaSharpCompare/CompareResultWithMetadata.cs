using System.Collections.Generic;

namespace Codeuctivity.SkiaSharpCompare
{
    internal class CompareResultWithMetadata(ICompareResult compareResult, Dictionary<string, (string? ValueA, string? ValueB)>? metadataDiff) : ICompareResult
    {
        public double MeanError { get; } = compareResult.MeanError;
        public int AbsoluteError { get; } = compareResult.AbsoluteError;
        public int PixelErrorCount { get; } = compareResult.PixelErrorCount;
        public double PixelErrorPercentage { get; } = compareResult.PixelErrorPercentage;
        public Dictionary<string, (string? ValueA, string? ValueB)>? MetadataDifferences { get; } = metadataDiff;
    }
}