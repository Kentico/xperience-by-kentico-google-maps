namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Gets addresses using geocoding API endpoint.
    /// </summary>
    public interface IAddressGeocoder
    {
        /// <summary>
        /// Gets address based on the provided value.
        /// </summary>
        /// <param name="value">Value used to get address.</param>
        /// <param name="supportedCountries">Supported countries separated by colons. Use Alpha-2 code for countries. If null, all countries supported by the API.</param>
        /// <returns>Address or null if value doesn't match any concrete address.</returns>
        Task<string?> Geocode(string value, string? supportedCountries = null);
    }
}
