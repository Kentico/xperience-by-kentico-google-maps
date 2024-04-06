namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Validates addresses.
    /// </summary>
    public interface IAddressValidator
    {
        /// <summary>
        /// Validates if the address is valid.
        /// </summary>
        /// <param name="value">The address to validate.</param>
        /// <returns>True if the address is valid, otherwise false.</returns>
        Task<AddressValidatorResult> Validate(string value);
    }
}
