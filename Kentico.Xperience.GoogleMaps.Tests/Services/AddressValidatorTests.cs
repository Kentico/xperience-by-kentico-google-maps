using NSubstitute;
using NUnit.Framework;

namespace Kentico.Xperience.GoogleMaps.Tests
{
    public class AddressValidatorTests
    {
        [TestFixture]
        public class ValidateTests : AddressValidatorTestsBase
        {
            [Test]
            public async Task Validate_ValidAddress_ReturnsExpectedResult()
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
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                AddressComplete = true,
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = VALID_ADDRESS,
                            },
                        },
                    })
                });

                var result = await addressValidator.Validate(VALID_ADDRESS);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.True);
                    Assert.That(result.FormattedAddress, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(NumberOfRequests, Is.EqualTo(2));

                    httpClientFactory.Received(2).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_InvalidAddress_ReturnsExpectedResult()
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

                var result = await addressValidator.Validate(INVALID_ADDRESS);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.False);
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_CompanyNamesEnabled_ReturnsExpectedResult()
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
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                AddressComplete = true,
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = COMPANY_ADDRESS,
                            },
                        },
                    })
                });

                var result = await addressValidator.Validate(COMPANY_NAME, "US", true);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.True);
                    Assert.That(result.FormattedAddress, Is.EqualTo(COMPANY_ADDRESS));
                    Assert.That(NumberOfRequests, Is.EqualTo(2));

                    httpClientFactory.Received(2).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_CompanyNamesDisabled_ReturnsExpectedResult()
            {
                const string COMPANY_NAME = "Rockstar Games";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                AddressComplete = null,
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = COMPANY_NAME,
                            },
                        },
                    })
                });

                var result = await addressValidator.Validate(COMPANY_NAME);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.False);
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_NotCompleteAddress_ReturnsExpectedResult()
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
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                AddressComplete = null,
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = NOT_COMPLETE_ADDRESS,
                            },
                        },
                    })
                });

                var result = await addressValidator.Validate(NOT_COMPLETE_ADDRESS);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.False);
                    Assert.That(NumberOfRequests, Is.EqualTo(2));

                    httpClientFactory.Received(2).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [TestCase(null, TestName = "Validate_NullValue_ThrowsException")]
            [TestCase("", TestName = "Validate_EmptyValue_ThrowsException")]
            public async Task Validate(string value)
            {
                var result = await addressValidator.Validate(value);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.False);
                    Assert.That(NumberOfRequests, Is.EqualTo(0));

                    httpClientFactory.Received(0).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }
        }
    }
}
