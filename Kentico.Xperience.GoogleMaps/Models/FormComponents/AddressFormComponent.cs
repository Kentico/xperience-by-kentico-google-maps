using System.ComponentModel.DataAnnotations;
using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.GoogleMaps;

[assembly: RegisterFormComponent(AddressFormComponent.IDENTIFIER,
                                 typeof(AddressFormComponent),
                                 "Address",
                                 IconClass = "icon-home")]

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents a address input form component.
    /// </summary>
    public class AddressFormComponent : FormComponent<AddressFormComponentProperties, string>
    {
        private readonly IAddressValidator addressValidator;


        /// <summary>
        /// Represents the <see cref="AddressFormComponent"/> identifier.
        /// </summary>
        public const string IDENTIFIER = "AddressFormComponent";


        /// <summary>
        /// Initializes an instance of the <see cref="AddressFormComponent"/> class.
        /// </summary>
        /// <param name="addressValidator">Service validating addresses.</param>
        public AddressFormComponent(IAddressValidator addressValidator)
        {
            this.addressValidator = addressValidator;
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

            string value = GetValue();

            var addressValidationResult = addressValidator.Validate(value).GetAwaiter().GetResult();

            if (!string.IsNullOrWhiteSpace(value) && !addressValidationResult.IsValid)
            {
                errors.Add(new ValidationResult("Entered value is not a valid address.", new[] { nameof(Value) }));
            }
            else
            {
                Value = addressValidationResult.FormattedAddress ?? string.Empty;
            }

            return errors;
        }
    }
}
