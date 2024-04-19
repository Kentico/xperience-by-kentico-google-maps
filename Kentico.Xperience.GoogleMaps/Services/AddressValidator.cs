﻿using System.Net.Http.Json;
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
        public async Task<AddressValidatorResult> Validate(string value, string? supportedCountries = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return GetValidationResult(false);
            }

            var validateAddressResponse = await SendValidateAddressRequest(value, supportedCountries);
            if (validateAddressResponse?.Result?.Address is not null
                && IsAddressValid(validateAddressResponse))
            {
                return GetValidationResult(true, validateAddressResponse.Result.Address.FormattedAddress);
            }

            return GetValidationResult(false);
        }


        private async Task<AddressValidationResponse?> SendValidateAddressRequest(string value, string? supportedCountries)
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


        private bool IsAddressValid(AddressValidationResponse? validateAddressResponse)
        {
            return validateAddressResponse?.Result?.Verdict?.AddressComplete == true
                && (validateAddressResponse.Result.Verdict?.InputGranularity == InputGranularity.Premise
                || validateAddressResponse.Result.Verdict?.InputGranularity == InputGranularity.SubPremise);
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
