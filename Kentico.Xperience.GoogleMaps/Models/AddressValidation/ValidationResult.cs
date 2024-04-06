namespace Kentico.Xperience.GoogleMaps.Models.AddressValidation
{
    /// <summary>
    /// Represents the result of an address validation.
    /// </summary>
    internal class ValidationResult
    {
        /// <summary>
        /// Gets or sets the verdict of the validation.
        /// </summary>
        public ValidationVerdict? Verdict { get; set; }

        /// <summary>
        /// Gets or sets the validated address.
        /// </summary>
        public ValidationAddress? Address { get; set; }
    }
}
