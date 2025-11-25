using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;

namespace CryptoLink.Architecture.Repositories
{
    public class BookWordRepository : IBookWordRepository
    {
        private readonly CryptoLinkDbContext _dbContext;

        public BookWordRepository(CryptoLinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task AddBookWord(string word, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllBookWordCategory(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllBookWords(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> RandomLink(int howMany, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
