using Codeuctivity.SkiaSharpCompare;
using NUnit.Framework;
using SkiaSharp;
using System;
using System.IO;

namespace SkiaSharpCompareTestNunit
{
    public class IntegrationTest
    {
        private const string jpg0Rgb24 = "../../../TestData/Calc0.jpg";
        private const string jpg1Rgb24 = "../../../TestData/Calc1.jpg";
        private const string png0Rgba32 = "../../../TestData/Calc0.png";
        private const string png1Rgba32 = "../../../TestData/Calc1.png";
        private const string pngBlack2x2px = "../../../TestData/Black.png";
        private const string pngBlack4x4px = "../../../TestData/BlackDoubleSize.png";
        private const string pngWhite2x2px = "../../../TestData/White.png";
        private const string pngTransparent2x2px = "../../../TestData/pngTransparent2x2px.png";
        private const string renderedForm1 = "../../../TestData/HC007-Test-02-3-OxPt.html1.png";
        private const string renderedForm2 = "../../../TestData/HC007-Test-02-3-OxPt.html2.png";
        private const string colorShift1 = "../../../TestData/ColorShift1.png";
        private const string colorShift2 = "../../../TestData/ColorShift2.png";

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack2x2px, false)]
        public void ShouldVerifyThatImagesFromFilePathSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack2x2px, false)]
        public void ShouldVerifyThatImagesSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, png0Rgba32, true)]
        [TestCase(png0Rgba32, jpg0Rgb24, true)]
        [TestCase(png0Rgba32, jpg1Rgb24, true)]
        [TestCase(png0Rgba32, pngBlack2x2px, false)]
        public void ShouldVerifyThatImageStreamsSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.True);
        }

        [Test]
        [TestCase(pngBlack2x2px, pngBlack2x2px, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, ResizeOption.DontResize, false)]
        [TestCase(colorShift1, colorShift2, ResizeOption.DontResize, false)]
        public void ShouldVerifyThatImagesWithDifferentSizeAreEqual(string pathActual, string pathExpected, ResizeOption resizeOption, bool expectedResult)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected, resizeOption), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageStreamsAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(Compare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatSkiaSharpImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(Compare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, null, 0)]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.DontResize, 0)]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.Resize, 0)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize, 0)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize, 0)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, 0)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize, 0)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d, ResizeOption.DontResize, 0)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize, 0)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize, 0)]
        [TestCase(renderedForm1, renderedForm2, 49267623, 60.794204096742348d, 174178, 21.49284304047384d, ResizeOption.Resize, 0)]
        [TestCase(renderedForm2, renderedForm1, 49267623, 60.794204096742348d, 174178, 21.49284304047384d, ResizeOption.Resize, 0)]
        [TestCase(colorShift1, colorShift2, 117896, 3.437201166180758d, 30398, 88.623906705539355d, ResizeOption.DontResize, 0)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.DontResize, 15)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int pixelColorShiftTolerance)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var diff = Compare.CalcDiff(absolutePathPic1, absolutePathPic2, resizeOption, pixelColorShiftTolerance);

            Console.WriteLine($"PixelErrorCount: {diff.PixelErrorCount}");
            Console.WriteLine($"PixelErrorPercentage: {diff.PixelErrorPercentage}");
            Console.WriteLine($"AbsoluteError: {diff.AbsoluteError}");
            Console.WriteLine($"MeanError: {diff.MeanError}");

            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(pngBlack2x2px, pngBlack4x4px)]
        [TestCase(pngBlack4x4px, pngWhite2x2px)]
        public void ShouldVerifyThatCalcDiffThrowsOnDifferentImageSizes(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(
                   () => Compare.CalcDiff(absolutePathPic1, absolutePathPic2, ResizeOption.DontResize));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, null)]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.Resize)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d, ResizeOption.DontResize)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 3060, 765, 4, 100.0d, ResizeOption.DontResize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize)]
        public void ShouldVerifyThatImageStreamsAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var diff = Compare.CalcDiff(pic1, pic2, resizeOption);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, null)]
        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngWhite2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(renderedForm1, renderedForm2, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(renderedForm2, renderedForm1, 0, 0, 0, 0, ResizeOption.Resize)]
        public void Diffmask(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMask = Path.GetTempFileName() + "differenceMask.png";

            using (var fileStreamDifferenceMask = File.Create(differenceMask))
            using (var maskImage = Compare.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2, resizeOption))
            {
                SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = Compare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask, resizeOption);
            File.Delete(differenceMask);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        private void SaveAsPng(SKBitmap maskImage, FileStream fileStreamDifferenceMask)
        {
            var encodedData = maskImage.Encode(SKEncodedImageFormat.Png, 100);
            encodedData.SaveTo(fileStreamDifferenceMask);
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        public void ShouldCalcDiffMaskSKBitmapAndUseOutcome(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPicPath = Path.GetTempFileName() + "differenceMask.png";

            using var absolutePic1 = SKBitmap.Decode(absolutePathPic1);
            using var absolutePic2 = SKBitmap.Decode(absolutePathPic2);

            using (var fileStreamDifferenceMask = File.Create(differenceMaskPicPath))
            using (var maskImage = Compare.CalcDiffMaskImage(absolutePic1, absolutePic2, resizeOption))
            {
                SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            using var differenceMaskPic = SKBitmap.Decode(differenceMaskPicPath);
            var maskedDiff = Compare.CalcDiff(absolutePic1, absolutePic2, differenceMaskPic, resizeOption);
            File.Delete(differenceMaskPicPath);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(pngWhite2x2px, pngBlack2x2px, pngTransparent2x2px, 765, 12240, 16, 100d, ResizeOption.Resize, 0)]
        [TestCase(pngWhite2x2px, pngBlack2x2px, pngBlack4x4px, 765, 12240, 16, 100d, ResizeOption.Resize, 0)]
        [TestCase(pngBlack2x2px, pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(pngBlack4x4px, pngBlack2x2px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(colorShift1, colorShift1, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(colorShift1, colorShift2, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 20)]
        public void ShouldUseDiffMask(string pathPic1, string pathPic2, string pathPic3, double expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int pixelColorShiftTolerance)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = SKBitmap.Decode(absolutePathPic1);
            using var pic2 = SKBitmap.Decode(absolutePathPic2);
            using var maskPic = SKBitmap.Decode(differenceMaskPic);

            var maskedDiff = Compare.CalcDiff(pic1, pic2, maskPic, resizeOption, pixelColorShiftTolerance);

            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(pngBlack2x2px, pngBlack2x2px, pngBlack4x4px)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, pngBlack2x2px)]
        [TestCase(pngBlack4x4px, pngBlack2x2px, pngBlack2x2px)]
        public void ShouldThrowUsingInvalidImageDimensionsDiffMask(string pathPic1, string pathPic2, string pathPic3)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = SKBitmap.Decode(absolutePathPic1);
            using var pic2 = SKBitmap.Decode(absolutePathPic2);
            using var maskPic = SKBitmap.Decode(differenceMaskPic);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => Compare.CalcDiff(pic1, pic2, maskPic, ResizeOption.DontResize));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        public void DiffMaskStreams(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using var maskImage = Compare.CalcDiffMaskImage(pic1, pic2);

            pic1.Position = 0;
            pic2.Position = 0;

            var maskedDiff = Compare.CalcDiff(pic1, pic2, maskImage);
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, 0)]
        [TestCase(colorShift1, colorShift2, 20)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByFilePath_NoDifferences(string image1RelativePath, string image2RelativePath, int pixelColorShiftTolerance)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = Compare.CalcDiffMaskImage(image1Path, image2Path, ResizeOption.DontResize, pixelColorShiftTolerance);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
            }

            using var diffMask2Image = Compare.CalcDiffMaskImage(image1Path, image2Path, diffMask1Path);

            using (var diffMask2Stream = File.Create(diffMask1Path))
            {
                ImageExtensions.SaveAsPng(diffMask2Image, diffMask2Stream);
            }

            Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);

            File.Delete(diffMask1Path);
        }

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByStream_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using var image1Stream = new FileStream(image1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var image2Stream = new FileStream(image2Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = Compare.CalcDiffMaskImage(image1Stream, image2Stream);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
            }

            image1Stream.Position = 0;
            image2Stream.Position = 0;

            using (var diffMask1Stream = new FileStream(diffMask1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                diffMask1Stream.Position = 0;
                using var diffMask2Image = Compare.CalcDiffMaskImage(image1Stream, image2Stream, diffMask1Stream);
                Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);
            }

            File.Delete(diffMask1Path);
        }

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByImage_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);

            using var image1 = SKBitmap.Decode(image1Path);
            using var image2 = SKBitmap.Decode(image2Path);

            using var diffMask1Image = Compare.CalcDiffMaskImage(image1, image2);

            using var diffMask2Image = Compare.CalcDiffMaskImage(image1, image2, diffMask1Image);

            Assert.That(IsImageEntirelyBlack(diffMask2Image), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        [TestCase(colorShift1, colorShift2)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.False);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        [TestCase(colorShift1, colorShift2)]
        public void ShouldVerifyThatImageStreamAreNotEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(Compare.ImagesAreEqual(actual, expected), Is.False);
        }

        [TestCase(png0Rgba32, pngBlack2x2px)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => Compare.CalcDiff(absolutePathPic1, absolutePathPic2));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [TestCase(png0Rgba32, png0Rgba32, pngBlack2x2px)]
        [TestCase(png0Rgba32, pngBlack2x2px, png0Rgba32)]
        [TestCase(pngBlack2x2px, png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2, string pathPic3)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var absolutePathPic3 = Path.Combine(AppContext.BaseDirectory, pathPic3);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => Compare.CalcDiff(absolutePathPic1, absolutePathPic2, absolutePathPic3));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        private static bool IsImageEntirelyBlack(SKBitmap image)
        {
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var sKColor = image.GetPixel(x, y);
                    if (sKColor.Red != 0 || sKColor.Green != 0 || sKColor.Blue != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}