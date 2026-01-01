using ErrorOr;

namespace CryptoLink.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateName = Error.Conflict(
            code: "User.DuplicateName",
            description: "User with that name exists");

        public static Error NotFound = Error.NotFound(
            code: "User.NotFound",
            description: "User not found");

        public static Error InvalidTokenPGP = Error.Conflict(
            code: "User.InvalidTokenPGP",
            description: $"Invalid PGP Public Key format.");

        public static Error SessionRegisterExpired = Error.Conflict(
            code: "User.SessionRegisterExpired",
            description: $"Registration session expired or invalid.");


    }
}