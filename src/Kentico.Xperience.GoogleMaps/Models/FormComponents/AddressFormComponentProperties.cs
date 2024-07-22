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
        [TextInputComponent(Label = "{$addressformcomponent.properties.defaultvalue.label$}",
            Order = EditingComponentOrder.DEFAULT_VALUE)]
        public override string DefaultValue { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets a value indicating whether validation is enabled for the address form component.
        /// </summary>
        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablevalidation.label$}",
            ExplanationText = "{$addressformcomponent.properties.enablevalidation.explanationtext$}",
            Order = 1)]
        public bool EnableValidation { get; set; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether suggestions are enabled for the address form component.
        /// </summary>
        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablesuggestions.label$}",
            ExplanationText = "{$addressformcomponent.properties.enablesuggestions.explanationtext$}",
            Order = 2)]
        public bool EnableSuggestions { get; set; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether current location suggestions is enabled for the address form component.
        /// </summary>
        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablecurrentlocationsuggestions.label$}",
            ExplanationText = "{$addressformcomponent.properties.enablecurrentlocationsuggestions.explanationtext$}",
            Order = 3)]
        public bool EnableCurrentLocationSuggestions { get; set; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether company names are enabled for the address form component.
        /// </summary>
        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablecompanynames.label$}",
            ExplanationText = "{$addressformcomponent.properties.enablecompanynames.explanationtext$}",
            Order = 4)]
        public bool EnableCompanyNames { get; set; } = false;


        /// <summary>
        /// Gets or sets the language in which suggestions are displayed.
        /// </summary>
        [TextInputComponent(Label = "{$addressformcomponent.properties.suggestionsLanguage.label$}",
            ExplanationText = "{$addressformcomponent.properties.suggestionsLanguage.explanationtext$}",
            ExplanationTextAsHtml = true,
            Order = 5)]
        public string SuggestionsLanguage { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the supported country for the address form component.
        /// </summary>
        [TextInputComponent(Label = "{$addressformcomponent.properties.supportedcountry.label$}",
            ExplanationText = "{$addressformcomponent.properties.supportedcountry.explanationtext$}",
            ExplanationTextAsHtml = true,
            Order = 6)]
        public string SupportedCountry { get; set; } = string.Empty;


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
