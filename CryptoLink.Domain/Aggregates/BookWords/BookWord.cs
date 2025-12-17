using CryptoLink.Domain.Aggregates.BookWords.ValueObcjets;
using CryptoLink.Domain.Common.Primitives;


namespace CryptoLink.Domain.Aggregates.BookWords
{
    public sealed class BookWord : AggregateRoot<BookWordId>
    {
        public string Word { get; set; }
        public string Category { get; set; }


        private BookWord(BookWordId id) : base(id)
        {
        }


        private BookWord(
        BookWordId id,
        string word,
        string category) : base(id)
        {
            Word = word;
            Category = category;
        }

        public static BookWord Create(
            BookWordId id,
            string word,
            string category
        )
        {
            var bookWord = new BookWord(
                id,
                word,
                category
            );
            return bookWord;
        }

        public BookWord Load(
            string word,
            string category
        )
        {
            Word = word;
            Category = category;
            return this;
        }


    }
}