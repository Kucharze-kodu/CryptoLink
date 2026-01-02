

namespace CryptoLink.Application.Contracts.Users
{
    public class LoginResponse(
    int Id,
    string Name,
    string Token,
    DateTime TokenExpiresOnUtc);
}
