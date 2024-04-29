using System.Net.Http.Headers;
using CMS.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NUnit.Framework;

namespace Kentico.Xperience.GoogleMaps.Tests
{
    /// <summary>
    /// Tests for <see cref="StartupExtensions"/> class.
    /// </summary>
    public class StartupExtensionsTests
    {
        [TestFixture]
        [Category.Unit]
        public class AddXperienceGoogleMapsTests
        {
            private const string APIKEY = "APIKey";

            private IServiceCollection services;
            private IConfiguration configuration;


            [SetUp]
            public void Setup()
            {
                services = new ServiceCollection();
            }


            [Test]
            public void AddXperienceGoogleMaps_ValidOptions_SetupOptionsAddHttpClient()
            {
                configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { $"{GoogleMapsConstants.SECTION_KEY}:{nameof(GoogleMapsOptions.APIKey)}", APIKEY }
                    })
                    .Build();

                services.AddXperienceGoogleMaps(configuration);

                var serviceProvider = services.BuildServiceProvider();
                var googleMapsOptions = serviceProvider.GetRequiredService<IOptions<GoogleMapsOptions>>();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(GoogleMapsConstants.CLIENT_NAME);

                Assert.Multiple(() =>
                {
                    Assert.That(googleMapsOptions, Is.Not.Null);
                    Assert.That(googleMapsOptions.Value.APIKey, Is.EqualTo(APIKEY));
                    Assert.That(httpClient.DefaultRequestHeaders.Accept, Is.InstanceOf<HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue>>());
                    Assert.That(httpClient.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")));
                });
            }


            [TestCase(null, TestName = "AddXperienceGoogleMaps_APIKeyNull_ThrowsException")]
            [TestCase("", TestName = "AddXperienceGoogleMaps_APIKeyEmpty_ThrowsException")]
            public void AddXperienceGoogleMaps_InvalidOptions_ThrowsException(string apiKey)
            {
                configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { $"{GoogleMapsConstants.SECTION_KEY}:{nameof(GoogleMapsOptions.APIKey)}", apiKey },
                    })
                    .Build();

                Assert.That(() => services.AddXperienceGoogleMaps(configuration), Throws.InvalidOperationException);
            }


            [Test]
            public void AddXperienceGoogleMaps_ConfigurationNull_ThrowsException()
            {
                Assert.That(() => services.AddXperienceGoogleMaps(null), Throws.ArgumentNullException);
            }
        }
    }
}
