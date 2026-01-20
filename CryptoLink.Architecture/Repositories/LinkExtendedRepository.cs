using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace CryptoLink.Architecture.Repositories
{
    public class LinkExtendedRepository : ILinkExtendedRepository
    {
        private readonly CryptoLinkDbContext _dbContext;

        public LinkExtendedRepository(CryptoLinkDbContext dbContext )
        {
            _dbContext = dbContext;
        }



        public async Task CreateLinkExtended(LinkExtended linkExtended, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.LinkExtendeds.FirstOrDefaultAsync(
                x => x.UrlExtended == linkExtended.UrlExtended && x.ExpiretOnUtc > linkExtended.ExpiretOnUtc, cancellationToken);

            if (result == null)
            {
                return;
            }
            await _dbContext.LinkExtendeds.AddAsync(
                linkExtended,    
                cancellationToken);

        }

        public async Task DeleteLinkExntended(UserId userId, LinkExtendedId linkExtendedId, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.LinkExtendeds.FirstOrDefaultAsync(x => x.Id == linkExtendedId && x.UserId == userId, cancellationToken);

            if (result is null)
            {
                return;
            }

            _dbContext.LinkExtendeds.Remove(result);

        }

        public async Task EditLinkExntended(UserId userId, LinkExtendedId linkExtendedId, string urlExtended, DateTime dataExpire, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.LinkExtendeds.FirstOrDefaultAsync(x => x.Id == linkExtendedId && x.UserId == userId, cancellationToken);

            if (result is null)
            {
                return;
            }

            if(urlExtended != null)
            {
                result.UrlExtended = urlExtended;
            }
            if(dataExpire != DateTime.UtcNow)
            {
                result.ExpiretOnUtc =dataExpire;
            }

        }

        public async Task<IEnumerable<LinkExtended>> GetAllLinkExntended(UserId userId, CancellationToken cancellationToken = default)
        {
            var resultlist = await _dbContext.LinkExtendeds
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            return resultlist;
        }

        public async Task<LinkExtended> GetLinkExtended(UserId userId, LinkExtendedId linkExtendedId, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.LinkExtendeds
                .FirstOrDefaultAsync(x => x.Id == linkExtendedId && x.UserId == userId, cancellationToken);

            if (result is null)
            {
                return null;
            }

            return result;
        }

        public async Task<string?> LoadLinkExtended(string linkExtended, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.LinkExtendeds
                .FirstOrDefaultAsync(x => x.UrlExtended == linkExtended && (x.ExpiretOnUtc == null || x.ExpiretOnUtc > DateTime.UtcNow), cancellationToken);

            if(result is null)
            {
                return null;
            }    
            return result.UrlShort;
        }
    }
}
