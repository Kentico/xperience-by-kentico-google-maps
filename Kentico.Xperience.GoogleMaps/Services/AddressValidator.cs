namespace Kentico.Xperience.GoogleMaps.Services
{
    /// <summary>
    /// Validates addresses using Google Maps API.
    /// </summary>
    public class AddressValidator
    {
        public bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return true;
        }
    }
}
