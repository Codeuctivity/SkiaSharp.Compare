using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Codeuctivity.SkiaSharpCompare;

namespace SkiaSharpCompare.Benchmarks;

public static class Program
{
	public static void Main(string[] args)
	{
		BenchmarkRunner.Run<ImagesAreEqualBenchmarks>();
	}
}

[MemoryDiagnoser]
public class ImagesAreEqualBenchmarks
{
	private string actualPath = string.Empty;
	private string expectedPath = string.Empty;
	private ImageCompare sut = null!;

	[Params(ResizeOption.Resize, ResizeOption.DontResize)]
	public ResizeOption ResizeOption { get; set; }

	[Params(TransparencyOptions.CompareAlphaChannel, TransparencyOptions.IgnoreAlphaChannel)]
	public TransparencyOptions TransparencyOptions { get; set; }

	[GlobalSetup]
	public void Setup()
	{
		actualPath = Path.Combine(AppContext.BaseDirectory, "TestData", "Calc0.jpg");
		expectedPath = Path.Combine(AppContext.BaseDirectory, "TestData", "Calc0.jpg");

		if (!File.Exists(actualPath) || !File.Exists(expectedPath))
		{
			throw new FileNotFoundException($"Missing benchmark files: '{actualPath}' or '{expectedPath}'.");
		}

		sut = new ImageCompare(ResizeOption, TransparencyOptions);
	}

	[Benchmark]
	public bool ImagesAreEqual_ByPath()
	{
		return sut.ImagesAreEqual(actualPath, expectedPath);
	}
}
