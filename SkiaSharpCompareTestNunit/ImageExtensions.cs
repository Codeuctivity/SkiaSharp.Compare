using SkiaSharp;
using System.IO;

namespace SkiaSharpCompareTestNunit
{
    internal class ImageExtensions
    {
        protected ImageExtensions()
        {
        }

        internal static void SaveAsPng(SKBitmap diffMask1Image, FileStream diffMask1Stream)
        {
            var encodedData = diffMask1Image.Encode(SKEncodedImageFormat.Png, 100);
            encodedData.SaveTo(diffMask1Stream);
        }

        public static bool IsImageEntirelyBlack(SKBitmap image, Codeuctivity.SkiaSharpCompare.TransparencyOptions transparencyOptions)
        {
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var sKColor = image.GetPixel(x, y);
                    if (sKColor.Red != 0 || sKColor.Green != 0 || sKColor.Blue != 0 || (transparencyOptions == Codeuctivity.SkiaSharpCompare.TransparencyOptions.CompareAlphaChannel && sKColor.Alpha != 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}