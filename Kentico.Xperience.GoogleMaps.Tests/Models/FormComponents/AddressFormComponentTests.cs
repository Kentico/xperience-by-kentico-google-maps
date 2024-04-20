using System.ComponentModel.DataAnnotations;
using CMS.Tests;
using Kentico.Xperience.GoogleMaps;
using NSubstitute;
using NUnit.Framework;

namespace Kentico.Xperience.RepoTemplate.Models.FormComponents
{
    public class AddressFormComponentTests
    {
        public class ValidateTests : UnitTests
        {
            private AddressFormComponent addressFormComponent;
            private IAddressValidator addressValidator;
            private IAddressGeocoder addressGeocoder;
            private ValidationContext validationContext;


            [SetUp]
            public void SetUp()
            {
                addressValidator = Substitute.For<IAddressValidator>();
                addressGeocoder = Substitute.For<IAddressGeocoder>();

                addressFormComponent = new AddressFormComponent(addressValidator, addressGeocoder);
                addressFormComponent.LoadProperties(new AddressFormComponentProperties());

                validationContext = new ValidationContext(addressFormComponent);
            }


            [Test]
            public void Validate_DisabledAddressValidationAndCompanyNames_KeepsOriginalValue()
            {
                const string ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressFormComponent.SetValue(ADDRESS);

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledAddressValidation_ValidAddress_SetsValidAddress()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = true,
                    FormattedAddress = VALID_ADDRESS,
                });

                addressFormComponent.SetValue(VALID_ADDRESS);
                addressFormComponent.Properties.EnableValidation = true;

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledAddressValidation_InvalidAddress_AddsError()
            {
                const string INVALID_ADDRESS = "This is invalid address.";

                addressValidator.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(new AddressValidatorResult
                {
                    IsValid = false,
                });

                addressFormComponent.SetValue(INVALID_ADDRESS);
                addressFormComponent.Properties.EnableValidation = true;

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(errors, Is.Not.Empty);
                    Assert.That(errors.First().ErrorMessage, Is.EqualTo("Entered value is not a valid address."));
                });
            }


            [Test]
            public void Validate_EnabledCompanyNames_CompanyName_SetsCompanyAddress()
            {
                const string COMPANY_NAME = "Rockstar Games";
                const string COMPANY_ADDRESS = "622 Broadway, New York, NY 10012, USA";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns(COMPANY_ADDRESS);

                addressFormComponent.SetValue(COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(COMPANY_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledCompanyNames_InvalidCompanyName_KeepsOriginalValue()
            {
                const string INVALID_COMPANY_NAME = "This is invalid company name.";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns((string)null);

                addressFormComponent.SetValue(INVALID_COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(INVALID_COMPANY_NAME));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledCompanyNames_ValidAddress_SetsValidAddress()
            {
                const string VALID_ADDRESS = "1600 Amphitheatre Parkway, Mountain View, CA";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns(VALID_ADDRESS);

                addressFormComponent.SetValue(VALID_ADDRESS);
                addressFormComponent.Properties.EnableCompanyNames = true;

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledAddressValidationAndCompanyNames_ValidAddress_SetsValidAddress()
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

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(VALID_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledAddressValidationAndCompanyNames_InvalidAddress_AddsError()
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

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(errors, Is.Not.Empty);
                    Assert.That(errors.First().ErrorMessage, Is.EqualTo("Entered value is not a valid address."));
                });
            }


            [Test]
            public void Validate_EnabledAddressValidationAndCompanyNames_ValidCompanyName_SetsCompanyAddress()
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

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(addressFormComponent.Value, Is.EqualTo(COMPANY_ADDRESS));
                    Assert.That(errors, Is.Empty);
                });
            }


            [Test]
            public void Validate_EnabledAddressValidationAndCompanyNames_InvalidCompanyName_AddsError()
            {
                const string INVALID_COMPANY_NAME = "This is invalid company name.";

                addressGeocoder.Geocode(Arg.Any<string>(), Arg.Any<string>()).Returns((string)null);

                addressFormComponent.SetValue(INVALID_COMPANY_NAME);
                addressFormComponent.Properties.EnableCompanyNames = true;
                addressFormComponent.Properties.EnableValidation = true;

                var errors = addressFormComponent.Validate(validationContext);

                Assert.Multiple(() =>
                {
                    Assert.That(errors, Is.Not.Empty);
                    Assert.That(errors.First().ErrorMessage, Is.EqualTo("Entered value is not a valid address."));
                });
            }
        }
    }
}
