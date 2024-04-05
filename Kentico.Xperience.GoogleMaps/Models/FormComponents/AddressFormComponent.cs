using System.ComponentModel.DataAnnotations;
using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.GoogleMaps.Models.FormComponents;
using Kentico.Xperience.GoogleMaps.Services;

[assembly: RegisterFormComponent(AddressFormComponent.IDENTIFIER,
                                 typeof(AddressFormComponent),
                                 "Address",
                                 IconClass = "icon-home")]

namespace Kentico.Xperience.GoogleMaps.Models.FormComponents
{
    /// <summary>
    /// Represents a address input form component.
    /// </summary>
    public class AddressFormComponent : FormComponent<AddressFormComponentProperties, string>
    {
        public const string IDENTIFIER = "AddressFormComponent";


        /// <summary>
        /// Represents the input value in the resulting HTML.
        /// </summary>
        [BindableProperty]
        public string Value { get; set; } = string.Empty;


        /// <summary>
        /// Gets the <see cref="Value"/>.
        /// </summary>
        public override string GetValue() => Value;


        /// <summary>
        /// Sets the <see cref="Value"/>.
        /// </summary>
        public override void SetValue(string value) => Value = value;


        /// <inheritdoc/>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            errors.AddRange(base.Validate(validationContext));

            string value = GetValue();

            var addressValidator = new AddressValidator();
            bool addressValidatorResult = addressValidator.IsValid(value);

            if (!string.IsNullOrWhiteSpace(value) && addressValidatorResult)
            {
                errors.Add(new ValidationResult("Entered value is not a valid address.", new[] { nameof(Value) }));
            }

            return errors;
        }
    }
}
