using System.Net.Http.Json;
using CMS.Core;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Validates addresses using API endpoint.
    /// </summary>
    internal class AddressValidator : IAddressValidator
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IEventLogService eventLogService;
        private readonly IOptions<GoogleMapsOptions> options;


        /// <summary>
        /// Initializes an instance of the <see cref="AddressValidator"/> class.
        /// </summary>
        public AddressValidator(IHttpClientFactory httpClientFactory, IEventLogService eventLogService, IOptions<GoogleMapsOptions> options)
        {
            this.httpClientFactory = httpClientFactory;
            this.eventLogService = eventLogService;
            this.options = options;
        }


        ///<inheritdoc/>
        public async Task<AddressValidatorResult> Validate(string value, string supportedCountries = "US", bool enableCompanyNames = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return GetValidationResult(false);
            }

            string address = value;
            if (enableCompanyNames)
            {
                var geocodeResponse = await SendGeocodeRequest(value, supportedCountries);
                if (geocodeResponse is not null && geocodeResponse.Status != "OK")
                {
                    return GetValidationResult(false);
                }
                address = geocodeResponse?.Results?.First().FormattedAddress ?? string.Empty;
            }

            var validateAddressResponse = await SendValidateAddressRequest(address, supportedCountries);
            if (validateAddressResponse?.Result?.Address is not null && validateAddressResponse.Result.Verdict?.AddressComplete == true)
            {
                return GetValidationResult(true, validateAddressResponse.Result.Address.FormattedAddress);
            }

            return GetValidationResult(false);
        }


        private async Task<GeocodeResponse?> SendGeocodeRequest(string value, string supportedCountries)
        {
            string url = string.Format(GoogleMapsConstants.GEOCODE_API_URL, options.Value.APIKey, value, $"country:{supportedCountries}");

            var httpClient = GetHttpClient();

            try
            {
                return await httpClient.GetFromJsonAsync<GeocodeResponse>(url);
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AddressValidator), nameof(SendGeocodeRequest), ex, additionalMessage: $"{nameof(SendGeocodeRequest)} failed. Request path: {url}");
            }

            return null;
        }


        private async Task<AddressValidationResponse?> SendValidateAddressRequest(string value, string supportedCountries)
        {
            var validateAddressRequestData = new AddressValidationRequestData()
            {
                Address = new AddressValidationRequestDataAddress()
                {
                    AddressLines = new List<string> { value },
                    RegionCode = supportedCountries
                }
            };
            string url = string.Format(GoogleMapsConstants.VALIDATION_API_URL, options.Value.APIKey);

            var httpClient = GetHttpClient();

            try
            {
                var validationResponse = await httpClient.PostAsJsonAsync(url, validateAddressRequestData);
                return await validationResponse.Content.ReadFromJsonAsync<AddressValidationResponse>();
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AddressValidator), nameof(SendValidateAddressRequest), ex, additionalMessage: $"{nameof(SendValidateAddressRequest)} failed. Request path: {url}");
            }

            return null;
        }


        private HttpClient GetHttpClient()
        {
            return httpClientFactory.CreateClient(GoogleMapsConstants.CLIENT_NAME);
        }


        private AddressValidatorResult GetValidationResult(bool isValid, string? formattedAddress = null)
        {
            return new AddressValidatorResult()
            {
                IsValid = isValid,
                FormattedAddress = formattedAddress
            };
        }
    }
}
