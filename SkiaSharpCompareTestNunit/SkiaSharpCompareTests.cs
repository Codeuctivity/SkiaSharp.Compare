using Codeuctivity.SkiaSharpCompare;
using NUnit.Framework;
using SkiaSharp;
using System;
using System.IO;

namespace SkiaSharpCompareTestNunit
{
    public class SkiaSharpCompareTests
    {
        private const string jpg0Rgb24 = "../../../TestData/Calc0.jpg";
        private const string jpg1Rgb24 = "../../../TestData/Calc1.jpg";
        private const string png0Rgba32 = "../../../TestData/Calc0.png";
        private const string png1Rgba32 = "../../../TestData/Calc1.png";
        private const string pngBlack2x2px = "../../../TestData/Black.png";
        private const string pngBlack4x4px = "../../../TestData/BlackDoubleSize.png";
        private const string pngWhite2x2px = "../../../TestData/White.png";
        private const string pngTransparent2x2px = "../../../TestData/pngTransparent2x2px.png";
        private const string pngPartialTransparent2x2px = "../../../TestData/pngPartialTransparent2x2px.png";
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

            Assert.That(ImageCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
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

            Assert.That(ImageCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
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

            Assert.That(ImageCompare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedOutcome));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(png0Rgba32, png0Rgba32, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg0Rgb24, jpg0Rgb24, ResizeOption.DontResize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(png0Rgba32, png0Rgba32, ResizeOption.DontResize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngTransparent2x2px, pngPartialTransparent2x2px, ResizeOption.DontResize, TransparencyOptions.IgnoreAlphaChannel)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected, ResizeOption resizeOption, TransparencyOptions transparencyOptions)
        {
            var sut = new ImageCompare(resizeOption, transparencyOptions);
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(sut.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.True);
        }

        [Test]
        [TestCase(pngBlack2x2px, pngBlack2x2px, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, ResizeOption.DontResize, false)]
        [TestCase(colorShift1, colorShift2, ResizeOption.DontResize, false)]
        public void ShouldVerifyThatImagesWithDifferentSizeAreEqual(string pathActual, string pathExpected, ResizeOption resizeOption, bool expectedResult)
        {
            var sut = new ImageCompare(resizeOption);
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(sut.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatImageStreamsAreEqual(string pathActual, string pathExpected)
        {
            var sut = new ImageCompare();
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(sut.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg0Rgb24)]
        [TestCase(png0Rgba32, png0Rgba32)]
        public void ShouldVerifyThatSkiaSharpImagesAreEqual(string pathActual, string pathExpected)
        {
            var sut = new ImageCompare();
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = SKBitmap.Decode(absolutePathActual);
            using var expected = SKBitmap.Decode(absolutePathExpected);

            Assert.That(sut.ImagesAreEqual(actual, expected), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, null, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg0Rgb24, png0Rgba32, 461891, 2.8574583652965777d, 158087, 97.799485288659028d, ResizeOption.Resize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg1Rgb24, png1Rgba32, 460034, 2.8459701566405187d, 158121, 97.820519165573728d, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(png1Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg1Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 208890, 1.2922842790329365d, 2101, 1.2997698646408156d, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(png0Rgba32, png1Rgba32, 203027, 1.25601321422385d, 681, 0.42129618173269651d, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, 12240, 765, 16, 100.0d, ResizeOption.Resize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(renderedForm1, renderedForm2, 49267623, 60.794204096742348d, 174178, 21.49284304047384d, ResizeOption.Resize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(renderedForm2, renderedForm1, 49267623, 60.794204096742348d, 174178, 21.49284304047384d, ResizeOption.Resize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(colorShift1, colorShift2, 117896, 3.437201166180758d, 30398, 88.623906705539355d, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(colorShift1, colorShift2, 0, 0, 0, 0, ResizeOption.DontResize, 15, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngPartialTransparent2x2px, pngTransparent2x2px, 2048, 128.0d, 16, 100, ResizeOption.DontResize, 0, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngPartialTransparent2x2px, pngTransparent2x2px, 0, 0, 0, 0, ResizeOption.DontResize, 0, TransparencyOptions.IgnoreAlphaChannel)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, int pixelColorShiftTolerance, TransparencyOptions transparencyOptions)
        {
            var sut = new ImageCompare(resizeOption, transparencyOptions, pixelColorShiftTolerance);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var diff = sut.CalcDiff(absolutePathPic1, absolutePathPic2);

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
            var sut = new ImageCompare();
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => sut.CalcDiff(absolutePathPic1, absolutePathPic2));

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
            var sut = new ImageCompare(resizeOption);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var diff = sut.CalcDiff(pic1, pic2);
            Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(pngBlack2x2px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngBlack2x2px, 0, 0, 0, 0, ResizeOption.DontResize, true)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, true)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.DontResize, false)]
        public void ShouldCalcDiffMaskSKBitmap(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, bool expectedOutcome)
        {
            var sut = new ImageCompare(resizeOption);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var absolutePic1 = SKBitmap.Decode(absolutePathPic1);
            using var absolutePic2 = SKBitmap.Decode(absolutePathPic2);

            using (var maskImage = sut.CalcDiffMaskImage(absolutePic1, absolutePic2))
            {
                Assert.That(ImageExtensions.IsImageEntirelyBlack(maskImage, TransparencyOptions.IgnoreAlphaChannel), Is.EqualTo(expectedOutcome));
            }
        }

        [TestCase(png0Rgba32, png1Rgba32, null, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngWhite2x2px, pngBlack2x2px, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngTransparent2x2px, pngPartialTransparent2x2px, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(png0Rgba32, png1Rgba32, ResizeOption.DontResize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(png0Rgba32, png1Rgba32, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngWhite2x2px, pngBlack4x4px, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngBlack4x4px, pngWhite2x2px, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(renderedForm1, renderedForm2, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(renderedForm2, renderedForm1, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        public void CalcDiff(string pathPic1, string pathPic2, ResizeOption resizeOption, TransparencyOptions transparencyOptions)
        {
            var sut = new ImageCompare(resizeOption, transparencyOptions);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var maskImage1 = sut.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2);
            Assert.That(ImageExtensions.IsImageEntirelyBlack(maskImage1, transparencyOptions), Is.False);

            using var absolutePic1 = SKBitmap.Decode(absolutePathPic1);
            using var absolutePic2 = SKBitmap.Decode(absolutePathPic2);
            var differenceMask = Path.GetTempFileName() + "differenceMask.png";

            using (var fileStreamDifferenceMask = File.Create(differenceMask))
            using (var maskImage = sut.CalcDiffMaskImage(absolutePic1, absolutePic2))
            {
                Assert.That(ImageExtensions.IsImageEntirelyBlack(maskImage, transparencyOptions), Is.False);
                SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var maskedDiff = Compare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask, resizeOption, 0, transparencyOptions);
            File.Delete(differenceMask);

            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(0), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(0), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(0), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(0), "PixelErrorPercentage");
        }

        private static void SaveAsPng(SKBitmap maskImage, FileStream fileStreamDifferenceMask)
        {
            var encodedData = maskImage.Encode(SKEncodedImageFormat.Png, 100);
            encodedData.SaveTo(fileStreamDifferenceMask);
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngTransparent2x2px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(pngTransparent2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel)]
        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0, ResizeOption.DontResize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.DontResize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(jpg0Rgb24, jpg1Rgb24, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngBlack2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngBlack2x2px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngTransparent2x2px, pngWhite2x2px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(pngTransparent2x2px, pngBlack4x4px, 0, 0, 0, 0, ResizeOption.Resize, TransparencyOptions.IgnoreAlphaChannel)]
        public void ShouldCalcDiffMaskSKBitmapAndUseOutcome(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, ResizeOption resizeOption, TransparencyOptions transparencyOptions)
        {
            var sut = new ImageCompare(resizeOption, transparencyOptions);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPicPath = Path.GetTempFileName() + "differenceMask.png";

            using var absolutePic1 = SKBitmap.Decode(absolutePathPic1);
            using var absolutePic2 = SKBitmap.Decode(absolutePathPic2);

            using (var fileStreamDifferenceMask = File.Create(differenceMaskPicPath))
            using (var maskImage = sut.CalcDiffMaskImage(absolutePic1, absolutePic2))
            {
                SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            using var differenceMaskPic = SKBitmap.Decode(differenceMaskPicPath);
            var maskedDiff = sut.CalcDiff(absolutePic1, absolutePic2, differenceMaskPic);
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
            var sut = new ImageCompare(resizeOption, TransparencyOptions.CompareAlphaChannel, pixelColorShiftTolerance);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = SKBitmap.Decode(absolutePathPic1);
            using var pic2 = SKBitmap.Decode(absolutePathPic2);
            using var maskPic = SKBitmap.Decode(differenceMaskPic);

            var maskedDiff = sut.CalcDiff(pic1, pic2, maskPic);

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
            var sut = new ImageCompare();
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var differenceMaskPic = Path.Combine(AppContext.BaseDirectory, pathPic3);
            using var pic1 = SKBitmap.Decode(absolutePathPic1);
            using var pic2 = SKBitmap.Decode(absolutePathPic2);
            using var maskPic = SKBitmap.Decode(differenceMaskPic);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => sut.CalcDiff(pic1, pic2, maskPic));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [TestCase(png0Rgba32, png1Rgba32, 0, 0, 0, 0)]
        public void CalcDiffStreams(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            var sut = new ImageCompare(transparencyOptions: TransparencyOptions.IgnoreAlphaChannel);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            using var pic1 = new FileStream(absolutePathPic1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var pic2 = new FileStream(absolutePathPic2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using var maskImage = sut.CalcDiffMaskImage(pic1, pic2);

            pic1.Position = 0;
            pic2.Position = 0;

            var maskedDiff = sut.CalcDiff(pic1, pic2, maskImage);
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [TestCase(png0Rgba32, png1Rgba32, TransparencyOptions.CompareAlphaChannel, 0)]
        [TestCase(colorShift1, colorShift2, TransparencyOptions.CompareAlphaChannel, 20)]
        [TestCase(png0Rgba32, png1Rgba32, TransparencyOptions.IgnoreAlphaChannel, 0)]
        [TestCase(colorShift1, colorShift2, TransparencyOptions.IgnoreAlphaChannel, 20)]
        [TestCase(pngTransparent2x2px, pngPartialTransparent2x2px, TransparencyOptions.IgnoreAlphaChannel, 0)]
        [TestCase(pngTransparent2x2px, pngPartialTransparent2x2px, TransparencyOptions.CompareAlphaChannel, 0)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByFilePath_NoDifferences(string image1RelativePath, string image2RelativePath, TransparencyOptions transparencyOptions, int pixelColorShiftTolerance)
        {
            var sut = new ImageCompare(ResizeOption.DontResize, transparencyOptions, pixelColorShiftTolerance);
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = sut.CalcDiffMaskImage(image1Path, image2Path);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
            }

            using var diffMask2Image = sut.CalcDiffMaskImage(image1Path, image2Path, diffMask1Path);

            using (var diffMask2Stream = File.Create(diffMask1Path))
            {
                ImageExtensions.SaveAsPng(diffMask2Image, diffMask2Stream);
            }

            Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask2Image, transparencyOptions), Is.True);

            File.Delete(diffMask1Path);
        }

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByStream_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var sut = new ImageCompare();
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);
            var diffMask1Path = Path.GetTempFileName() + "differenceMask.png";

            using var image1Stream = new FileStream(image1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var image2Stream = new FileStream(image2Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (var diffMask1Stream = File.Create(diffMask1Path))
            {
                using var diffMask1Image = sut.CalcDiffMaskImage(image1Stream, image2Stream);
                ImageExtensions.SaveAsPng(diffMask1Image, diffMask1Stream);
            }

            image1Stream.Position = 0;
            image2Stream.Position = 0;

            using (var diffMask1Stream = new FileStream(diffMask1Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                diffMask1Stream.Position = 0;
                using var diffMask2Image = sut.CalcDiffMaskImage(image1Stream, image2Stream, diffMask1Stream);
                Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask2Image, TransparencyOptions.IgnoreAlphaChannel), Is.True);
            }

            File.Delete(diffMask1Path);
        }

        [TestCase(png0Rgba32, png1Rgba32)]
        public void CalcDiffMaskImage_WhenSupplyingDiffMaskOfTwoImagesByImage_NoDifferences(string image1RelativePath, string image2RelativePath)
        {
            var sut = new ImageCompare();
            var image1Path = Path.Combine(AppContext.BaseDirectory, image1RelativePath);
            var image2Path = Path.Combine(AppContext.BaseDirectory, image2RelativePath);

            using var image1 = SKBitmap.Decode(image1Path);
            using var image2 = SKBitmap.Decode(image2Path);

            using var diffMask1Image = sut.CalcDiffMaskImage(image1, image2);
            using var diffMask2Image = sut.CalcDiffMaskImage(image1, image2, diffMask1Image);

            Assert.That(ImageExtensions.IsImageEntirelyBlack(diffMask2Image, TransparencyOptions.IgnoreAlphaChannel), Is.True);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        [TestCase(colorShift1, colorShift2)]
        [TestCase(pngTransparent2x2px, pngPartialTransparent2x2px)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            var sut = new ImageCompare();
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            Assert.That(sut.ImagesAreEqual(absolutePathActual, absolutePathExpected), Is.False);
        }

        [Test]
        [TestCase(jpg0Rgb24, jpg1Rgb24)]
        [TestCase(png0Rgba32, png1Rgba32)]
        [TestCase(jpg0Rgb24, png1Rgba32)]
        [TestCase(jpg0Rgb24, png0Rgba32)]
        [TestCase(jpg1Rgb24, png1Rgba32)]
        [TestCase(colorShift1, colorShift2)]
        [TestCase(pngTransparent2x2px, pngPartialTransparent2x2px)]
        public void ShouldVerifyThatImageStreamAreNotEqual(string pathActual, string pathExpected)
        {
            var sut = new ImageCompare();
            var absolutePathActual = Path.Combine(AppContext.BaseDirectory, pathActual);
            var absolutePathExpected = Path.Combine(AppContext.BaseDirectory, pathExpected);

            using var actual = new FileStream(absolutePathActual, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var expected = new FileStream(absolutePathExpected, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Assert.That(sut.ImagesAreEqual(actual, expected), Is.False);
        }

        [TestCase(png0Rgba32, pngBlack2x2px, TransparencyOptions.IgnoreAlphaChannel)]
        [TestCase(png0Rgba32, pngBlack2x2px, TransparencyOptions.CompareAlphaChannel)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2, TransparencyOptions transparencyOptions)
        {
            var sut = new ImageCompare(ResizeOption.DontResize, transparencyOptions);
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => sut.CalcDiff(absolutePathPic1, absolutePathPic2));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }

        [TestCase(png0Rgba32, png0Rgba32, pngBlack2x2px)]
        [TestCase(png0Rgba32, pngBlack2x2px, png0Rgba32)]
        [TestCase(pngBlack2x2px, png0Rgba32, png0Rgba32)]
        [TestCase(pngPartialTransparent2x2px, png0Rgba32, png0Rgba32)]
        [TestCase(png0Rgba32, png0Rgba32, pngPartialTransparent2x2px)]
        public void ShouldVerifyThatImageWithDifferentSizeThrows(string pathPic1, string pathPic2, string pathPic3)
        {
            var sut = new ImageCompare();
            var absolutePathPic1 = Path.Combine(AppContext.BaseDirectory, pathPic1);
            var absolutePathPic2 = Path.Combine(AppContext.BaseDirectory, pathPic2);
            var absolutePathPic3 = Path.Combine(AppContext.BaseDirectory, pathPic3);

            var exception = Assert.Throws<SkiaSharpCompareException>(() => sut.CalcDiff(absolutePathPic1, absolutePathPic2, absolutePathPic3));

            Assert.That(exception?.Message, Is.EqualTo("Size of images differ."));
        }
    }
}