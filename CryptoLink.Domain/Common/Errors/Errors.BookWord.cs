using ErrorOr;


namespace CryptoLink.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class BookWord
        {
            public static Error IsNotAuthorized = Error.Conflict(
                code: "BookWord.NotAuthorized",
                description: "Login to admin to add word");
        }
    }
}