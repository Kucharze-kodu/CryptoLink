

namespace CryptoLink.WebUI.Client.Services.Dto
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiresOnUtc { get; set; }
    }
}
