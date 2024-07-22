namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents the validation response for an address.
    /// </summary>
    internal class AddressValidationResponse
    {
        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public AddressValidationResult? Result { get; set; }


        /// <summary>
        /// Gets or sets the validation error.
        /// </summary>
        public AddressValidationError? Error { get; set; }
    }
}
