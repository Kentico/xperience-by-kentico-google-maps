using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Kentico.Xperience.GoogleMaps
{
    public class GoogleMapsTagHelper : TagHelper
    {
        private readonly IGoogleMapsScriptsRenderer googleMapsScriptsRenderer;


        public GoogleMapsTagHelper(IGoogleMapsScriptsRenderer googleMapsScriptsRenderer)
        {
            this.googleMapsScriptsRenderer = googleMapsScriptsRenderer;
        }


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
                .AppendLine(googleMapsScriptsRenderer.RenderAddressFormComponentScriptTag()));
        }
    }
}
