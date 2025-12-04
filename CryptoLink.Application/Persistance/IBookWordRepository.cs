using CryptoLink.Domain.Aggregates.BookWords;

namespace CryptoLink.Application.Persistance
{
    public interface IBookWordRepository
    {
        Task AddBookWord(BookWord bookWord, CancellationToken cancellationToken = default);
        Task RemoveBookWord(string bookname, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookWord>> GetAllBookWordCategory(string category, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookWord>> GetAllBookWords(CancellationToken cancellationToken = default);
        Task<string> RandomLink(int howMany, CancellationToken cancellationToken = default);
    }
}
