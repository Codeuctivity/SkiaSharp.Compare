using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// SkiaSharpCompare, compares images. An alpha channel is ignored.
    /// </summary>
    public static class Compare
    {
        private const string sizeDiffersExceptionMessage = "Size of images differ.";

        /// <summary>
        /// Is true if width and height of both images are equal
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(string pathImage1, string pathImage2)
        {
            using var image1 = SKBitmap.Decode(pathImage1);
            using var image2 = SKBitmap.Decode(pathImage2);
            return ImagesHaveEqualSize(image1, image2);
        }

        /// <summary>
        /// Is true if width and height of both images are equal
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(Stream image1, Stream image2)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);
            using var actualImage = DecodeStream(image1);
            using var expectedImage = DecodeStream(image2);
            return ImagesHaveEqualSize(actualImage, expectedImage);
        }

        /// <summary>
        /// Is true if width and height of both images are equal
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(SKBitmap image1, SKBitmap image2)
        {
            return ImagesHaveSameDimension(image1, image2);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <param name="compareMetadata">If true, compares image metadata (EXIF, etc.) in addition to pixel data.</param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(string pathImage1, string pathImage2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel, bool compareMetadata = false)
        {
            if (compareMetadata)
            {
                var hasMetadataDiff = MetadataComparer.CompareMetadata(pathImage1, pathImage2)?.Count != 0;

                if (hasMetadataDiff)
                {
                    return false;
                }
            }

            using var image1 = SKBitmap.Decode(pathImage1);
            using var image2 = SKBitmap.Decode(pathImage2);
            return ImagesAreEqual(image1, image2, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <param name="compareMetadata">If true, compares image metadata (EXIF, etc.) in addition to pixel data.</param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(Stream image1, Stream image2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel, bool compareMetadata = false)
        {
            if (compareMetadata)
            {
                var hasMetadataDiff = MetadataComparer.CompareMetadata(image1, image2)?.Count != 0;

                if (hasMetadataDiff)
                {
                    return false;
                }
            }

            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);

            using var decodedImage1 = DecodeStream(image1);
            using var decodedImage2 = DecodeStream(image2);
            return ImagesAreEqual(decodedImage1, decodedImage2, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="transparencyOptions"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="compareMetadata">If true, compares image metadata (EXIF, etc.) in addition to pixel data.</param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImagesAreEqual(SKBitmap image1, SKBitmap image2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel, bool compareMetadata = false)
        {
            if (compareMetadata)
            {
                throw new NotSupportedException("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison.");
            }

            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);

            if (resizeOption == ResizeOption.DontResize && !ImagesHaveSameDimension(image1, image2))
            {
                return false;
            }

            if (resizeOption == ResizeOption.DontResize || ImagesHaveSameDimension(image1, image2))
            {
                for (var x = 0; x < image1.Width; x++)
                {
                    for (var y = 0; y < image1.Height; y++)
                    {
                        if (transparencyOptions == TransparencyOptions.CompareAlphaChannel && pixelColorShiftTolerance == 0 && !image1.GetPixel(x, y).Equals(image2.GetPixel(x, y)))
                        {
                            return false;
                        }
                        else
                        {
                            var actualPixel = image1.GetPixel(x, y);
                            var expectedPixel = image2.GetPixel(x, y);
                            var a = 0;

                            if (transparencyOptions == TransparencyOptions.CompareAlphaChannel)
                            {
                                a = Math.Abs(expectedPixel.Alpha - actualPixel.Alpha);
                            }

                            var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                            var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                            var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                            var sum = r + g + b + a;

                            if (sum > pixelColorShiftTolerance)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            var grown = GrowToSameDimension(image1, image2);
            try
            {
                return ImagesAreEqual(grown.Item1, grown.Item2, ResizeOption.DontResize);
            }
            finally
            {
                grown.Item1?.Dispose();
                grown.Item2?.Dispose();
            }
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathImage1, string pathImage2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.IgnoreAlphaChannel)
        {
            using var actual = SKBitmap.Decode(pathImage1);
            using var expected = SKBitmap.Decode(pathImage2);
            return CalcDiffInternal(actual, expected, null, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Stream image1, Stream image2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.IgnoreAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);

            using var actual = DecodeStream(image1);
            using var expected = DecodeStream(image2);
            return CalcDiffInternal(actual, expected, null, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images using a mask image for tolerated difference between the two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="pathMaskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, string pathMaskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            using var image1 = SKBitmap.Decode(pathActualImage);
            using var image2 = SKBitmap.Decode(pathExpectedImage);
            using var mask = SKBitmap.Decode(pathMaskImage);
            return CalcDiff(image1, image2, mask, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="maskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns></returns>
        public static ICompareResult CalcDiff(Stream image1, Stream image2, SKBitmap maskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);
            ArgumentNullException.ThrowIfNull(maskImage);

            using var actual = DecodeStream(image1);
            using var expected = DecodeStream(image2);
            return CalcDiff(actual, expected, maskImage, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="maskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns></returns>
        public static ICompareResult CalcDiff(SKBitmap image1, SKBitmap image2, SKBitmap maskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);
            ArgumentNullException.ThrowIfNull(maskImage);

            var metadataDifference = new Dictionary<string, (string? ValueA, string? ValueB)>();
            return CalcDiff(image1, image2, maskImage, metadataDifference, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Calculates ICompareResult expressing the amount of difference of both images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="metadataDifference"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Mean and absolute pixel error</returns>
        internal static ICompareResult CalcDiffInternal(SKBitmap image1, SKBitmap image2, Dictionary<string, (string? ValueA, string? ValueB)>? metadataDifference, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.IgnoreAlphaChannel)
        {
            var imagesHaveSameDimension = ImagesHaveSameDimension(image1, image2);

            if (resizeOption == ResizeOption.Resize && !imagesHaveSameDimension)
            {
                var grown = GrowToSameDimension(image1, image2);
                try
                {
                    return CalcDiffInternal(grown.Item1, grown.Item2, metadataDifference, ResizeOption.DontResize, pixelColorShiftTolerance, transparencyOptions);
                }
                finally
                {
                    grown.Item1?.Dispose();
                    grown.Item2?.Dispose();
                }
            }

            if (!ImagesHaveSameDimension(image1, image2))
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            var quantity = image1.Width * image1.Height;
            var absoluteError = 0;
            var pixelErrorCount = 0;

            for (var x = 0; x < image1.Width; x++)
            {
                for (var y = 0; y < image1.Height; y++)
                {
                    var actualPixel = image1.GetPixel(x, y);
                    var expectedPixel = image2.GetPixel(x, y);

                    var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                    var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                    var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                    var sum = r + g + b;
                    if (transparencyOptions == TransparencyOptions.CompareAlphaChannel)
                    {
                        var a = Math.Abs(expectedPixel.Alpha - actualPixel.Alpha);
                        sum += a;
                    }
                    absoluteError += (sum > pixelColorShiftTolerance ? sum : 0);
                    pixelErrorCount += (sum > pixelColorShiftTolerance) ? 1 : 0;
                }
            }

            var meanError = (double)absoluteError / quantity;
            var pixelErrorPercentage = (double)pixelErrorCount / quantity * 100;
            return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage, metadataDifference);
        }

        private static SKBitmap DecodeStream(Stream stream)
        {
            // Addressing https://github.com/mono/SkiaSharp/issues/2263

            using var copy = new MemoryStream();

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            stream.CopyTo(copy);

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            var bytes = copy.ToArray();
            using var skData = SKData.CreateCopy(bytes);

            return SKBitmap.Decode(skData);
        }

        internal static ICompareResult CalcDiff(SKBitmap image1, SKBitmap image2, SKBitmap maskImage, Dictionary<string, (string? ValueA, string? ValueB)> metadataDifference, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(maskImage);

            var imagesHaveSameDimension = ImagesHaveSameDimension(image1, image2) && ImagesHaveSameDimension(image1, maskImage);

            if (resizeOption == ResizeOption.Resize && !imagesHaveSameDimension)
            {
                var grown = GrowToSameDimension(image1, image2, maskImage);
                try
                {
                    return CalcDiff(grown.Item1, grown.Item2, grown.Item3, ResizeOption.DontResize, pixelColorShiftTolerance, transparencyOptions);
                }
                finally
                {
                    grown.Item1?.Dispose();
                    grown.Item2?.Dispose();
                    grown.Item3?.Dispose();
                }
            }

            if (!imagesHaveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            var quantity = image1.Width * image1.Height;
            var absoluteError = 0;
            var pixelErrorCount = 0;

            for (var x = 0; x < image1.Width; x++)
            {
                for (var y = 0; y < image1.Height; y++)
                {
                    var maskImagePixel = maskImage.GetPixel(x, y);
                    var actualPixel = image1.GetPixel(x, y);
                    var expectedPixel = image2.GetPixel(x, y);

                    var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                    var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                    var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);

                    var error = 0;

                    if (r > maskImagePixel.Red)
                    {
                        error += r;
                    }

                    if (g > maskImagePixel.Green)
                    {
                        error += g;
                    }

                    if (b > maskImagePixel.Blue)
                    {
                        error += b;
                    }

                    if (transparencyOptions == TransparencyOptions.CompareAlphaChannel)
                    {
                        var a = Math.Abs(expectedPixel.Alpha - actualPixel.Alpha);
                        if (a > maskImagePixel.Alpha)
                        {
                            error += a;
                        }
                    }

                    absoluteError += (error > pixelColorShiftTolerance ? error : 0);
                    pixelErrorCount += error > pixelColorShiftTolerance ? 1 : 0;
                }
            }
            var meanError = (double)absoluteError / quantity;
            var pixelErrorPercentage = (double)pixelErrorCount / quantity * 100;
            return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage, metadataDifference);
        }

        private static bool ImagesHaveSameDimension(SKBitmap actual, SKBitmap expected)
        {
            ArgumentNullException.ThrowIfNull(actual);
            ArgumentNullException.ThrowIfNull(expected);

            return actual.Height == expected.Height && actual.Width == expected.Width;
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(string pathImage1, string pathImage2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            using var actual = SKBitmap.Decode(pathImage1);
            using var expected = SKBitmap.Decode(pathImage2);
            return CalcDiffMaskImage(actual, expected, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <param name="pathMaskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(string pathImage1, string pathImage2, string pathMaskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            using var actual = SKBitmap.Decode(pathImage1);
            using var expected = SKBitmap.Decode(pathImage2);
            using var mask = SKBitmap.Decode(pathMaskImage);
            return CalcDiffMaskImage(actual, expected, mask, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(Stream image1, Stream image2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);

            using var actual = DecodeStream(image1);
            using var expected = DecodeStream(image2);

            return CalcDiffMaskImage(actual, expected, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="maskImage"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(Stream image1, Stream image2, Stream maskImage, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);
            ArgumentNullException.ThrowIfNull(maskImage);

            using var actual = DecodeStream(image1);
            using var expected = DecodeStream(image2);
            using var mask = DecodeStream(maskImage);

            return CalcDiffMaskImage(actual, expected, mask, resizeOption, pixelColorShiftTolerance, transparencyOptions);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(SKBitmap image1, SKBitmap image2, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            var imagesHAveSameDimension = ImagesHaveSameDimension(image1, image2);

            if (resizeOption == ResizeOption.DontResize && !imagesHAveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            if (imagesHAveSameDimension)
            {
                var maskImage = new SKBitmap(image1.Width, image1.Height);

                for (var x = 0; x < image1.Width; x++)
                {
                    for (var y = 0; y < image1.Height; y++)
                    {
                        var actualPixel = image1.GetPixel(x, y);
                        var expectedPixel = image2.GetPixel(x, y);

                        var red = (byte)Math.Abs(actualPixel.Red - expectedPixel.Red);
                        var green = (byte)Math.Abs(actualPixel.Green - expectedPixel.Green);
                        var blue = (byte)Math.Abs(actualPixel.Blue - expectedPixel.Blue);

                        if (pixelColorShiftTolerance == 0)
                        {
                            if (transparencyOptions == TransparencyOptions.CompareAlphaChannel)
                            {
                                var alpha = (byte)Math.Abs(actualPixel.Alpha - expectedPixel.Alpha);
                                // Ensure mask pixel has full opacity if there's any color difference
                                var effectiveAlpha = (red > 0 || green > 0 || blue > 0) ? (byte)255 : alpha;
                                var pixel = new SKColor(red, green, blue, effectiveAlpha);
                                maskImage.SetPixel(x, y, pixel);
                            }
                            else
                            {
                                var pixel = new SKColor(red, green, blue);
                                maskImage.SetPixel(x, y, pixel);
                            }
                        }
                        else
                        {
                            if (transparencyOptions == TransparencyOptions.CompareAlphaChannel)
                            {
                                var alpha = (byte)Math.Abs(actualPixel.Alpha - expectedPixel.Alpha);
                                var pixel = new SKColor(red, green, blue, alpha);
                                var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                                var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                                var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                                var a = Math.Abs(expectedPixel.Alpha - actualPixel.Alpha);
                                var sum = r + g + b + a;

                                if (sum > pixelColorShiftTolerance)
                                {
                                    maskImage.SetPixel(x, y, pixel);
                                }
                                else
                                {
                                    maskImage.SetPixel(x, y, 0);
                                }
                            }
                            else
                            {
                                var pixel = new SKColor(red, green, blue);
                                var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                                var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                                var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                                var sum = r + g + b;

                                if (sum > pixelColorShiftTolerance)
                                {
                                    maskImage.SetPixel(x, y, pixel);
                                }
                                else
                                {
                                    maskImage.SetPixel(x, y, 0);
                                }
                            }
                        }
                    }
                }
                return maskImage;
            }

            var grown = GrowToSameDimension(image1, image2);
            try
            {
                return CalcDiffMaskImage(grown.Item1, grown.Item2, ResizeOption.DontResize, pixelColorShiftTolerance, transparencyOptions);
            }
            finally
            {
                grown.Item1?.Dispose();
                grown.Item2?.Dispose();
            }
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="mask"></param>
        /// <param name="resizeOption"></param>
        /// <param name="pixelColorShiftTolerance"></param>
        /// <param name="transparencyOptions"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static SKBitmap CalcDiffMaskImage(SKBitmap image1, SKBitmap image2, SKBitmap mask, ResizeOption resizeOption = ResizeOption.DontResize, int pixelColorShiftTolerance = 0, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel)
        {
            ArgumentNullException.ThrowIfNull(mask);

            var imagesHaveSameDimension = ImagesHaveSameDimension(image1, image2) && ImagesHaveSameDimension(image1, mask);

            if (resizeOption == ResizeOption.DontResize && !imagesHaveSameDimension)
            {
                throw new SkiaSharpCompareException(sizeDiffersExceptionMessage);
            }

            if (imagesHaveSameDimension)
            {
                var maskImage = new SKBitmap(image1.Width, image1.Height);

                for (var x = 0; x < image1.Width; x++)
                {
                    for (var y = 0; y < image1.Height; y++)
                    {
                        var actualPixel = image1.GetPixel(x, y);
                        var expectedPixel = image2.GetPixel(x, y);
                        var maskPixel = mask.GetPixel(x, y);

                        var redDiff = Math.Abs(actualPixel.Red - expectedPixel.Red);
                        var greenDiff = Math.Abs(actualPixel.Green - expectedPixel.Green);
                        var blueDiff = Math.Abs(actualPixel.Blue - expectedPixel.Blue);

                        var red = (byte)Math.Max(0, redDiff - maskPixel.Red);
                        var green = (byte)Math.Max(0, greenDiff - maskPixel.Green);
                        var blue = (byte)Math.Max(0, blueDiff - maskPixel.Blue);

                        if (transparencyOptions == TransparencyOptions.CompareAlphaChannel)
                        {
                            var alphaDiff = Math.Abs(actualPixel.Alpha - expectedPixel.Alpha);
                            var alpha = (byte)Math.Max(0, alphaDiff - maskPixel.Alpha);
                            var pixel = new SKColor(red, green, blue, alpha);

                            if (pixelColorShiftTolerance == 0)
                            {
                                maskImage.SetPixel(x, y, pixel);
                            }
                            else
                            {
                                var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                                var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                                var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                                var a = Math.Abs(expectedPixel.Alpha - actualPixel.Alpha);
                                var sum = r + g + b + a;

                                if (sum > pixelColorShiftTolerance)
                                {
                                    maskImage.SetPixel(x, y, pixel);
                                }
                                else
                                {
                                    maskImage.SetPixel(x, y, 0);
                                }
                            }
                        }
                        else
                        {
                            var pixel = new SKColor(red, green, blue);

                            if (pixelColorShiftTolerance == 0)
                            {
                                maskImage.SetPixel(x, y, pixel);
                            }
                            else
                            {
                                var r = Math.Abs(expectedPixel.Red - actualPixel.Red);
                                var g = Math.Abs(expectedPixel.Green - actualPixel.Green);
                                var b = Math.Abs(expectedPixel.Blue - actualPixel.Blue);
                                var sum = r + g + b;

                                if (sum > pixelColorShiftTolerance)
                                {
                                    maskImage.SetPixel(x, y, pixel);
                                }
                                else
                                {
                                    maskImage.SetPixel(x, y, 0);
                                }
                            }
                        }
                    }
                }
                return maskImage;
            }

            var grown = GrowToSameDimension(image1, image2, mask);
            try
            {
                return CalcDiffMaskImage(grown.Item1, grown.Item2, grown.Item3, ResizeOption.DontResize, pixelColorShiftTolerance, transparencyOptions);
            }
            finally
            {
                grown.Item1?.Dispose();
                grown.Item2?.Dispose();
                grown.Item3?.Dispose();
            }
        }

        private static (SKBitmap, SKBitmap) GrowToSameDimension(SKBitmap image1, SKBitmap image2)
        {
            var biggestWidth = image1.Width > image2.Width ? image1.Width : image2.Width;
            var biggestHeight = image1.Height > image2.Height ? image1.Height : image2.Height;
            var skSize = new SKSizeI(biggestWidth, biggestHeight);
            var grownExpected = image2.Resize(skSize, SKSamplingOptions.Default);
            var grownActual = image1.Resize(skSize, SKSamplingOptions.Default);

            return (grownActual, grownExpected);
        }

        private static (SKBitmap, SKBitmap, SKBitmap) GrowToSameDimension(SKBitmap image1, SKBitmap image2, SKBitmap mask)
        {
            var biggestWidth = image1.Width > image2.Width ? image1.Width : image2.Width;
            biggestWidth = biggestWidth > mask.Width ? biggestWidth : mask.Width;
            var biggestHeight = image1.Height > image2.Height ? image1.Height : image2.Height;
            biggestHeight = biggestHeight > mask.Height ? biggestHeight : mask.Height;
            var skSize = new SKSizeI(biggestWidth, biggestHeight);
            var grownExpected = image2.Resize(skSize, SKSamplingOptions.Default);
            var grownActual = image1.Resize(skSize, SKSamplingOptions.Default);
            var grownMask = mask.Resize(skSize, SKSamplingOptions.Default);

            return (grownActual, grownExpected, grownMask);
        }
    }
}