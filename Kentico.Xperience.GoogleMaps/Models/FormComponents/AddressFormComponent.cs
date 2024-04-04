using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.GoogleMaps.Models.FormComponents;

[assembly: RegisterFormComponent(AddressFormComponent.IDENTIFIER,
                                 typeof(AddressFormComponent),
                                 "Address",
                                 IconClass = "icon-home")]

namespace Kentico.Xperience.GoogleMaps.Models.FormComponents
{
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
    }
}
