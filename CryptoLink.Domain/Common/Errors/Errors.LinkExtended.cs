using ErrorOr;

namespace CryptoLink.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class LinkExtended
        {
            public static Error IsNotAuthorized = Error.Conflict(
            code: "LinkExntended.NotAuthorized",
            description: "Login to create link");

            public static Error IsWrongData = Error.Conflict(
            code: "LinkExntended.IsWrongData",
            description: "We dont have data");

        }
    }
}
