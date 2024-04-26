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
        public string Value { get; set; } = string.Empty;


        ///<inheritdoc/>
        public override string GetValue()
        {
            return Value;
        }


        ///<inheritdoc/>
        public override void SetValue(string value)
        {
            Value = value;
        }


        /// <inheritdoc/>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            errors.AddRange(base.Validate(validationContext));

            string? value = GetValue();

            try
            {
                if (Properties.EnableCompanyNames)
                {
                    value = addressGeocoder.Geocode(value, Properties.SupportedCountries).GetAwaiter().GetResult();
                }

                if (Properties.EnableValidation)
                {
                    var addressValidatorResult = value is not null
                        ? addressValidator.Validate(value, Properties.SupportedCountries).GetAwaiter().GetResult()
                        : null;
                    if (addressValidatorResult is null || !addressValidatorResult.IsValid)
                    {
                        errors.Add(new ValidationResult(localizationService.GetString("addressformcomponent.validationerror"), new[] { nameof(Value) }));
                    }
                    else
                    {
                        Value = addressValidatorResult.FormattedAddress ?? string.Empty;
                    }
                }
                else if (value is not null)
                {
                    Value = value;
                }
            }
            catch (InvalidOperationException)
            {
                errors.Add(new ValidationResult(localizationService.GetString("addressformcomponent.servererror"), new[] { nameof(Value) }));
            }

            return errors;
        }
    }
}
