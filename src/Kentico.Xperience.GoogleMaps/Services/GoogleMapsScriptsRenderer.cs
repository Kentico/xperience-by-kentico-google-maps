using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Renders Google Maps script tags.
    /// </summary>
    internal class GoogleMapsScriptsRenderer : IGoogleMapsScriptsRenderer
    {
        private readonly IOptions<GoogleMapsOptions> options;


        /// <summary>
        /// Initializes an instance of the <see cref="GoogleMapsScriptsRenderer"/> class.
        /// </summary>
        public GoogleMapsScriptsRenderer(IOptions<GoogleMapsOptions> options)
        {
            this.options = options;
        }


        /// <inheritdoc/>
        public IHtmlContent RenderAddressFormComponentTags()
        {
            string scriptBody = GetEmbeddedResource(GoogleMapsConstants.ADDRESS_FORM_COMPONENT_SCRIPT_PATH);
            string styleBody = GetEmbeddedResource(GoogleMapsConstants.ADDRESS_FORM_COMPONENT_STYLE_PATH);
            return new HtmlContentBuilder()
                .AppendHtml(RenderTag("script", tagBody: scriptBody))
                .AppendHtml(RenderTag("style", tagBody: styleBody));
        }


        /// <inheritdoc/>
        public IHtmlContent RenderPluginScriptTag()
        {
            string url = string.Format(GoogleMapsConstants.PLUGIN_URL, options.Value.APIKey);
            return new HtmlContentBuilder()
               .AppendHtml(RenderTag("script", url));
        }


        private IHtmlContent RenderTag(string tagName, string? tagUrl = null, string? tagBody = null)
        {
            var script = new TagBuilder(tagName);
            if (tagUrl != null)
            {
                script.Attributes.Add("src", tagUrl);
            }
            if (tagBody != null)
            {
                script.InnerHtml.AppendHtml(tagBody);
            }

            return script;
        }


        private string GetEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(name) ?? throw new InvalidOperationException($"Resource '{name}' not found.");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
