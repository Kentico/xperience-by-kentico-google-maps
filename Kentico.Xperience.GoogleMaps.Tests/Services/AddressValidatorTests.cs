using NSubstitute;
using NUnit.Framework;

namespace Kentico.Xperience.GoogleMaps.Tests
{
    public class AddressValidatorTests
    {
        [TestFixture]
        public class ValidateTests : AddressServicesTestsBase
        {
            [Test]
            public async Task Validate_ValidAddress_ReturnsExpectedResult()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                InputGranularity = InputGranularity.Premise,
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
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_InvalidAddress_ReturnsExpectedResult()
            {
                const string INVALID_ADDRESS = "This is invalid address.";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                InputGranularity = InputGranularity.Premise,
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = INVALID_ADDRESS + ", United States",
                            }
                        },
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
            public async Task Validate_NotCompleteAddress_ReturnsExpectedResult()
            {
                const string NOT_COMPLETE_ADDRESS = "Baltimore, MD";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                InputGranularity = InputGranularity.Other
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
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_NotSupportedCountryAddress_ReturnsExpectedResult()
            {
                const string NOT_SUPPORTED_ADDRESS = "Nové sady 25";

                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new AddressValidationResponse
                    {
                        Result = new AddressValidationResult
                        {
                            Verdict = new AddressValidationVerdict
                            {
                                InputGranularity = InputGranularity.Premise
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = NOT_SUPPORTED_ADDRESS + ", United States",
                            },
                        },
                    })
                });

                var result = await addressValidator.Validate(NOT_SUPPORTED_ADDRESS, "US");

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.False);
                    Assert.That(NumberOfRequests, Is.EqualTo(1));

                    httpClientFactory.Received(1).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public async Task Validate_CompantName_ReturnsExpectedResult()
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
                                InputGranularity = InputGranularity.Premise
                            },
                            Address = new AddressValidationAddress
                            {
                                FormattedAddress = COMPANY_NAME + ", United States",
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


            [TestCase(null, TestName = "Validate_NullValue_ReturnsNull")]
            [TestCase("", TestName = "Validate_EmptyValue_ReturnsNull")]
            public async Task Validate_InvalidValue_ReturnsNull(string value)
            {
                var result = await addressValidator.Validate(value);

                Assert.Multiple(() =>
                {
                    Assert.That(result.IsValid, Is.False);
                    Assert.That(NumberOfRequests, Is.EqualTo(0));

                    httpClientFactory.Received(0).CreateClient(GoogleMapsConstants.CLIENT_NAME);
                });
            }


            [Test]
            public void Validate_InvalidAPIKey_ThrowsException()
            {
                MockHttpClient(new List<HttpResponseMessage>
                {
                    GetMessage(new AddressValidationResponse
                    {
                        Error = new AddressValidationError
                        {
                            Message = "Invalid API key."
                        }
                    })
                });

                Assert.ThrowsAsync<InvalidOperationException>(async () => await addressValidator.Validate("Address"));
            }
        }
    }
}
