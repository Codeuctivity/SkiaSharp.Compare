# Compares images

## Compares each pixel to determine the equality

```csharp
bool isEqual = Compare.ImagesAreEqual("actual.png", "expected.png");
```

## [Calculates diff](https://dotnetfiddle.net/tTnq2j)

```csharp
var calcDiff = Compare.CalcDiff("2x2PixelBlack.png", "2x2PixelWhite.png");
Console.WriteLine($"PixelErrorCount: {calcDiff.PixelErrorCount}");
Console.WriteLine($"PixelErrorPercentage: {calcDiff.PixelErrorPercentage}");
Console.WriteLine($"AbsoluteError: {calcDiff.AbsoluteError}");
Console.WriteLine($"MeanError: {calcDiff.MeanError}");
// PixelErrorCount: 4
// PixelErrorPercentage: 100
// AbsoluteError: 3060
// MeanError: 765
```

## [Configure transparency, enable metadata compare, ...](https://dotnetfiddle.net/lygaRU)

```csharp
var comparer = new ImageCompare(ResizeOption.Resize, TransparencyOptions.CompareAlphaChannel, pixelColorShiftTolerance: 5, compareMetadata: true);
var calcDiff = comparer.CalcDiff("pngPartialTransparent4x4Pixel.png", "2x2PixelWhite.png");
// Displaying the differences
Console.WriteLine($"PixelErrorCount: {calcDiff.PixelErrorCount}");
Console.WriteLine($"PixelErrorPercentage: {calcDiff.PixelErrorPercentage}");
Console.WriteLine($"AbsoluteError: {calcDiff.AbsoluteError}");
Console.WriteLine($"MeanError: {calcDiff.MeanError}");
foreach (var metadataDifference in calcDiff.MetadataDifferences)
    Console.WriteLine($"Metadata Difference: {metadataDifference}");

// PixelErrorCount: 16
// PixelErrorPercentage: 100
// AbsoluteError: 14272
// MeanError: 892
// Metadata Difference: [File:File Name, (pngPartialTransparent4x4Pixel.png, 2x2PixelWhite.png)]
// Metadata Difference: [File:File Size, (688 bytes, 556 bytes)]
// Metadata Difference: [PNG-IHDR:Color Type, (True Color with Alpha, True Color)]
// Metadata Difference: [PNG-IHDR:Image Height, (4, 2)]
// Metadata Difference: [PNG-IHDR:Image Width, (4, 2)]
// Metadata Difference: [PNG-tEXt:Textual Data, (Software: Paint.NET 5.1.2, Comment: Created with GIMP)]
// ...
```