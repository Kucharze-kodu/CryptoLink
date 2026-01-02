

namespace CryptoLink.Application.Contracts.Users
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiresOnUtc { get; set; }
    }
}
