using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Tag helper for rendering Google Maps scripts and address form component tags.
    /// </summary>
    public class GoogleMapsTagHelper : TagHelper
    {
        private readonly IGoogleMapsScriptsRenderer googleMapsScriptsRenderer;


        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapsTagHelper"/> class.
        /// </summary>
        /// <param name="googleMapsScriptsRenderer">The Google Maps scripts renderer.</param>
        public GoogleMapsTagHelper(IGoogleMapsScriptsRenderer googleMapsScriptsRenderer)
        {
            this.googleMapsScriptsRenderer = googleMapsScriptsRenderer;
        }


        /// <summary>
        /// Processes the Google Maps tag helper.
        /// </summary>
        /// <param name="context">The tag helper context.</param>
        /// <param name="output">The tag helper output.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.TagName = null;
            output.Content.SetHtmlContent(new HtmlContentBuilder()
                .AppendLine(googleMapsScriptsRenderer.RenderPluginScriptTag())
                .AppendLine(googleMapsScriptsRenderer.RenderAddressFormComponentTags()));
        }
    }
}
