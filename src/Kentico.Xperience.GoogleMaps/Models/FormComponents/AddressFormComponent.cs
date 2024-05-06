using System.ComponentModel.DataAnnotations;
using CMS.Core;
using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.GoogleMaps;

[assembly: RegisterFormComponent(AddressFormComponent.IDENTIFIER,
                                 typeof(AddressFormComponent),
                                 "{$addressformcomponent.name$}",
                                 IconClass = "icon-home")]

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents a address input form component.
    /// </summary>
    public class AddressFormComponent : FormComponent<AddressFormComponentProperties, string>
    {
        private readonly IAddressValidator addressValidator;
        private readonly IAddressGeocoder addressGeocoder;
        private readonly ILocalizationService localizationService;


        /// <summary>
        /// Represents the <see cref="AddressFormComponent"/> identifier.
        /// </summary>
        public const string IDENTIFIER = "AddressFormComponent";


        /// <summary>
        /// Initializes an instance of the <see cref="AddressFormComponent"/> class.
        /// </summary>
        /// <param name="addressValidator">Service validating addresses.</param>
        /// <param name="addressGeocoder">Service getting addresses using geocoding API.</param>
        /// <param name="localizationService">Localization service.</param>
        public AddressFormComponent(IAddressValidator addressValidator, IAddressGeocoder addressGeocoder, ILocalizationService localizationService)
        {
            this.addressValidator = addressValidator;
            this.addressGeocoder = addressGeocoder;
            this.localizationService = localizationService;
        }


        /// <summary>
        /// Represents the input value in the resulting HTML.
        /// </summary>
        [BindableProperty]
        public string Address { get; set; } = string.Empty;


        ///<inheritdoc/>
        public override string GetValue()
        {
            return Address;
        }


        ///<inheritdoc/>
        public override void SetValue(string value)
        {
            Address = value;
        }


        /// <inheritdoc/>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            errors.AddRange(base.Validate(validationContext));

            return ValidateInternal(errors).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Label of the current location button.
        /// </summary>
        public string CurrentLocationButtonLabel => localizationService.GetString("addressformcomponent.currentlocationbutton.label");


        internal async Task<IEnumerable<ValidationResult>> ValidateInternal(List<ValidationResult> errors)
        {
            string? address = GetValue();

            try
            {
                if (Properties.EnableCompanyNames)
                {
                    address = await addressGeocoder.Geocode(address, Properties.SupportedCountries);
                }

                if (Properties.EnableValidation)
                {
                    var addressValidatorResult = address is not null
                        ? await addressValidator.Validate(address, Properties.SupportedCountries)
                        : null;
                    if (addressValidatorResult is null || !addressValidatorResult.IsValid)
                    {
                        errors.Add(new ValidationResult(localizationService.GetString("addressformcomponent.validationerror"), new[] { nameof(Address) }));
                    }
                    else
                    {
                        Address = addressValidatorResult.FormattedAddress ?? string.Empty;
                    }
                }
                else if (address is not null)
                {
                    Address = address;
                }
            }
            catch (InvalidOperationException)
            {
                errors.Add(new ValidationResult(localizationService.GetString("addressformcomponent.servererror"), new[] { nameof(Address) }));
            }

            return errors;
        }
    }
}
