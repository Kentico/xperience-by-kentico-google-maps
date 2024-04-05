using System.Text.Json;
using System.Web;
using Kentico.Xperience.GoogleMaps.Models.AddressValidation;

namespace Kentico.Xperience.GoogleMaps.Services
{
    /// <summary>
    /// Validates addresses using Google Maps API.
    /// </summary>
    public class AddressValidator
    {
        private readonly HttpClient httpClient;


        /// <summary>
        /// Initializes an instance of the <see cref="AddressValidator"/> class.
        /// </summary>
        public AddressValidator() => httpClient = new HttpClient();


        /// <summary>
        /// Validates if the address is valid.
        /// </summary>
        /// <param name="value">The address to validate.</param>
        /// <returns>True if the address is valid, otherwise false.</returns>
        public async Task<bool> IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var geocodeResponse = await SendGeocodeRequest(value);
            if (geocodeResponse is not null && geocodeResponse.Status != "OK")
            {
                return false;
            }

            return true;
        }


        private async Task<GeocodeResponse?> SendGeocodeRequest(string value)
        {
            var builder = new UriBuilder(GoogleMapsConstants.GEOCODE_API_URL);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["address"] = value;
            query["key"] = "APIKEY";
            query["components"] = "country:US";
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await httpClient.GetAsync(url);

            string responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<GeocodeResponse>(responseString);
        }
    }
}
