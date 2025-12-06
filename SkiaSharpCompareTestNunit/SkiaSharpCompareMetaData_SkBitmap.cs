using Codeuctivity.SkiaSharpCompare;
using NUnit.Framework;
using SkiaSharp;
using System;
using System.IO;

namespace SkiaSharpCompareTestNunit
{
    internal class SkiaSharpCompareMetaData_SkBitmap
    {
        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32)]
        [TestCase(TestFiles.imageWithoutGpsMetadata, TestFiles.imageWithGpsMetadata)]
        public void ImagesAreEqual_SamePixelComparedByMetadataShouldReturnResult(string pic1Path, string pic2Path)
        {
            var pic1 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, pic1Path));
            var pic2 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, pic2Path));

            var sut = new ImageCompare(compareMetadata: false);
            Assert.That(sut.ImagesAreEqual(pic1, pic2));
        }

        [TestCase(TestFiles.png0Rgba32, TestFiles.png0Rgba32)]
        [TestCase(TestFiles.imageWithoutGpsMetadata, TestFiles.imageWithGpsMetadata)]
        public void ImagesAreEqual_SamePixelComparedByMetadataShouldThrows(string pic1Path, string pic2Path)
        {
            var pic1 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, pic1Path));
            var pic2 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, pic2Path));

            var sut = new ImageCompare(compareMetadata: true);

            var ex = Assert.Throws<NotSupportedException>(() =>
            {
                sut.ImagesAreEqual(pic1, pic2);
            });

            Assert.That(ex?.Message, Is.EqualTo("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison."));
        }

        [Test]
        public void CalcDiff_SamePixelComparedByMetadataShouldReturnResult_Null()
        {
            var pic1 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithGpsMetadata));
            var pic2 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata));

            var sut = new ImageCompare(compareMetadata: false);
            var actual = sut.CalcDiff(pic1, pic2);

            Assert.That(actual.MetadataDifferences, Is.Null);
            Assert.That(actual.PixelErrorCount, Is.Zero);
        }

        [Test]
        public void CalcDiff_SamePixelComparedByMetadataShouldReturnEmptyResult()
        {
            var pic1 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata));
            var pic2 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata));

            var sut = new ImageCompare(compareMetadata: true);
            var ex = Assert.Throws<NotSupportedException>(() =>
            {
                sut.CalcDiff(pic1, pic2);
            });

            Assert.That(ex?.Message, Is.EqualTo("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison."));
        }

        [Test]
        [SetCulture("en-US")]
        public void CalcDiff_SamePixelComparedByMetadataShouldReturnCollectionOfMetadataThatDiffers()
        {
            var pic1 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithoutGpsMetadata));
            var pic2 = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, TestFiles.imageWithGpsMetadata));

            var sut = new ImageCompare(compareMetadata: true);

            var ex = Assert.Throws<NotSupportedException>(() =>
              {
                  sut.CalcDiff(pic1, pic2);
              });

            Assert.That(ex?.Message, Is.EqualTo("Metadata comparison is not implemented for SKBitmap inputs. https://github.com/mono/SkiaSharp/issues/1139 Use the overload with streams or filepath to get support for metadata comparison."));
        }
    }
}