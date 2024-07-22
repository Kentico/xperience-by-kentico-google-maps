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
        /// <param name="supportedCountry">Supported country in Alpha-2 code. If null, all countries supported by the API.</param>
        /// <returns>Address or null if value doesn't match any concrete address.</returns>
        Task<string?> Geocode(string value, string? supportedCountry = null);
    }
}
