using SkiaSharp;
using System.IO;

namespace Codeuctivity.SkiaSharpCompare
{
    public class ImageCompare
    {
        public ImageCompare(ResizeOption resizeOption = ResizeOption.DontResize, TransparencyOptions transparencyOptions = TransparencyOptions.CompareAlphaChannel, int pixelColorShiftTolerance = 0)
        {
            ResizeOption = resizeOption;
            TransparencyOptions = transparencyOptions;
            PixelColorShiftTolerance = pixelColorShiftTolerance;
        }

        public ResizeOption ResizeOption { get; }
        public TransparencyOptions TransparencyOptions { get; }
        public int PixelColorShiftTolerance { get; }

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