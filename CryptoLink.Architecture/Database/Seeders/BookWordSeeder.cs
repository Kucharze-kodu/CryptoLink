
using CryptoLink.Domain.Aggregates.BookWords;
using Microsoft.EntityFrameworkCore;


namespace CryptoLink.Architecture.Database.Seeders
{
    public class BookWordSeeder
    {
        public static async Task SeedAsync(CryptoLinkDbContext context)
        {
            if (await context.BookWords.AnyAsync())
            {
                return; 
            }

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;


            var folderPath = Path.Combine(baseDir, "Database", "Seeders", "Word");


            if (!Directory.Exists(folderPath))
            {
                var alternativePath = Path.Combine(baseDir, "Word");
                if (Directory.Exists(alternativePath))
                {
                    folderPath = alternativePath;
                }
                else
                {
                    throw new DirectoryNotFoundException($"Nie znaleziono folderu ze słownikami. Szukano w: {folderPath}");
                }
            }

            // 3. Pobierz wszystkie pliki .txt
            var files = Directory.GetFiles(folderPath, "*.txt");
            var wordsToAdd = new List<BookWord>();

            foreach (var filePath in files)
            {
                var categoryName = Path.GetFileNameWithoutExtension(filePath);

                var lines = await File.ReadAllLinesAsync(filePath);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    wordsToAdd.Add(BookWord.Create(categoryName, line.Trim()));
                }
            }


            if (wordsToAdd.Any())
            {
                await context.BookWords.AddRangeAsync(wordsToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}