namespace Kentico.Xperience.GoogleMaps.Models.AddressValidation
{
    /// <summary>
    /// Represents the response from the geocoding service.
    /// </summary>
    internal class GeocodeResponse
    {
        /// <summary>
        /// Gets or sets the status of the geocode response.
        /// </summary>
        public string? Status { get; set; }


        /// <summary>
        /// Gets or sets the collection of geocode results.
        /// </summary>
        public IEnumerable<GeocodeResult>? Results { get; set; }
    }
}
