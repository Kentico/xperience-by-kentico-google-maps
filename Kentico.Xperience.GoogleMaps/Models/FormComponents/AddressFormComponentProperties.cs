﻿using CMS.DataEngine;
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


        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablevalidation.label$}", Order = EditingComponentOrder.DEFAULT_VALUE,
            Tooltip = "{$addressformcomponent.properties.enablevalidation.tooltip$}")]
        public bool EnableValidation { get; set; } = false;


        [CheckBoxComponent(Label = "{$addressformcomponent.properties.enablecompanynames.label$}", Order = EditingComponentOrder.DEFAULT_VALUE,
            Tooltip = "{$addressformcomponent.properties.enablecompanynames.tooltip$}")]
        public bool EnableCompanyNames { get; set; } = false;


        [TextInputComponent(Label = "{$addressformcomponent.properties.supportedcountries.label$}", Order = EditingComponentOrder.DEFAULT_VALUE,
            ExplanationText = "{$addressformcomponent.properties.supportedcountries.explanationtext$}", ExplanationTextAsHtml = true)]
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
