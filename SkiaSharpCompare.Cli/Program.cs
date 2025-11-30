using System;
using System.IO;
using Codeuctivity.SkiaSharpCompare;
using SkiaSharpCompare.Metadata;

namespace Codeuctivity.SkiaSharpCompare.Cli
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args is null || args.Length < 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  dotnet run -- <fileA> <fileB>");
                Console.WriteLine("  dotnet run -- <directoryA> <directoryB> --dir");
                Console.WriteLine("  dotnet run -- <file> --meta    # show metadata for a file");
                return 2;
            }

            try
            {
                // If user requested metadata display
                if (Array.IndexOf(args, "--meta") >= 0)
                {
                    var fileArg = args[0];
                    var absolute = Path.GetFullPath(fileArg);
                    if (!File.Exists(absolute))
                    {
                        Console.Error.WriteLine($"File not found: {absolute}");
                        return 4;
                    }

                    try
                    {
                        var meta = MetadataExtractorAdapter.Extract(absolute);
                        Console.WriteLine($"Metadata for: {absolute}");
                        if (meta.Count == 0)
                        {
                            Console.WriteLine("  (no metadata found)");
                        }
                        else
                        {
                            foreach (var kv in meta.OrderBy(k => k.Key))
                            {
                                Console.WriteLine($"  {kv.Key}: {kv.Value}");
                            }
                        }

                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract metadata: {ex.Message}");
                        return 5;
                    }
                }

                if (args.Length >= 3 && args[2] == "--dir")
                {
                    var summary = CliRunner.CompareDirectories(args[0], args[1]);
                    CliRunner.PrintDirectorySummary(summary, Console.Out);
                }
                else if (args.Length >= 2)
                {
                    var resultInfo = CliRunner.CompareFiles(args[0], args[1], compareMetadata: true);
                    CliRunner.PrintCompareResult(resultInfo, Console.Out, Path.GetFileName(args[0]), Path.GetFileName(args[1]));
                }
                else
                {
                    Console.WriteLine("Insufficient arguments. See usage.");
                    return 2;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fatal error: {ex.Message}");
                return 3;
            }
        }
    }
}