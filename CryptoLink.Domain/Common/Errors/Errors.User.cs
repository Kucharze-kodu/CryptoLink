using ErrorOr;

namespace CryptoLink.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail = Error.Conflict(
            code: "User.DuplicateEmail",
            description: "User with that email exists");

        public static Error NotFound = Error.NotFound(
            code: "User.NotFound",
            description: "User not found");

        public static Error InvalidCredentials = Error.Conflict(
            code: "User.InvalidCredentials",
            description: "Invalid credentials");
        
        public static Error NotVerified = Error.Unauthorized(
            code: "User.NotVerified",
            description: "User not verified");
        
        public static Error InvalidToken = Error.Conflict(
            code: "User.InvalidToken",
            description: $"The provided token is invalid or has expired or is already used");
    }
}