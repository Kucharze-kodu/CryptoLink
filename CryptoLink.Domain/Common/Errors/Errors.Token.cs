using ErrorOr;

namespace CryptoLink.Domain.Common.Errors;

public static partial class Errors
{
    public static class Token
    {
        public static Error Invalid = Error.Conflict(
            code: "Token.Invalid",
            description: "The provided token is invalid or has expired or is already used");
        
        public static Error AlreadySent = Error.Conflict(
            code: "Token.AlreadySent",
            description: "The token was already sent");
    }
}