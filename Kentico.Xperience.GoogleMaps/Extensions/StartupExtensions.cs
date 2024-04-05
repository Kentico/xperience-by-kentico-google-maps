using System.Net.Http.Headers;
using Kentico.Xperience.GoogleMaps.Services;
using Kentico.Xperience.RepoTemplate.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kentico.Xperience.GoogleMaps.Extensions
{
    /// <summary>
    /// Startup extensions necessary for Google Maps.
    /// </summary>
    public static class StartupExtensions
    {
        public static IServiceCollection AddGoogleMaps(this IServiceCollection services, IConfiguration configuration)
        {
            var googleMapsSection = configuration.GetSection(GoogleMapsConstants.SECTION_KEY);
            services.Configure<GoogleMapsOptions>(googleMapsSection);

            var googleMapsOptions = googleMapsSection.Get<GoogleMapsOptions>();

            if (string.IsNullOrEmpty(googleMapsOptions.APIKey))
            {
                throw new InvalidOperationException(nameof(googleMapsOptions.APIKey));
            }

            services.Configure<GoogleMapsOptions>(googleMapsSection);

            services.AddHttpClient(GoogleMapsConstants.CLIENT_NAME, client
                => client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")));

            services.AddSingleton<IAddressValidator, AddressValidator>();

            return services;
        }
    }
}
