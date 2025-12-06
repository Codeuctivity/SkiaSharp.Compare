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
        /// <param name="pathImage1">The  file path to the first image. This cannot be null or empty.</param>
        /// <param name="pathImage2">The  file path to the second image. This cannot be null or empty.</param>
        /// <returns>An <see cref="ICompareResult"/> object representing the differences between the two images.</returns>
        public ICompareResult CalcDiff(string pathImage1, string pathImage2)
        {
            var compareResult = Compare.CalcDiff(pathImage1, pathImage2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(pathImage1, pathImage2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates the difference between two images located at the specified file paths, using a provided difference mask.
        /// </summary>
        /// <param name="pahtImage1"></param>
        /// <param name="pathImage2"></param>
        /// <param name="differenceMask"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(string pahtImage1, string pathImage2, string differenceMask)
        {
            var compareResult = Compare.CalcDiff(pahtImage1, pathImage2, differenceMask, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(pahtImage1, pathImage2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates the difference between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(SKBitmap image1, SKBitmap image2)
        {
            ArgumentNullException.ThrowIfNull(image1);
            ArgumentNullException.ThrowIfNull(image2);

            if (CompareMetadata)
            {
                throw new NotSupportedException("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison.");
            }

            return Compare.CalcDiffInternal(image1, image2, null, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates the difference between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="differenceMaskPic"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(SKBitmap image1, SKBitmap image2, SKBitmap differenceMaskPic)
        {
            if (CompareMetadata)
            {
                throw new NotSupportedException("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison.");
            }

            return Compare.CalcDiff(image1, image2, differenceMaskPic, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates the difference between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(Stream image1, Stream image2)
        {
            var compareResult = Compare.CalcDiff(image1, image2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                if (image1.CanSeek)
                {
                    image1.Position = 0;
                }

                if (image2.CanSeek)
                {
                    image2.Position = 0;
                }

                var metadataDiff = MetadataComparer.CompareMetadata(image1, image2);

                if (image1.CanSeek)
                {
                    image1.Position = 0;
                }

                if (image2.CanSeek)
                {
                    image2.Position = 0;
                }

                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates the difference between two images provided as file streams, using a specified mask image to focus the comparison.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="maskImage"></param>
        /// <returns></returns>
        public ICompareResult CalcDiff(Stream image1, Stream image2, SKBitmap maskImage)
        {
            var compareResult = Compare.CalcDiff(image1, image2, maskImage, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);

            if (CompareMetadata)
            {
                var metadataDiff = MetadataComparer.CompareMetadata(image1, image2);
                return new CompareResultWithMetadata(compareResult, metadataDiff);
            }

            return compareResult;
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images located at the specified file paths.
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(string pathImage1, string pathImage2)
        {
            return Compare.CalcDiffMaskImage(pathImage1, pathImage2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two in-memory images represented as <see cref="SKBitmap"/> objects.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(SKBitmap image1, SKBitmap image2)
        {
            return Compare.CalcDiffMaskImage(image1, image2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images located at the specified file paths,
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <param name="pathDiffMask"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(string pathImage1, string pathImage2, string pathDiffMask)
        {
            return Compare.CalcDiffMaskImage(pathImage1, pathImage2, pathDiffMask, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images provided as file streams.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(Stream image1, Stream image2)
        {
            return Compare.CalcDiffMaskImage(image1, image2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two images provided as file streams,
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="diffMask"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(Stream image1, Stream image2, Stream diffMask)
        {
            return Compare.CalcDiffMaskImage(image1, image2, diffMask, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Calculates a difference mask image that highlights the differences between two in-memory images represented as <see cref="SKBitmap"/> objects,
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="diffMask"></param>
        /// <returns></returns>
        public SKBitmap CalcDiffMaskImage(SKBitmap image1, SKBitmap image2, SKBitmap diffMask)
        {
            return Compare.CalcDiffMaskImage(image1, image2, diffMask, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        /// <summary>
        /// Checks if two images located at the specified file paths are identical, considering the configured options for resizing,
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <returns></returns>
        public bool ImagesAreEqual(string pathImage1, string pathImage2)
        {
            return Compare.ImagesAreEqual(pathImage1, pathImage2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions, CompareMetadata);
        }

        /// <summary>
        /// Checks if two images provided as file streams are identical, considering the configured options for resizing,
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public bool ImagesAreEqual(Stream image1, Stream image2)
        {
            return Compare.ImagesAreEqual(image1, image2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions, CompareMetadata);
        }

        /// <summary>
        /// Checks if two in-memory images represented as <see cref="SKBitmap"/> objects are identical, considering the configured options for resizing,
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public bool ImagesAreEqual(SKBitmap image1, SKBitmap image2)
        {
            return Compare.ImagesAreEqual(image1, image2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions, CompareMetadata);
        }

        /// <summary>
        /// Checks if two images have the same dimensions (width and height).
        /// </summary>
        /// <param name="pathImage1"></param>
        /// <param name="pathImage2"></param>
        /// <returns></returns>
        public static bool ImagesHaveEqualSize(string pathImage1, string pathImage2)
        {
            return Compare.ImagesHaveEqualSize(pathImage1, pathImage2);
        }
    }
}