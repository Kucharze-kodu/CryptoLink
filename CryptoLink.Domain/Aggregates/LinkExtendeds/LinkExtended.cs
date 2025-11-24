using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Common.Primitives;
using System.Text.Json.Serialization;


namespace CryptoLink.Domain.Aggregates.LinkExtendeds
{
    public sealed class LinkExtended : AggregateRoot<LinkExtendedId>
    {
        public UserId UserId { get; private set; }
        public string UrlExtended { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedOnUtc { get; private set; }
        public DateTime? ExpiretOnUtc { get; private set; } = null;



        // connect table
        private readonly List<Links> _links = new();
        public IReadOnlyCollection<Links> Links => _links.AsReadOnly();

        private readonly List<User> _user = new();
        public IReadOnlyCollection<User> Users => _user.AsReadOnly();



        private LinkExtended(LinkExtendedId id) : base(id)
        {
        }


        [JsonConstructor]
        private LinkExtended(
        LinkExtendedId id,
        UserId userId,
        string urlExtended,
        string shortUrl,
        DateTime? expiretOnUtc) : base(id)
        {
            UserId = userId;
            UrlExtended = urlExtended;
            ShortUrl = shortUrl;
            CreatedOnUtc = DateTime.UtcNow;
            ExpiretOnUtc = expiretOnUtc;
        }

        public static LinkExtended Create(
            UserId userId,
            string urlExtended,
            string shortUrl,
            DateTime? expiretOnUtc
        )
        {
            var linkExtended = new LinkExtended(
                default,
                userId,
                urlExtended,
                shortUrl,
                expiretOnUtc);
            return linkExtended;
        }

        public static LinkExtended Load(
            LinkExtendedId id,
            UserId userId,
            string urlExtended,
            string shortUrl,
            DateTime createdOnUtc,
            DateTime? expiretOnUtc
        )
        {
            var linkExtended = new LinkExtended(
                id,
                userId,
                urlExtended,
                shortUrl,
                expiretOnUtc);
            linkExtended.CreatedOnUtc = createdOnUtc;
            return linkExtended;
        }


    }
}
