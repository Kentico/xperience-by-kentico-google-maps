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

        // https://developers.google.com/maps/documentation/geocoding/requests-geocoding#StatusCodes
        private static readonly Lazy<HashSet<string>> apiErrors = new(() =>
        {
            string[] errors = ["OVER_DAILY_LIMIT", "OVER_QUERY_LIMIT", "REQUEST_DENIED", "UNKNOWN_ERROR"];

            return new HashSet<string>(errors, StringComparer.OrdinalIgnoreCase);
        });


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
            if (geocodeResponse is null || geocodeResponse.Status != "OK" || geocodeResponse.Results?.Count() != 1)
            {
                if (geocodeResponse?.Status is not null && IsApiError(geocodeResponse.Status))
                {
                    eventLogService.LogError(nameof(AddressValidator), nameof(Geocode), $"{nameof(SendGeocodeRequest)} returned {geocodeResponse.Status} status.");
                    throw new InvalidOperationException("Geocoding API error.");
                }
                return null;
            }
            return geocodeResponse.Results.First().FormattedAddress;
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
                throw new InvalidOperationException("Geocoding API error.");
            }
        }


        private bool IsApiError(string status)
        {
            return apiErrors.Value.Contains(status);
        }


        private HttpClient GetHttpClient()
        {
            return httpClientFactory.CreateClient(GoogleMapsConstants.CLIENT_NAME);
        }
    }
}
