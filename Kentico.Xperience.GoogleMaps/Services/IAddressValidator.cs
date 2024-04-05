namespace Kentico.Xperience.GoogleMaps.Services
{
    public interface IAddressValidator
    {
        Task<bool> IsValid(string value);
    }
}
