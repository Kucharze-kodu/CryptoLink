

namespace CryptoLink.Application.Utils
{
    public interface IUserContext
    {
        bool? IsAuthenticated { get; }
        int? UserId { get; }
        string? UserName { get; }
    }
}
