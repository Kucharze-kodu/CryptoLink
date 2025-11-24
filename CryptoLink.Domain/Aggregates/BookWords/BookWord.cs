using CryptoLink.Domain.Aggregates.BookWords.ValueObcjets;
using CryptoLink.Domain.Common.Primitives;
using System.Text.Json.Serialization;


namespace CryptoLink.Domain.Aggregates.BookWords
{
    public sealed class BookWord : AggregateRoot<BookWordId>
    {
        public string Word { get; set; }
        public string Category { get; set; }


        private BookWord(BookWordId id) : base(id)
        {
        }


        [JsonConstructor]
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
            return new BookWord(
                id: id,
                word: word,
                category: category
            );
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