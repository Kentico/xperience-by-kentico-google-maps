using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Renders Google Maps script tags.
    /// </summary>
    internal class GoogleMapsScriptsRenderer : IGoogleMapsScriptsRenderer
    {
        /// <inheritdoc/>
        public IHtmlContent RenderAddressFormComponentScriptTag()
        {
            string scriptBody = GetEmbeddedResource("Kentico.Xperience.GoogleMaps.Scripts.AddressFormComponent.js");
            return new HtmlContentBuilder()
                .AppendHtml($"<script>{scriptBody}</script>");
        }


        /// <inheritdoc/>
        public IHtmlContent RenderPluginScriptTag()
        {
            return RenderScriptTag(GoogleMapsConstants.PLUGIN_URL);
        }


        private IHtmlContent RenderScriptTag(string scriptUrl)
        {
            string scriptSrc = HttpUtility.HtmlAttributeEncode(HttpUtility.UrlPathEncode(scriptUrl));

            var script = new TagBuilder("script");
            script.Attributes.Add("src", scriptSrc);
            script.Attributes.Add("async", string.Empty);
            script.Attributes.Add("type", "text/javascript");

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
