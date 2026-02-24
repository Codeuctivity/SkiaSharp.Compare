using Codeuctivity.SkiaSharpCompare;
using NUnit.Framework;
using SkiaSharp;
using System;
using System.IO;

namespace SkiaSharpCompareTestNunit
{
    public class SkiaSharpStaticCompareTests
    {
        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg0Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.jpg0Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.jpg1Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.pngBlack2x2px, false)]
        public void ShouldVerifyThatImagesFromFilePathSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg0Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.jpg0Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.jpg1Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.pngBlack2x2px, false)]
        public void ShouldVerifyThatImagesSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg0Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.jpg0Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.jpg1Rgb24, true)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.pngBlack2x2px, false)]
        public void ShouldVerifyThatImageStreamsSizeAreEqual(string pathActual, string pathExpected, bool expectedOutcome)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg0Rgb24, 0)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, 0)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, 15)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected, int colorShiftTolerance)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected, pixelColorShiftTolerance: colorShiftTolerance), Is.True);
        }

        [Test]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px, ResizeOption.Resize, true)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, ResizeOption.Resize, true)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, ResizeOption.DontResize, false)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, ResizeOption.DontResize, false)]
        public void ShouldVerifyThatImagesWithDifferentSizeAreEqual(string pathActual, string pathExpected, ResizeOption resizeOption, bool expectedResult)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected, resizeOption), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg0Rgb24)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32)]
        public void ShouldVerifyThatImageStreamsAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(Compare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg0Rgb24)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32)]
        public void ShouldVerifyThatSkiaSharpImagesAreEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(Compare.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, null, 0)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.png1Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.pngBlack4x4px, TestFiles.pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.renderedForm1, TestFiles.renderedForm2, 49267623, 60.794204096742348d, 174178, 21.49284304047384d, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.renderedForm2, TestFiles.renderedForm1, 49267623, 60.794204096742348d, 174178, 21.49284304047384d, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, 117896, 3.437201166180758d, 30398, 88.623906705539355d, ResizeOption.DontResize, 0)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, 0, 0, 0, 0, ResizeOption.DontResize, 15)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int colorShiftTolerance)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var diff = Compare.CalcDiff(absolutePathPic1, absolutePathPic2, resizeOption, colorShiftTolerance);

            Console.WriteLine($"PixelErrorCount: {diff.PixelErrorCount}");
            Console.WriteLine($"PixelErrorPercentage: {diff.PixelErrorPercentage}");
            Console.WriteLine($"AbsoluteError: {diff.AbsoluteError}");
            Console.WriteLine($"MeanError: {diff.MeanError}");

            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px)]
        [TestCase(TestFiles.pngBlack4x4px, TestFiles.pngWhite2x2px)]
        public void ShouldVerifyThatCalcDiffThrowsOnDifferentImageSizes(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(
                   () => Compare.CalcDiff(absolutePathPic1, absolutePathPic2, ResizeOption.DontResize));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, null)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.DontResize)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.Resize)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize)]
        [TestCase(TestFiles.png1Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d, ResizeOption.DontResize)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngWhite2x2px, 3060, 765, 4, 100.0d, ResizeOption.DontResize)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.pngBlack4x4px, TestFiles.pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize)]
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

        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0, null)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.pngWhite2x2px, TestFiles.pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.pngBlack4x4px, TestFiles.pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.renderedForm1, TestFiles.renderedForm2, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.renderedForm2, TestFiles.renderedForm1, 0, 0, 0, 0, ResizeOption.Resize)]
        public void DiffMask(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
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

        private static void SaveAsPng(SKBitmap maskImage, FileStream fileStreamDifferenceMask)
        {
            var encodedData = maskImage.Encode(SKEncodedImageFormat.Png, 100);
            encodedData.SaveTo(fileStreamDifferenceMask);
        }

        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
        public void ShouldCalcDiffMaskSKBitmap(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var absolutePic1 = SKBitmap.Decode(absolutePathPic1);
            using var absolutePic2 = SKBitmap.Decode(absolutePathPic2);

            using (var maskImage = Compare.CalcDiffMaskImage(absolutePic1, absolutePic2, resizeOption))
            {
                Assert.That(ImageExtensions.IsImageEntirelyBlack(maskImage, TransparencyOptions.IgnoreAlphaChannel));
            }
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg1Rgb24, 0, 0, 0, 0, ResizeOption.Resize)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize)]
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

        [TestCase(TestFiles.pngWhite2x2px, TestFiles.pngBlack2x2px, TestFiles.pngTransparent4x4px, 765, 12240, 16, 100d, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.pngWhite2x2px, TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, 765, 12240, 16, 100d, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, TestFiles.pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.pngBlack4x4px, TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift1, TestFiles.pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 0)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, TestFiles.pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, 20)]
        public void ShouldUseDiffMask(string pathPic1, string pathPic2, string pathPic3, double expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int colorShiftTolerance)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = SKBitmap.Decode(absolutePathPic1);
            using var pic2 = SKBitmap.Decode(absolutePathPic2);
            using var maskPic = SKBitmap.Decode(differenceMaskPic);

            var maskedDiff = Compare.CalcDiff(pic1, pic2, maskPic, resizeOption, colorShiftTolerance);

            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.pngBlack4x4px, TestFiles.pngBlack2x2px)]
        [TestCase(TestFiles.pngBlack4x4px, TestFiles.pngBlack2x2px, TestFiles.pngBlack2x2px)]
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

        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 0, 0, 0, 0)]
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

        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, 0, false)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, 20, true)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, 0, false)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByFilePath_NoDifferences(string image1RelativePath, string image2RelativePath, int colorShiftTolerance, bool expectIsImageEntirelyBlack)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = Compare.CalcDiffMaskImage(image1Path, image2Path, ResizeOption.DontResize, colorShiftTolerance);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
                Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask1Image, TransparencyOptions.IgnoreAlphaChannel), Is.EqualTo(expectIsImageEntirelyBlack));
            }

            using var diffMask2Image = Compare.CalcDiffMaskImage(image1Path, image2Path, diffMask1Path);

            using (var diffMask2Stream = File.Create(diffMask1Path))
            {
                ImageExtensions.SaveAsPng(diffMask2Image, diffMask2Stream);
            }

            Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask2Image, TransparencyOptions.IgnoreAlphaChannel));

            File.Delete(diffMask1Path);
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32)]
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
                Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask2Image, TransparencyOptions.IgnoreAlphaChannel), Is.True);
            }

            File.Delete(diffMask1Path);
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32, TestFiles.png1Rgba32, 0, false)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift1, TestFiles.colorShift2, 15, true)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByImage_NoDifferences(string image1RelativePath, string image2RelativePath, string image3RelativePath, int colorShiftTolerance, bool expectToleranceMaskToEntirelyBlack)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var image3Path = Path.Combine(AppContext.BaseDirectory, image3RelativePath);

            using var image1 = SKBitmap.Decode(image1Path);
            using var image2 = SKBitmap.Decode(image2Path);
            using var image3 = SKBitmap.Decode(image3Path);

            using var diffMask1Image = Compare.CalcDiffMaskImage(image1, image2);

            using var diffMask2Image = Compare.CalcDiffMaskImage(image1, image3, diffMask1Image, pixelColorShiftTolerance: colorShiftTolerance);

            Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask1Image, TransparencyOptions.IgnoreAlphaChannel), Is.EqualTo(expectToleranceMaskToEntirelyBlack));
            Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask2Image, TransparencyOptions.IgnoreAlphaChannel), Is.True);
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, 0)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, 15)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2, 15)]
        public void CalcDiffMaskImage_ToleranceColorShift_NoDifferences(string image1RelativePath, string image2RelativePath, int colorShiftTolerance)
        {
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);

            using var image1 = SKBitmap.Decode(image1Path);
            using var image2 = SKBitmap.Decode(image2Path);

            using var diffMask1Image = Compare.CalcDiffMaskImage(image1, image2, pixelColorShiftTolerance: colorShiftTolerance);

            Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask1Image, TransparencyOptions.IgnoreAlphaChannel), Is.True);
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg1Rgb24)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png1Rgba32)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.png1Rgba32)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.False);
        }

        [Test]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.jpg1Rgb24)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.png1Rgba32)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png1Rgba32)]
        [TestCase(TestFiles.jpg0Rgb24, TestFiles.png0Rgba32)]
        [TestCase(TestFiles.jpg1Rgb24, TestFiles.png1Rgba32)]
        [TestCase(TestFiles.colorShift1, TestFiles.colorShift2)]
        public void ShouldVerifyThatImageStreamAreNotEqual(string pathActual, string pathExpected)
        {
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(Compare.ImagesAreEqual(actual, expected), Is.False);
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.pngBlack2x2px)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => Compare.CalcDiff(absolutePathPic1, absolutePathPic2));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32, TestFiles.pngBlack2x2px)]
        [TestCase(TestFiles.png0Rgba32, TestFiles.pngBlack2x2px, TestFiles.png0Rgba32)]
        [TestCase(TestFiles.pngBlack2x2px, TestFiles.png0Rgba32, TestFiles.png0Rgba32)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2, string pathPic3)
        {
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var absolutePathPic3 = Path.Combine(AppContext.BaseDirectory, pathPic3);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => Compare.CalcDiff(absolutePathPic1, absolutePathPic2, absolutePathPic3));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }
    }
}