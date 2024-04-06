﻿namespace Kentico.Xperience.GoogleMaps.Models.AddressValidation
{
    /// <summary>
    /// Represents the result of an address validation.
    /// </summary>
    public class AddressValidationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the address is valid.
        /// </summary>
        public bool IsValid { get; set; }


        /// <summary>
        /// Gets or sets the formatted address.
        /// </summary>
        public string? FormattedAddress { get; set; }
    }
}
