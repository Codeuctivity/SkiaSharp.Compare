using SkiaSharp;
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
        public ImageCompare(ResizeOption resizeOption = ResizeOption.DontResize, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel, int pixelColorShiftTolerance = 0)
        {
            ResizeOption = resizeOption;
            TransparencyOptions = transparencyOptions;
            PixelColorShiftTolerance = pixelColorShiftTolerance;
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
        /// Calculates the difference between two images located at the specified file paths.
        /// </summary>
        /// <remarks>The comparison process may involve resizing the images or applying tolerances for
        /// pixel color shifts and transparency, depending on the configured options.</remarks>
        /// <param name="absolutePathPic1">The absolute file path to the first image. This cannot be null or empty.</param>
        /// <param name="absolutePathPic2">The absolute file path to the second image. This cannot be null or empty.</param>
        /// <returns>An <see cref="ICompareResult"/> object representing the differences between the two images.</returns>
        public ICompareResult CalcDiff(string absolutePathPic1, string absolutePathPic2)
        {
            return Compare.CalcDiff(absolutePathPic1, absolutePathPic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public ICompareResult CalcDiff(string absolutePathPic1, string absolutePathPic2, string differenceMask)
        {
            return Compare.CalcDiff(absolutePathPic1, absolutePathPic2, differenceMask, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public ICompareResult CalcDiff(SKBitmap absolutePic1, SKBitmap absolutePic2, SKBitmap differenceMaskPic)
        {
            return Compare.CalcDiff(absolutePic1, absolutePic2, differenceMaskPic, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public ICompareResult CalcDiff(FileStream pic1, FileStream pic2)
        {
            return Compare.CalcDiff(pic1, pic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public ICompareResult CalcDiff(FileStream pic1, FileStream pic2, SKBitmap maskImage)
        {
            return Compare.CalcDiff(pic1, pic2, maskImage, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public SKBitmap CalcDiffMaskImage(string absolutePathPic1, string absolutePathPic2)
        {
            return Compare.CalcDiffMaskImage(absolutePathPic1, absolutePathPic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public SKBitmap CalcDiffMaskImage(SKBitmap absolutePic1, SKBitmap absolutePic2)
        {
            return Compare.CalcDiffMaskImage(absolutePic1, absolutePic2, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public SKBitmap CalcDiffMaskImage(string image1Path, string image2Path, string diffMask1Path)
        {
            return Compare.CalcDiffMaskImage(image1Path, image2Path, diffMask1Path, ResizeOption, PixelColorShiftTolerance);
        }

        public SKBitmap CalcDiffMaskImage(FileStream image1Stream, FileStream image2Stream)
        {
            return Compare.CalcDiffMaskImage(image1Stream, image2Stream, ResizeOption, PixelColorShiftTolerance);
        }

        public SKBitmap CalcDiffMaskImage(FileStream image1Stream, FileStream image2Stream, FileStream diffMask1Stream)
        {
            return Compare.CalcDiffMaskImage(image1Stream, image2Stream, diffMask1Stream, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public SKBitmap CalcDiffMaskImage(SKBitmap image1, SKBitmap image2, SKBitmap diffMask1Image)
        {
            return Compare.CalcDiffMaskImage(image1, image2, diffMask1Image, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public bool ImagesAreEqual(string absolutePathActual, string absolutePathExpected)
        {
            return Compare.ImagesAreEqual(absolutePathActual, absolutePathExpected, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public bool ImagesAreEqual(FileStream actual, FileStream expected)
        {
            return Compare.ImagesAreEqual(actual, expected, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public bool ImagesAreEqual(SKBitmap actual, SKBitmap expected)
        {
            return Compare.ImagesAreEqual(actual, expected, ResizeOption, PixelColorShiftTolerance, TransparencyOptions);
        }

        public bool ImagesHaveEqualSize(string absolutePathActual, string absolutePathExpected)
        {
            return Compare.ImagesHaveEqualSize(absolutePathActual, absolutePathExpected);
        }
    }
}