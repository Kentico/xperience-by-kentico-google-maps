using Microsoft.AspNetCore.Html;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Interface for rendering Google Maps script tags.
    /// </summary>
    public interface IGoogleMapsScriptsRenderer
    {
        /// <summary>
        /// Renders script tag with src set to URL of Google Maps plugin.
        /// </summary>
        IHtmlContent RenderPluginScriptTag();


        /// <summary>
        /// Renders tags necessary for <see cref="AddressFormComponent"/>'s autocomplete functionality.
        /// </summary>
        IHtmlContent RenderAddressFormComponentTags();
    }
}
