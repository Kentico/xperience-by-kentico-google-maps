using System.Text.Json;
using System.Web;
using Kentico.Xperience.GoogleMaps.Models.AddressValidation;
using Kentico.Xperience.RepoTemplate.Options;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.GoogleMaps.Services
{
    /// <summary>
    /// Validates addresses using Google Maps API.
    /// </summary>
    internal class AddressValidator : IAddressValidator
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptions<GoogleMapsOptions> options;


        /// <summary>
        /// Initializes an instance of the <see cref="AddressValidator"/> class.
        /// </summary>
        public AddressValidator(IHttpClientFactory httpClientFactory, IOptions<GoogleMapsOptions> options)
        {
            this.httpClientFactory = httpClientFactory;
            this.options = options;
        }


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
            query["key"] = options.Value.APIKey;
            query["components"] = "country:US";
            builder.Query = query.ToString();
            string url = builder.ToString();

            var httpClient = GetHttpClient();

            var response = await httpClient.GetAsync(url);

            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);

            return JsonSerializer.Deserialize<GeocodeResponse>(responseString);
        }


        private HttpClient GetHttpClient()
        {
            return httpClientFactory.CreateClient(GoogleMapsConstants.CLIENT_NAME);
        }
    }
}
