using CryptoLink.Domain.Aggregates.BookWords;
using CryptoLink.Domain.Aggregates.BookWords.ValueObcjets;

namespace CryptoLink.Application.Persistance
{
    public interface IBookWordRepository
    {
        Task AddBookWord(BookWord bookWord, CancellationToken cancellationToken = default);
        Task RemoveBookWord(BookWordId IdBookWord, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookWord>> GetAllBookWordCategory(string category, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookWord>> GetAllBookWords(CancellationToken cancellationToken = default);
        Task<string> RandomLink(int howMany,List<string> categories, CancellationToken cancellationToken = default);
    }
}
