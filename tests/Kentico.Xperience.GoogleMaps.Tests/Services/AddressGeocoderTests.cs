using NSubstitute;
using NUnit.Framework;

namespace Kentico.Xperience.GoogleMaps.Tests
{
    public class AddressGeocoderTests
    {
        [TestFixture]
        public class GeocodeTests : AddressServicesTestsBase
        {
            [Test]
            public async Task Geocode_ValidAddress_ReturnsValidAddress()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new GeocodeResponse
                    {
                        Status = "OK",
                        Results = new List<GeocodeResult>
                        {
                            new()
                            {
                                FormattedAddress = VALID_ADDRESS,
                            },
                        },
                    }),
                });

                string result = await addressGeocoder.Geocode(VALID_ADDRESS);

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Geocode_InvalidAddress_ReturnsNull()
            {
                const string INVALID_ADDRESS = "This is invalid address.";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new GeocodeResponse
                    {
                        Status = "ZERO_RESULTS",
                        Results = new List<GeocodeResult>(),
                    })
                });

                string result = await addressGeocoder.Geocode(INVALID_ADDRESS);

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null);
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Geocode_NotCompleteAddress_ReturnsNotCompleteAddress()
            {
                const string NOT_COMPLETE_ADDRESS = "Baltimore, MD";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new GeocodeResponse
                    {
                        Status = "OK",
                        Results = new List<GeocodeResult>
                        {
                            new()
                            {
                                FormattedAddress = NOT_COMPLETE_ADDRESS,
                            },
                        },
                    }),
                });

                string result = await addressGeocoder.Geocode(NOT_COMPLETE_ADDRESS);

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.EqualTo(NOT_COMPLETE_ADDRESS));
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Geocode_CompanyName_ReturnsCompanyAddress()
            {
                const string COMPANY_NAME = "Rockstar Games";
                const string COMPANY_ADDRESS = "622 Broadway, New York, NY 10012, USA";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new GeocodeResponse
                    {
                        Status = "OK",
                        Results = new List<GeocodeResult>
                        {
                            new()
                            {
                                FormattedAddress = COMPANY_ADDRESS,
                            },
                        },
                    }),
                });

                string result = await addressGeocoder.Geocode(COMPANY_NAME);

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.EqualTo(COMPANY_ADDRESS));
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Geocode_NotSupportedCountryAddress_ReturnsNull()
            {
                const string NOT_SUPPORTED_ADDRESS = "Nové sady 25";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new GeocodeResponse
                    {
                        Status = "OK",
                    }),
                });

                string result = await addressGeocoder.Geocode(NOT_SUPPORTED_ADDRESS, "US");

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null);
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [TestCase(null, TestName = "Geocode_NullValue_ReturnsNull")]
            [TestCase("", TestName = "Geocode_EmptyValue_ReturnsNull")]
            public async Task Geocode_InvalidValue_ReturnsNull(string value)
            {
                string result = await addressGeocoder.Geocode(value);

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null);
                    Assert.That(NumberOfRequests, Is.EqualTo(0));

                    httpClientFactory.Received(0).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public void Geocode_InvalidAPIKey_ThrowsException()
            {
                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new GeocodeResponse
                    {
                        Status = "REQUEST_DENIED",
                    }),
                });

                Assert.ThrowsAsync<InvalidOperationException>(async () => await addressGeocoder.Geocode("Address"));
            }
        }
    }
}
