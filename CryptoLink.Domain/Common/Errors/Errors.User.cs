using ErrorOr;

namespace CryptoLink.Domain.Common.Errors;

public static partial class Errors
{
    public static class BookWord
    {
        public static Error DuplicateEmail = Error.Conflict(
            code: "BookWord.DuplicateEmail",
            description: "BookWord with that email exists");

        public static Error NotFound = Error.NotFound(
            code: "BookWord.NotFound",
            description: "BookWord not found");

        public static Error InvalidCredentials = Error.Conflict(
            code: "BookWord.InvalidCredentials",
            description: "Invalid credentials");
        
        public static Error InvalidToken = Error.Conflict(
            code: "BookWord.InvalidToken",
            description: $"The provided token is invalid or has expired or is already used");
    }
}