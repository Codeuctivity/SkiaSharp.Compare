namespace Codeuctivity.SkiaSharpCompare
{
    /// <summary>
    /// Specifies options for handling transparency when comparing images.
    /// </summary>
    /// <remarks>These options determine whether the alpha channel (transparency) is considered during image
    /// comparison.</remarks>
    public enum TransparencyOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the alpha channel should be ignored during processing.
        /// </summary>
        IgnoreAlphaChannel,

        /// <summary>
        /// Compares the alpha channel values of two colors.
        /// </summary>
        /// <remarks>This method is useful for sorting or comparing colors based on their transparency
        /// levels.</remarks>
        CompareAlphaChannel
    }
}