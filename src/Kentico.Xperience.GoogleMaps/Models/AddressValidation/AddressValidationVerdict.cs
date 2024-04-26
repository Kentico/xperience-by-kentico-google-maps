namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents the validation verdict for an address.
    /// </summary>
    internal class AddressValidationVerdict
    {
        /// <summary>
        /// Gets or sets a value indicating whether the address is complete.
        /// </summary>
        public bool? AddressComplete { get; set; }


        /// <summary>
        /// Gets or sets the input granularity of the address.
        /// </summary>
        public InputGranularity? InputGranularity { get; set; }
    }
}
