using System.Text.Json.Serialization;

namespace Kentico.Xperience.GoogleMaps.Models.AddressValidation
{
    /// <summary>
    /// Represents a geocode result.
    /// </summary>
    internal class GeocodeResult
    {
        /// <summary>
        /// Gets or sets the formatted address.
        /// </summary>
        [JsonPropertyName("formatted_address")]
        public string? FormattedAddress { get; set; }
    }
}
