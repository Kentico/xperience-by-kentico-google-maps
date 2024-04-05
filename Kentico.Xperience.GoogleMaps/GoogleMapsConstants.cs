namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Constants used by Google Maps integration.
    /// </summary>
    public static class GoogleMapsConstants
    {
        /// <summary>
        /// The URL for the Google Maps Geocode API.
        /// </summary>
        public const string GEOCODE_API_URL = "https://maps.googleapis.com/maps/api/geocode/json";


        /// <summary>
        /// Name of the Google maps section in the 'appsettings.json' file.
        /// </summary>
        public const string SECTION_KEY = "xperience.googleMaps";


        /// <summary>
        /// Name of the named HTTP client.
        /// </summary>
        public const string CLIENT_NAME = "GoogleMapsClient";
    }
}
