
namespace CryptoLink.Application.Persistance
{
    namespace CryptoLink.Application.Common.Interfaces
    {
        public interface ICacheService
        {
            Task<T?> GetAsync<T>(string key);
            Task SetAsync<T>(string key, T value, TimeSpan expiration);

            Task RemoveAsync(string key);
        }
    }
}
