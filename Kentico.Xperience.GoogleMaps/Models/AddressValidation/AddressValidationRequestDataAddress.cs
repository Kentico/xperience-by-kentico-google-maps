namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents the address data for a validation request.
    /// </summary>
    internal class AddressValidationRequestDataAddress
    {
        /// <summary>
        /// Gets or sets the region code of the address.
        /// </summary>
        public string? RegionCode { get; set; }

        /// <summary>
        /// Gets or sets the address lines of the address.
        /// </summary>
        public IEnumerable<string>? AddressLines { get; set; }
    }
}
