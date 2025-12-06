namespace Codeuctivity.SkiaSharpCompare.Cli
{
    /// <summary>
    /// Summary of comparing two directories.
    /// </summary>
    public sealed class DirectoryCompareSummary
    {
        public Dictionary<string, global::Codeuctivity.SkiaSharpCompare.ICompareResult?> MatchedResults { get; init; } = new();
        public List<string> OnlyInA { get; init; } = new();
        public List<string> OnlyInB { get; init; } = new();
        public List<string> UnsupportedFiles { get; init; } = new();
    }
}