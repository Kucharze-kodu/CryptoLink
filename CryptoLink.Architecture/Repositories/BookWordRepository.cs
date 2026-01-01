using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Domain.Aggregates.BookWords;
using CryptoLink.Domain.Aggregates.BookWords.ValueObcjets;
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

        public async Task<IEnumerable<BookWord>> GetAllBookWordCategory(List<string> category, CancellationToken cancellationToken = default)
        {
            List<BookWord>? result = new List<BookWord?>();

            if (category == null || category.Count == 0)
            {
                result = await _dbContext.BookWords.ToListAsync(cancellationToken);
            }
            else
            {
                result = await _dbContext.BookWords
                        .Where(x => category.Contains(x.Category))
                        .ToListAsync(cancellationToken);
            }

            if (result == null)
            {
                return null;
            }

            return result;

        }

        public async Task<IEnumerable<string>> GetAllCategory(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.BookWords
                            .Select(x => x.Category) 
                            .Distinct()
                            .ToListAsync(cancellationToken);

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<string> RandomLink(int howMany, List<string> categories, CancellationToken cancellationToken = default)
        {
            if( howMany <= 0)
            {
                return "404";
            }

            var randomWords = await _dbContext.BookWords
                .Where(x => categories.Contains(x.Category))
                .OrderBy(x => EF.Functions.Random())
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

        public async Task RemoveBookWord(BookWordId idBookWord, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.BookWords.SingleOrDefaultAsync(x => x.Id == idBookWord, cancellationToken);

            if (result == null)
            {
                return;
            }

            _dbContext.BookWords.Remove(result);
        }
    }
}
