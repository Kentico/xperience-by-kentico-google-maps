using System.Net.Http.Json;
using CMS.Core;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Gets addresses using geocoding API endpoint.
    /// </summary>
    internal class AddressGeocoder : IAddressGeocoder
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IEventLogService eventLogService;
        private readonly IOptions<GoogleMapsOptions> options;


        /// <summary>
        /// Initializes an instance of the <see cref="AddressGeocoder"/> class.
        /// </summary>
        public AddressGeocoder(IHttpClientFactory httpClientFactory, IEventLogService eventLogService, IOptions<GoogleMapsOptions> options)
        {
            this.httpClientFactory = httpClientFactory;
            this.eventLogService = eventLogService;
            this.options = options;
        }


        ///<inheritdoc/>
        public async Task<string?> Geocode(string value, string? supportedCountries = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var geocodeResponse = await SendGeocodeRequest(value, supportedCountries);
            if (geocodeResponse is not null && geocodeResponse.Status != "OK" && geocodeResponse.Results?.Count() != 1)
            {
                if (geocodeResponse.Status is "OVER_DAILY_LIMIT" or "OVER_QUERY_LIMIT" or "REQUEST_DENIED" or "UNKNOWN_ERROR")
                {
                    eventLogService.LogError(nameof(AddressValidator), nameof(Geocode), $"{nameof(SendGeocodeRequest)} returned {geocodeResponse.Status} status.");
                }
                return null;
            }
            return geocodeResponse?.Results?.First().FormattedAddress;
        }


        private async Task<GeocodeResponse?> SendGeocodeRequest(string value, string? supportedCountries)
        {
            string url = string.Format(GoogleMapsConstants.GEOCODE_API_URL, options.Value.APIKey, value, !string.IsNullOrEmpty(supportedCountries) ? $"country:{supportedCountries}" : null);

            var httpClient = GetHttpClient();

            try
            {
                return await httpClient.GetFromJsonAsync<GeocodeResponse>(url);
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AddressGeocoder), nameof(SendGeocodeRequest), ex, additionalMessage: $"{nameof(SendGeocodeRequest)} failed. Request path: {url}");
            }

            return null;
        }


        private HttpClient GetHttpClient()
        {
            return httpClientFactory.CreateClient(GoogleMapsConstants.CLIENT_NAME);
        }
    }
}
