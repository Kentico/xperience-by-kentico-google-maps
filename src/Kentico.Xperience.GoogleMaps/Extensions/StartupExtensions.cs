using System.Net.Http.Headers;
using Kentico.Xperience.GoogleMaps;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Startup extensions necessary for Google Maps.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Initializes <see cref="GoogleMapsOptions"/>.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        public static IServiceCollection AddXperienceGoogleMaps(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var googleMapsSection = configuration.GetSection(GoogleMapsConstants.SECTION_KEY);
            services.Configure<GoogleMapsOptions>(googleMapsSection);

            var googleMapsOptions = googleMapsSection.Get<GoogleMapsOptions>();

            if (string.IsNullOrEmpty(googleMapsOptions?.APIKey))
            {
                throw new InvalidOperationException(nameof(googleMapsOptions.APIKey));
            }

            services.AddHttpClient(GoogleMapsConstants.CLIENT_NAME, client
                => client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")));

            services.AddSingleton<IAddressValidator, AddressValidator>();
            services.AddSingleton<IAddressGeocoder, AddressGeocoder>();

            return services;
        }
    }
}
