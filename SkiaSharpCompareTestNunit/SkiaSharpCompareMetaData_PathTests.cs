using Codeuctivity.SkiaSharpCompare;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkiaSharpCompareTestNunit
{
    internal class SkiaSharpCompareMetaData_PathTests
    {
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, true, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, false, true)]
        [TestCase(TestFiles.imageWithoutGpsMetadata, TestFiles.imageWithGpsMetadata, true, false)]
        [TestCase(TestFiles.imageWithoutGpsMetadata, TestFiles.imageWithGpsMetadata, false, true)]
        public void ImagesAreEqual_SamePixelComparedByMetadataShouldReturnResult(string pic1Path, string pic2Path, bool shouldCompareMetadata, bool expectOutcomeComparisonOfImage)
        {
            var pic1 = Path.Combine(AppContext.BaseDirectory, pic1Path);
            var pic2 = Path.Combine(AppContext.BaseDirectory, pic2Path);

            var sut = new ImageCompare(compareMetadata: shouldCompareMetadata);
            Assert.That(sut.ImagesAreEqual(pic1, pic2), Is.EqualTo(expectOutcomeComparisonOfImage));
        }

        [Test]
        public void CalcDiff_SamePixelComparedByMetadataShouldReturnResult_Null()
        {
            var pic1 = Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithGpsMetadata);
            var pic2 = Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata);

            var sut = new ImageCompare(compareMetadata: false);
            var actual = sut.CalcDiff(pic1, pic2);

            Assert.That(actual.MetadataDifferences, Is.Null);
            Assert.That(actual.PixelErrorCount, Is.Zero);
        }

        [Test]
        public void CalcDiff_SamePixelComparedByMetadataShouldReturnEmptyResult()
        {
            var pic1 = Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata);
            var pic2 = Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata);

            var sut = new ImageCompare(compareMetadata: true);
            var actual = sut.CalcDiff(pic1, pic2);

            Assert.That(actual.MetadataDifferences, Is.Empty);
            Assert.That(actual.PixelErrorCount, Is.Zero);
        }

        [Test]
        [SetCulture("en-US")]
        public void CalcDiff_SamePixelComparedByMetadataShouldReturnCollectionOfMetadataThatDiffers()
        {
            var pic1 = Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata);
            var pic2 = Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithGpsMetadata);

            var sut = new ImageCompare(compareMetadata: true);
            var actual = sut.CalcDiff(pic1, pic2);

            // Output actual and expected to test run logs for easier diagnosis
            TestContext.WriteLine($"Actual image: {pic1}");
            TestContext.WriteLine($"Expected image: {pic2}");
            TestContext.WriteLine($"Actual MetadataDifferences: {FormatMetadata(actual.MetadataDifferences)}");

            var expected = new Dictionary<string, (string? ValueA, string? ValueB)>
            {
                { "File:File Name", (Path.GetFileName(pic1), Path.GetFileName(pic2)) },
                { "GPS:GPS Altitude", ("0 metres", "201.62 metres") },
                { "GPS:GPS Date Stamp", ("", "2025:01:03") },
                { "GPS:GPS Latitude", ("0° 0' 0\"", "48° 12' 7.17\"") },
                { "GPS:GPS Latitude Ref", ("", "N") },
                { "GPS:GPS Longitude", ("0° 0' 0\"", "16° 24' 7.53\"") },
                { "GPS:GPS Longitude Ref", ("", "E") },
                { "GPS:GPS Processing Method", ("", "GPS") },
                { "GPS:GPS Time-Stamp", ("00:00:00.000 UTC", "14:41:20.000 UTC") }
            };

            TestContext.WriteLine($"Expected MetadataDifferences: {FormatMetadata(expected)}");

            Assert.That(actual.MetadataDifferences, Is.EqualTo(expected));
            Assert.That(actual.PixelErrorCount, Is.Zero);
        }

        private static string FormatMetadata(Dictionary<string, (string? ValueA, string? ValueB)>? metadata)
        {
            if (metadata is null)
            {
                return "null";
            }

            return string.Join("; ", metadata.Select(kvp => $"{kvp.Key}=[{kvp.Value.ValueA ?? ""} | {kvp.Value.ValueB ?? ""}]"));
        }
    }
}