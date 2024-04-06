using CMS.DataEngine;
using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents properties of a <see cref="AddressFormComponent"/>.
    /// </summary>
    public class AddressFormComponentProperties : FormComponentProperties<string>
    {
        ///<inheritdoc/>
        [TextInputComponent(Label = "{$kentico.formbuilder.defaultvalue$}", Order = EditingComponentOrder.DEFAULT_VALUE)]
        public override string DefaultValue { get; set; } = string.Empty;


        [CheckBoxComponent(Label = "Enable Validation", Order = EditingComponentOrder.DEFAULT_VALUE,
            ExplanationText = "This option enables validation of addresses using Google Maps API.")]
        public bool EnableValidation { get; set; } = false;


        [CheckBoxComponent(Label = "Enable company names", Order = EditingComponentOrder.DEFAULT_VALUE,
            ExplanationText = "This option enables suggestions and validation for company names.")]
        public bool EnableCompanyNames { get; set; } = false;


        [TextInputComponent(Label = "Supported countries", Order = EditingComponentOrder.DEFAULT_VALUE,
            ExplanationText = "Insert countries you want to support separated by colons. Use Alpha-2 code for countries: https://www.iban.com/country-codes")]
        public string SupportedCountries { get; set; } = string.Empty;


        /// <summary>
        /// Initializes a new instance of the <see cref="AddressFormComponentProperties"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the base class to data type <see cref="FieldDataType.Text"/> and size 500.
        /// </remarks>
        public AddressFormComponentProperties()
            : base(FieldDataType.Text, 500)
        {
        }
    }
}
