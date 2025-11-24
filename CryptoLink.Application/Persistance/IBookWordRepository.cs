

namespace CryptoLink.Application.Persistance
{
    public interface IBookWordRepository
    {
        Task AddBookWord(string word, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetAllBookWordCategory(CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetAllBookWords(CancellationToken cancellationToken = default);
        Task<string> RandomLink(int howMany, CancellationToken cancellationToken = default);
    }
}
