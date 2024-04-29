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
        [TextInputComponent(Label = "{$addressformcomponent.properties.defaultvalue.label$}")]
        public override string DefaultValue { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets a value indicating whether validation is enabled for the address form component.
        /// </summary>
        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablevalidation.label$}", Tooltip = "{$addressformcomponent.properties.enablevalidation.tooltip$}")]
        public bool EnableValidation { get; set; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether company names are enabled for the address form component.
        /// </summary>
        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablecompanynames.label$}", Tooltip = "{$addressformcomponent.properties.enablecompanynames.tooltip$}")]
        public bool EnableCompanyNames { get; set; } = false;


        /// <summary>
        /// Gets or sets the supported countries for the address form component.
        /// </summary>
        [TextInputComponent(Label = "{$addressformcomponent.properties.supportedcountries.label$}", ExplanationText = "{$addressformcomponent.properties.supportedcountries.explanationtext$}",
            ExplanationTextAsHtml = true)]
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
