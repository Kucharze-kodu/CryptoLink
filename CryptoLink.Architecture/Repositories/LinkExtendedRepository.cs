using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Domain.Aggregates.LinkExtended;
using CryptoLink.Domain.Aggregates.LinkExtended.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;


namespace CryptoLink.Architecture.Repositories
{
    public class LinkExtendedRepository : ILinkExtendedRepository
    {
        private readonly CryptoLinkDbContext _dbContext;

        public LinkExtendedRepository(CryptoLinkDbContext dbContext )
        {
            _dbContext = dbContext;
        }



        public async Task CreateLinkExtended(UserId userId, string urlExtended, string urlShort, DateTime? expiresAt, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLinkExntended(UserId userId, LinkExtendedId linkExtendedId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EditLinkExntended(UserId userId, LinkExtendedId linkExtendedId, string urlExtended, string urlShort, DateTime? expiresAt, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LinkExtended>> GetAllLinkExntended(UserId userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoadLinkExtended(string linkExtended, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
