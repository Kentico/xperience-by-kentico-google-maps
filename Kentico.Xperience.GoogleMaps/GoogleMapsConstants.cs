﻿namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Constants used by Google Maps integration.
    /// </summary>
    public static class GoogleMapsConstants
    {
        /// <summary>
        /// The URL for the Google Maps Geocode API.
        /// </summary>
        public const string GEOCODE_API_URL = "https://maps.googleapis.com/maps/api/geocode/json?key={0}&address={1}&components={2}";


        /// <summary>
        /// The URL for the Google Maps Address Validation API.
        /// </summary>
        public const string VALIDATION_API_URL = "https://addressvalidation.googleapis.com/v1:validateAddress?key={0}";


        /// <summary>
        /// Name of the Google maps section in the 'appsettings.json' file.
        /// </summary>
        public const string SECTION_KEY = "CMSXperienceGoogleMaps";


        /// <summary>
        /// Name of the named HTTP client.
        /// </summary>
        public const string CLIENT_NAME = "XperienceGoogleMapsClient";
    }
}
