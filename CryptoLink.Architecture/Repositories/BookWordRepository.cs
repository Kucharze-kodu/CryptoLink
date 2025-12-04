using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Domain.Aggregates.BookWords;
using Microsoft.EntityFrameworkCore;

namespace CryptoLink.Architecture.Repositories
{
    public class BookWordRepository : IBookWordRepository
    {
        private readonly CryptoLinkDbContext _dbContext;

        public BookWordRepository(CryptoLinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task AddBookWord(BookWord bookWord, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.BookWords.SingleOrDefaultAsync(x => x.Word == bookWord.Word, cancellationToken);

            if (result != null)
            {
                return;
            }
            await _dbContext.BookWords.AddAsync(bookWord, cancellationToken);
            

        }

        public async Task<IEnumerable<BookWord>> GetAllBookWordCategory(string category, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.BookWords.Where(x => x.Category == category).ToListAsync(cancellationToken);

            if (result == null)
            {
                return null;
            }

            return result;

        }

        public async Task<IEnumerable<BookWord>> GetAllBookWords(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.BookWords.ToListAsync(cancellationToken);

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<string> RandomLink(int howMany, CancellationToken cancellationToken = default)
        {
            if( howMany <= 0)
            {
                return null;
            }

            var randomWords = await _dbContext.BookWords
                .OrderByDescending(x => EF.Functions.Random()) 
                .Take(howMany) 
                .Select(x => x.Word) 
                .ToListAsync(cancellationToken);

            if (randomWords == null || randomWords.Count == 0)
            {
                return null;
            }

            string linkSegment = string.Join("-", randomWords);


            return linkSegment;

        }

        public async Task RemoveBookWord(string bookname, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.BookWords.SingleOrDefaultAsync(x => x.Word == bookname, cancellationToken);

            if (result == null)
            {
                return;
            }

            _dbContext.BookWords.Remove(result);
        }
    }
}
