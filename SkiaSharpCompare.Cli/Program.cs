using System;
using System.IO;
using Codeuctivity.SkiaSharpCompare;

namespace Codeuctivity.SkiaSharpCompare.Cli
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args is null || args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  dotnet run -- <fileA> <fileB>");
                Console.WriteLine("  dotnet run -- <directoryA> <directoryB> --dir");
                return 2;
            }

            try
            {
                if (args.Length >= 3 && args[2] == "--dir")
                {
                    var summary = CliRunner.CompareDirectories(args[0], args[1]);
                    CliRunner.PrintDirectorySummary(summary, Console.Out);
                }
                else
                {
                    var resultInfo = CliRunner.CompareFiles(args[0], args[1], compareMetadata: true);
                    CliRunner.PrintCompareResult(resultInfo, Console.Out, Path.GetFileName(args[0]), Path.GetFileName(args[1]));
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