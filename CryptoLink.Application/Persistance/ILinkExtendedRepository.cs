using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;


namespace CryptoLink.Application.Persistance
{
    public interface ILinkExtendedRepository
    {
        Task CreateLinkExtended(UserId userId, string urlExtended, string urlShort, DateTime? expiresAt, CancellationToken cancellationToken = default);
        Task EditLinkExntended(UserId userId,LinkExtendedId linkExtendedId, string urlExtended, string urlShort, DateTime? expiresAt, CancellationToken cancellationToken = default);
        Task DeleteLinkExntended(UserId userId, LinkExtendedId linkExtendedId, CancellationToken cancellationToken = default);
        Task<IEnumerable<LinkExtended>> GetAllLinkExntended(UserId userId, CancellationToken cancellationToken = default);

        Task<string> LoadLinkExtended(string linkExtended, CancellationToken cancellationToken = default);
    }
}
