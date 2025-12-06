using Codeuctivity.SkiaSharpCompare.Cli;
using NUnit.Framework;
using System;
using System.IO;

namespace SkiaSharpCompare.Cli.Tests
{
    [TestFixture]
    public class SkiaSharpCompareCliTests
    {
        [Test]
        public void CompareFiles_NonImageFiles_AreReportedUnsupported()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);

            try
            {
                var a = Path.Combine(dir, "a.txt");
                var b = Path.Combine(dir, "b.txt");

                File.WriteAllText(a, "not an image");
                File.WriteAllText(b, "not an image");

                var info = CliRunner.CompareFiles(a, b);
                Assert.That(info.Unsupported, Is.True);
                Assert.That(info.Result, Is.Null);
                Assert.That(info.ErrorMessage, Is.Not.Null.And.Not.Empty);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Test]
        public void CompareDirectories_MatchesMissingAndUnsupported()
        {
            var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            var dirA = Path.Combine(root, "A");
            var dirB = Path.Combine(root, "B");
            Directory.CreateDirectory(dirA);
            Directory.CreateDirectory(dirB);

            try
            {
                // file only in A
                var onlyA = Path.Combine(dirA, "onlyA.txt");
                File.WriteAllText(onlyA, "not image");

                // file only in B
                var onlyB = Path.Combine(dirB, "onlyB.txt");
                File.WriteAllText(onlyB, "not image");

                // common file (but not a valid image)
                var commonA = Path.Combine(dirA, "common.jpg");
                var commonB = Path.Combine(dirB, "common.jpg");
                File.WriteAllText(commonA, "not an image");
                File.WriteAllText(commonB, "not an image");

                var summary = CliRunner.CompareDirectories(dirA, dirB);

                Assert.That(summary.OnlyInA, Has.Member("onlyA.txt"));
                Assert.That(summary.OnlyInB, Has.Member("onlyB.txt"));
                Assert.That(summary.MatchedResults.Keys, Has.Member("common.jpg"));
                Assert.That(summary.MatchedResults["common.jpg"], Is.Null);
                Assert.That(summary.UnsupportedFiles, Has.Member("common.jpg"));
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }
    }
}