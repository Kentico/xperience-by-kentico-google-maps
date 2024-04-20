using System.ComponentModel.DataAnnotations;
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


        /// <summary>
        /// Represents the <see cref="AddressFormComponent"/> identifier.
        /// </summary>
        public const string IDENTIFIER = "AddressFormComponent";


        /// <summary>
        /// Initializes an instance of the <see cref="AddressFormComponent"/> class.
        /// </summary>
        /// <param name="addressValidator">Service validating addresses.</param>
        /// <param name="addressGeocoder">Service getting addresses using geocoding API.</param>
        public AddressFormComponent(IAddressValidator addressValidator, IAddressGeocoder addressGeocoder)
        {
            this.addressValidator = addressValidator;
            this.addressGeocoder = addressGeocoder;
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

            if (Properties.EnableCompanyNames)
            {
                value = addressGeocoder.Geocode(value, Properties.SupportedCountries).GetAwaiter().GetResult();
            }

            var addressValidationResult = Properties.EnableValidation && value is not null
                ? addressValidator.Validate(value, Properties.SupportedCountries).GetAwaiter().GetResult()
                : null;

            if (Properties.EnableValidation && !addressValidationResult?.IsValid == true)
            {
                errors.Add(new ValidationResult("Entered value is not a valid address.", new[] { nameof(Value) }));
            }
            else if (Properties.EnableValidation && addressValidationResult is not null)
            {
                Value = addressValidationResult.FormattedAddress ?? string.Empty;
            }

            return errors;
        }
    }
}
