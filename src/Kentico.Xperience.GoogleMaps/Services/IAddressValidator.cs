namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Validates addresses using API endpoint.
    /// </summary>
    public interface IAddressValidator
    {
        /// <summary>
        /// Validates if the address is valid.
        /// </summary>
        /// <param name="value">The address to validate.</param>
        /// <param name="supportedCountries">Supported countries separated by colons. Use Alpha-2 code for countries. If null, all countries supported by the API.</param>
        /// <returns>True if the address is valid, otherwise false.</returns>
        Task<AddressValidatorResult> Validate(string value, string? supportedCountries = null);
    }
}
