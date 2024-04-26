namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents the result of an address validation.
    /// </summary>
    internal class AddressValidationResult
    {
        /// <summary>
        /// Gets or sets the verdict of the validation.
        /// </summary>
        public AddressValidationVerdict? Verdict { get; set; }

        /// <summary>
        /// Gets or sets the validated address.
        /// </summary>
        public AddressValidationAddress? Address { get; set; }
    }
}
