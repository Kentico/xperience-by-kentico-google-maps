using System.Net.Http.Json;
using CMS.Core;
using Kentico.Xperience.GoogleMaps.Models.AddressValidation;
using Kentico.Xperience.RepoTemplate.Options;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.GoogleMaps.Services
{
    /// <summary>
    /// Validates addresses using API.
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
        public async Task<AddressValidationResult> Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return GetValidationResult(false);
            }

            var geocodeResponse = await SendGeocodeRequest(value);
            if (geocodeResponse is not null && geocodeResponse.Status != "OK")
            {
                return GetValidationResult(false);
            }

            var validateAddressResponse = await SendValidateAddressRequest(geocodeResponse?.Results?.First().FormattedAddress ?? string.Empty);
            if (validateAddressResponse?.Result?.Verdict is not null
                && validateAddressResponse?.Result?.Address is not null
                && validateAddressResponse.Result.Verdict.AddressComplete)
            {
                return GetValidationResult(true, validateAddressResponse.Result.Address.FormattedAddress);
            }

            return GetValidationResult(false);
        }


        private async Task<GeocodeResponse?> SendGeocodeRequest(string value)
        {
            string url = string.Format(GoogleMapsConstants.GEOCODE_API_URL, options.Value.APIKey, value, "country:US");

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


        private async Task<ValidationResponse?> SendValidateAddressRequest(string value)
        {
            var validateAddressRequestData = new ValidationRequestData()
            {
                Address = new ValidationRequestDataAddress()
                {
                    AddressLines = new List<string> { value },
                    RegionCode = "US"
                }
            };
            string url = string.Format(GoogleMapsConstants.VALIDATION_API_URL, options.Value.APIKey);

            var httpClient = GetHttpClient();

            try
            {
                var validationResponse = await httpClient.PostAsJsonAsync(url, validateAddressRequestData);
                return await validationResponse.Content.ReadFromJsonAsync<ValidationResponse>();
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


        private AddressValidationResult GetValidationResult(bool isValid, string? formattedAddress = null)
        {
            return new AddressValidationResult()
            {
                IsValid = isValid,
                FormattedAddress = formattedAddress
            };
        }
    }
}
