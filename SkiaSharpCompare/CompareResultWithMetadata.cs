using System.Collections.Generic;

namespace Codeuctivity.SkiaSharpCompare
{
    internal class CompareResultWithMetadata : ICompareResult
    {
        public CompareResultWithMetadata(ICompareResult compareResult, Dictionary<string, (string? ValueA, string? ValueB)>? metadataDiff)
        {
            MeanError = compareResult.MeanError;
            AbsoluteError = compareResult.AbsoluteError;
            PixelErrorCount = compareResult.PixelErrorCount;
            PixelErrorPercentage = compareResult.PixelErrorPercentage;
            MetadataDifferences = metadataDiff;
        }

        public double MeanError { get; }
        public int AbsoluteError { get; }
        public int PixelErrorCount { get; }
        public double PixelErrorPercentage { get; }
        public Dictionary<string, (string? ValueA, string? ValueB)>? MetadataDifferences { get; }
    }
}