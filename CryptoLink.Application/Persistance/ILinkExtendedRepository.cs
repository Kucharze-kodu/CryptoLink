using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;


namespace CryptoLink.Application.Persistance
{
    public interface ILinkExtendedRepository
    {
        Task CreateLinkExtended(LinkExtended linkExtended, CancellationToken cancellationToken = default);
        Task EditLinkExntended(UserId userId, LinkExtendedId linkExtendedId, string urlExtended, DateTime dataExpire, CancellationToken cancellationToken = default);
        Task DeleteLinkExntended(UserId userId, LinkExtendedId linkExtendedId, CancellationToken cancellationToken = default);

        Task<IEnumerable<LinkExtended>> GetAllLinkExntended(UserId userId, CancellationToken cancellationToken = default);
        Task<LinkExtended> GetLinkExtended(UserId userId, LinkExtendedId linkExtendedId, CancellationToken cancellationToken = default);

        Task<string?> LoadLinkExtended(string linkExtended, CancellationToken cancellationToken = default);
    }
}
