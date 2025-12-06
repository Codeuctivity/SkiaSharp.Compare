using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SkiaSharpCompare.Cli.Tests
{
    public class SkiaSharpCompareCliIntegrationTests
    {
        private const int ProcessTimeoutMs = 30_000;

        [Test]
        public async Task SkiaSharpCompareCli_ShouldExitSuccessfully()
        {
            // Static precompiled CLI project directory (relative to test assembly output).
            var cliProjectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "SkiaSharpCompare.Cli"));
            Assert.That(Directory.Exists(cliProjectDir), Is.True, $"Could not locate CLI project directory at '{cliProjectDir}'. Ensure the solution layout is unchanged and the path is valid.");

            // Find a JPG file in the repository to pass to the CLI.
            var jpgFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "SkiaSharpCompareTestNunit", "TestData", "imageWithGpsMetadata.jpg"));
            Assert.That(jpgFile, Is.Not.Null, "Could not locate any .jpg file in the repository to use for the integration test.");

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run -- \"{jpgFile}\" --meta",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = false,
                WorkingDirectory = cliProjectDir
            };

            using var process = new Process { StartInfo = startInfo };

            process.Start();

            var stdOutTask = process.StandardOutput.ReadToEndAsync();
            var stdErrTask = process.StandardError.ReadToEndAsync();

            var exited = await Task.Run(() => process.WaitForExit(ProcessTimeoutMs));
            if (!exited)
            {
                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch
                {
                    // Best effort
                }

                Assert.Fail($"Process did not exit within {ProcessTimeoutMs} ms. StdOut:{Environment.NewLine}{await stdOutTask}{Environment.NewLine}StdErr:{Environment.NewLine}{await stdErrTask}");
            }

            var stdout = await stdOutTask;
            var stderr = await stdErrTask;

            Assert.That(process.ExitCode, Is.EqualTo(0), $"Process exited with non-zero exit code {process.ExitCode}.{Environment.NewLine}StdOut:{Environment.NewLine}{stdout}{Environment.NewLine}StdErr:{Environment.NewLine}{stderr}");
            Assert.That(stdout, Does.Contain("GPS:GPS Altitude: 201"));
        }
    }
}