using CMS.Core;
using CMS.Tests;
using NSubstitute;
using NUnit.Framework;

namespace Kentico.Xperience.GoogleMaps.Tests
{
    public class AddressFormComponentTests
    {
        [TestFixture]
        [Category.Unit]
        public class ValidateInternalTests
        {
            private AddressFormComponent addressFormComponent;
            private IAddressValidator addressValidator;
            private IAddressGeocoder addressGeocoder;
            private ILocalizationService localizationService;

            private const string ERROR_MESSAGE = "Entered value is not a valid address.";


            [SetUp]
            public void SetUp()
            {
                addressValidator = Substitute.For<IAddressValidator>();
                addressGeocoder = Substitute.For<IAddressGeocoder>();
                localizationService = Substitute.For<ILocalizationService>();
                localizationService.GetString(Arg.Any<string>()).Returns(ERROR_MESSAGE);

                addressFormComponent = new AddressFormComponent(addressValidator, addressGeocoder, localizationService);
                addressFormComponent.LoadProperties(new AddressFormComponentProperties());
            }


            [Test]
            public async Task ValidateInternal_DisabledAddressValidationAndCompanyNames_KeepsOriginalValue()
            {
                const string ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressFormComponent.SetValue(ADDRESS);

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledAddressValidation_ValidAddress_SetsValidAddress()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = true,
                    FormattedAddress = VALID_ADDRESS,
                });

                addressFormComponent.SetValue(VALID_ADDRESS);
                addressFormComponent.Properties.EnableValidation = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledAddressValidation_InvalidAddress_AddsError()
            {
                const string INVALID_ADDRESS = "This is invalid address.";

                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = false,
                });

                addressFormComponent.SetValue(INVALID_ADDRESS);
                addressFormComponent.Properties.EnableValidation = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(errors, Is.Not.Empty);
                    Assert.That(errors.Count(), Is.EqualTo(1));
                    Assert.That(errors.First().ErrorMessage, Is.EqualTo(ERROR_MESSAGE));
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledCompanyNames_CompanyName_SetsCompanyAddress()
            {
                const string COMPANY_NAME = "Rockstar Games";
                const string COMPANY_ADDRESS = "622 Broadway, New York, NY 10012, USA";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns(COMPANY_ADDRESS);

                addressFormComponent.SetValue(COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(COMPANY_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledCompanyNames_InvalidCompanyName_KeepsOriginalValue()
            {
                const string INVALID_COMPANY_NAME = "This is invalid company name.";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns((string)null);

                addressFormComponent.SetValue(INVALID_COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(INVALID_COMPANY_NAME));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledCompanyNames_ValidAddress_SetsValidAddress()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns(VALID_ADDRESS);

                addressFormComponent.SetValue(VALID_ADDRESS);
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledAddressValidationAndCompanyNames_ValidAddress_SetsValidAddress()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns(VALID_ADDRESS);
                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = true,
                    FormattedAddress = VALID_ADDRESS,
                });

                addressFormComponent.SetValue(VALID_ADDRESS);
                addressFormComponent.Properties.EnableCompanyNames = true;
                addressFormComponent.Properties.EnableValidation = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledAddressValidationAndCompanyNames_InvalidAddress_AddsError()
            {
                const string INVALID_ADDRESS = "This is invalid address.";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns((string)null);
                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = false,
                });

                addressFormComponent.SetValue(INVALID_ADDRESS);
                addressFormComponent.Properties.EnableValidation = true;
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(errors, Is.Not.Empty);
                    Assert.That(errors.Count(), Is.EqualTo(1));
                    Assert.That(errors.First().ErrorMessage, Is.EqualTo(ERROR_MESSAGE));
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledAddressValidationAndCompanyNames_ValidCompanyName_SetsCompanyAddress()
            {
                const string COMPANY_NAME = "Rockstar Games";
                const string COMPANY_ADDRESS = "622 Broadway, New York, NY 10012, USA";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns(COMPANY_ADDRESS);
                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = true,
                    FormattedAddress = COMPANY_ADDRESS,
                });

                addressFormComponent.SetValue(COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;
                addressFormComponent.Properties.EnableValidation = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Address, Is.EqualTo(COMPANY_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public async Task ValidateInternal_EnabledAddressValidationAndCompanyNames_InvalidCompanyName_AddsError()
            {
                const string INVALID_COMPANY_NAME = "This is invalid company name.";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns((string)null);

                addressFormComponent.SetValue(INVALID_COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;
                addressFormComponent.Properties.EnableValidation = true;

                var errors = await addressFormComponent.ValidateInternal([]);

                Assert.Multiple(() =>
                {
                    Assert.That(errors, Is.Not.Empty);
                    Assert.That(errors.Count(), Is.EqualTo(1));
                    Assert.That(errors.First().ErrorMessage, Is.EqualTo(ERROR_MESSAGE));
                });
            }
        }
    }
}
