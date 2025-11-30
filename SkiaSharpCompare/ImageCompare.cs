using SkiaSharp;
using System;
using System.IO;

namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// Provides functionality for comparing images and calculating differences between them.
    /// </summary>
    /// <remarks>The <see cref="ImageCompare"/> class allows users to compare images using various options,
    /// such as resizing,  transparency handling, and pixel color shift tolerance. It supports multiple input types,
    /// including file paths,  streams, and in-memory bitmaps. The class also provides methods to generate difference
    /// masks and check for  image equality or size equality.</remarks>
    public class ImageCompare
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCompare"/> class with the specified options for resizing,
        /// transparency handling, and color shift tolerance.
        /// </summary>
        /// <param name="resizeOption">Specifies how images should be resized before comparison. The default is <see
        /// cref="ResizeOption.DontResize"/>.</param>
        /// <param name="transparencyOptions">Specifies how transparency should be handled during comparison. The default is <see
        /// cref="TransparencyOptions.CompareAlphaChannel"/>.</param>
        /// <param name="pixelColorShiftTolerance">Specifies the tolerance for color shifts in pixel values during comparison.  A value of 0 means no
        /// tolerance, and higher values allow for greater differences. The default is 0.</param>
        /// <param name="compareMetadata">If true, compares image metadata (EXIF, etc.) in addition to pixel data.</param>
        public ImageCompare(ResizeOption resizeOption = ResizeOption.DontResize, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel, int pixelColorShiftTolerance = 0, bool compareMetadata = false)
        {
            ResizeOption = resizeOption;
            TransparencyOptions = transparencyOptions;
            PixelColorShiftTolerance = pixelColorShiftTolerance;
            CompareMetadata = compareMetadata;
        }

        /// <summary>
        /// Gets the resize option that determines how an image should be resized.
        /// </summary>
        public ResizeOption ResizeOption { get; }

        /// <summary>
        /// Gets the transparency options that determine how transparency should be handled during image comparison.
        /// </summary>
        public TransparencyOptions TransparencyOptions { get; }

        /// <summary>
        /// Gets the tolerance level for pixel color shifts in image processing operations.
        /// </summary>
        /// <remarks>This property is typically used to determine whether two pixels are considered
        /// similar in color during image comparison or analysis tasks.</remarks>
        public int PixelColorShiftTolerance { get; }

        /// <summary>
        /// Gets a value indicating whether metadata should be included in the comparison operation.
        /// </summary>
        public bool CompareMetadata { get; }

        /// <summary>
        /// Calculates the difference between two images located at the specified file paths.
        /// </summary>
        /// <remarks>The comparison process may involve resizing the images or applying tolerances for
        /// pixel color shifts and transparency, depending on the configured options.</remarks>
        /// <param name="pathPic1">The  file path to the first image. This cannot be null or empty.</param>
        /// <param name="pathPic2">The  file path to the second image. This cannot be null or empty.</param>
        /// <returns>An <see cref="ICompareResult"/> object representing the differences between the two images.</returns>
        public ICompareResult CalcDiff(string pathPic1, string pathPic2)
        {
            var compareResult = Compare.CalcDiff(pathPic1, pathPic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(pathPic1, pathPic2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates the difference between two images located at the specified file paths, using a provided difference mask.
        /// </summary>
        /// <param name="pathPic1"></param>
        /// <param name="pathPic2"></param>
        /// <param name="differenceMask"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(string pathPic1, string pathPic2, string differenceMask)
        {
            var compareResult = Compare.CalcDiff(pathPic1, pathPic2, differenceMask, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(pathPic1, pathPic2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates the difference between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="pic1"></param>
        /// <param name="pic2"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(SKBitmap pic1, SKBitmap pic2)
        {
            ArgumentNullException.ThrowIfNull(pic1);
            ArgumentNullException.ThrowIfNull(pic2);

            if (CompareMetadata)
            {
                throw new NotSupportedException("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison.");
            }

            return Compare.CalcDiffInternal(pic1, pic2, null, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates the difference between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="Pic1"></param>
        /// <param name="Pic2"></param>
        /// <param name="differenceMaskPic"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(SKBitmap Pic1, SKBitmap Pic2, SKBitmap differenceMaskPic)
        {
            if (CompareMetadata)
            {
                throw new NotSupportedException("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison.");
            }

            return Compare.CalcDiff(Pic1, Pic2, differenceMaskPic, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates the difference between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="pic1"></param>
        /// <param name="pic2"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(Stream pic1, Stream pic2)
        {
            var compareResult = Compare.CalcDiff(pic1, pic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(pic1, pic2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates the difference between two images provided as file streams, using a specified mask image to focus the comparison.
        /// </summary>
        /// <param name="pic1"></param>
        /// <param name="pic2"></param>
        /// <param name="maskImage"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(Stream pic1, Stream pic2, SKBitmap maskImage)
        {
            var compareResult = Compare.CalcDiff(pic1, pic2, maskImage, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(pic1, pic2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images located at the specified file paths.
        /// </summary>
        /// <param name="PathPic1"></param>
        /// <param name="PathPic2"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(string PathPic1, string PathPic2)
        {
            return Compare.CalcDiffMaskImage(PathPic1, PathPic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="Pic1"></param>
        /// <param name="Pic2"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(SKBitmap Pic1, SKBitmap Pic2)
        {
            return Compare.CalcDiffMaskImage(Pic1, Pic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images located at the specified file paths,
        /// </summary>
        /// <param name="image1Path"></param>
        /// <param name="image2Path"></param>
        /// <param name="diffMask1Path"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(string image1Path, string image2Path, string diffMask1Path)
        {
            return Compare.CalcDiffMaskImage(image1Path, image2Path, diffMask1Path, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images provided as file streams.
        /// </summary>
        /// <param name="image1Stream"></param>
        /// <param name="image2Stream"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(Stream image1Stream, Stream image2Stream)
        {
            return Compare.CalcDiffMaskImage(image1Stream, image2Stream, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images provided as file streams,
        /// </summary>
        /// <param name="image1Stream"></param>
        /// <param name="image2Stream"></param>
        /// <param name="diffMask1Stream"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(Stream image1Stream, Stream image2Stream, Stream diffMask1Stream)
        {
            return Compare.CalcDiffMaskImage(image1Stream, image2Stream, diffMask1Stream, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two in-memory images represented as <see cref="SKBitmap"/> objects,
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="diffMask1Image"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(SKBitmap image1, SKBitmap image2, SKBitmap diffMask1Image)
        {
            return Compare.CalcDiffMaskImage(image1, image2, diffMask1Image, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Checks if two images located at the specified file paths are identical, considering the configured options for resizing,
        /// </summary>
        /// <param name="PathActual"></param>
        /// <param name="PathExpected"></param>
        /// <returns></returns>
        public bool ImagesAreEqual(string PathActual, string PathExpected)
        {
            return Compare.ImagesAreEqual(PathActual, PathExpected, ResizeOption, PixelColorShiftTolerance, TransparencyOptions, CompareMetadata);
        }

        /// <summary>
        /// Checks if two images provided as file streams are identical, considering the configured options for resizing,
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public bool ImagesAreEqual(Stream actual, Stream expected)
        {
            return Compare.ImagesAreEqual(actual, expected, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Checks if two in-memory images represented as <see cref="SKBitmap"/> objects are identical, considering the configured options for resizing,
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public bool ImagesAreEqual(SKBitmap actual, SKBitmap expected)
        {
            return Compare.ImagesAreEqual(actual, expected, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Checks if two images have the same dimensions (width and height).
        /// </summary>
        /// <param name="PathActual"></param>
        /// <param name="PathExpected"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(string PathActual, string PathExpected)
        {
            return Compare.ImagesHaveEqualSize(PathActual, PathExpected);
        }
    }
}