namespace Codeuctivity.SkiaSharpCompare.Cli
{
    /// <summary>
    /// Helper that exposes programmatic entry points for the console and tests.
    /// </summary>
    public static class CliRunner
    {
        /// <summary>
        /// Result wrapper for a single file comparison.
        /// </summary>
        public sealed class FileCompareInfo
        {
            public ICompareResult? Result { get; init; }
            public bool Unsupported { get; init; }
            public string? ErrorMessage { get; init; }
        }

        /// <summary>
        /// Compare two image files. If an image cannot be loaded or compared, <see cref="FileCompareInfo.Unsupported"/>
        /// will be true and <see cref="FileCompareInfo.Result"/> will be null.
        /// </summary>
        public static FileCompareInfo CompareFiles(string pathA, string pathB, bool compareMetadata = true)
        {
            if (string.IsNullOrWhiteSpace(pathA))
            {
                throw new ArgumentException("PathA is required", nameof(pathA));
            }

            if (string.IsNullOrWhiteSpace(pathB))
            {
                throw new ArgumentException("PathB is required", nameof(pathB));
            }

            var absoluteA = Path.GetFullPath(pathA);
            var absoluteB = Path.GetFullPath(pathB);

            var comparer = new ImageCompare(compareMetadata: compareMetadata);

            try
            {
                var result = comparer.CalcDiff(absoluteA, absoluteB);
                return new FileCompareInfo
                {
                    Result = result,
                    Unsupported = false,
                    ErrorMessage = null
                };
            }
            catch (Exception ex)
            {
                return new FileCompareInfo
                {
                    Result = null,
                    Unsupported = true,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Compare two directories. Matches files by filename. Returns compare results for matched names,
        /// lists of files existing only in one directory and a list of files which failed to be compared (unsupported).
        /// </summary>
        public static DirectoryCompareSummary CompareDirectories(string directoryA, string directoryB, bool compareMetadata = true)
        {
            if (string.IsNullOrWhiteSpace(directoryA))
            {
                throw new ArgumentException("directoryA is required", nameof(directoryA));
            }

            if (string.IsNullOrWhiteSpace(directoryB))
            {
                throw new ArgumentException("directoryB is required", nameof(directoryB));
            }

            var dirA = Path.GetFullPath(directoryA);
            var dirB = Path.GetFullPath(directoryB);

            if (!Directory.Exists(dirA))
            {
                throw new DirectoryNotFoundException(dirA);
            }

            if (!Directory.Exists(dirB))
            {
                throw new DirectoryNotFoundException(dirB);
            }

            var filesA = Directory.GetFiles(dirA).Select(Path.GetFileName).Where(n => n != null).Cast<string>().ToHashSet(StringComparer.OrdinalIgnoreCase);
            var filesB = Directory.GetFiles(dirB).Select(Path.GetFileName).Where(n => n != null).Cast<string>().ToHashSet(StringComparer.OrdinalIgnoreCase);

            var onlyInA = filesA.Except(filesB, StringComparer.OrdinalIgnoreCase).OrderBy(n => n).ToList();
            var onlyInB = filesB.Except(filesA, StringComparer.OrdinalIgnoreCase).OrderBy(n => n).ToList();
            var matched = filesA.Intersect(filesB, StringComparer.OrdinalIgnoreCase).OrderBy(n => n).ToList();

            var matchedResults = new Dictionary<string, ICompareResult?>(StringComparer.OrdinalIgnoreCase);
            var unsupported = new List<string>();

            foreach (var fileName in matched)
            {
                var pathA = Path.Combine(dirA, fileName);
                var pathB = Path.Combine(dirB, fileName);

                var info = CompareFiles(pathA, pathB, compareMetadata: compareMetadata);
                if (info.Unsupported)
                {
                    matchedResults[fileName] = null;
                    unsupported.Add(fileName);
                }
                else
                {
                    matchedResults[fileName] = info.Result;
                }
            }

            return new DirectoryCompareSummary
            {
                MatchedResults = matchedResults,
                OnlyInA = onlyInA,
                OnlyInB = onlyInB,
                UnsupportedFiles = unsupported
            };
        }

        /// <summary>
        /// Print a file comparison result to the provided text writer.
        /// </summary>
        public static void PrintCompareResult(FileCompareInfo info, TextWriter writer, string? nameA = null, string? nameB = null)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            nameA ??= "A";
            nameB ??= "B";

            writer.WriteLine($"Comparing: {nameA} <> {nameB}");

            if (info.Unsupported)
            {
                writer.WriteLine($"  Unsupported or failed to compare: {info.ErrorMessage}");
                return;
            }

            var result = info.Result!;
            writer.WriteLine($"  PixelErrorCount: {result.PixelErrorCount}");
            writer.WriteLine($"  PixelErrorPercentage: {result.PixelErrorPercentage:F4}");
            writer.WriteLine($"  AbsoluteError: {result.AbsoluteError}");
            writer.WriteLine($"  MeanError: {result.MeanError:F4}");

            if (result.MetadataDifferences is null)
            {
                writer.WriteLine("  Metadata comparison disabled (null).");
            }
            else if (result.MetadataDifferences.Count == 0)
            {
                writer.WriteLine("  Metadata: no differences.");
            }
            else
            {
                writer.WriteLine("  Metadata differences:");
                foreach (var kvp in result.MetadataDifferences)
                {
                    writer.WriteLine($"    {kvp.Key}: (A: {kvp.Value.ValueA ?? string.Empty}, B: {kvp.Value.ValueB ?? string.Empty})");
                }
            }
        }

        /// <summary>
        /// Print directory compare summary to the writer.
        /// </summary>
        public static void PrintDirectorySummary(DirectoryCompareSummary summary, TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteLine("Directory comparison summary:");
            writer.WriteLine();

            writer.WriteLine("Matched files:");
            if (summary.MatchedResults.Count == 0)
            {
                writer.WriteLine("  (none)");
            }
            else
            {
                foreach (var kvp in summary.MatchedResults.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
                {
                    writer.WriteLine($"  {kvp.Key}:");
                    if (kvp.Value is null)
                    {
                        writer.WriteLine("    Unsupported or failed to compare");
                    }
                    else
                    {
                        writer.WriteLine($"    PixelErrorCount: {kvp.Value.PixelErrorCount}, PixelErrorPercentage: {kvp.Value.PixelErrorPercentage:F4}");
                    }
                }
            }

            writer.WriteLine();
            writer.WriteLine("Only in A:");
            if (summary.OnlyInA.Count == 0)
            {
                writer.WriteLine("  (none)");
            }
            else
            {
                foreach (var n in summary.OnlyInA)
                {
                    writer.WriteLine($"  {n}");
                }
            }

            writer.WriteLine();
            writer.WriteLine("Only in B:");
            if (summary.OnlyInB.Count == 0)
            {
                writer.WriteLine("  (none)");
            }
            else
            {
                foreach (var n in summary.OnlyInB)
                {
                    writer.WriteLine($"  {n}");
                }
            }

            writer.WriteLine();
            writer.WriteLine("Unsupported files:");
            if (summary.UnsupportedFiles.Count == 0)
            {
                writer.WriteLine("  (none)");
            }
            else
            {
                foreach (var n in summary.UnsupportedFiles)
                {
                    writer.WriteLine($"  {n}");
                }
            }
        }
    }
}