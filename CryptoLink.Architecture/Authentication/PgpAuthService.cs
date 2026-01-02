using CryptoLink.Application.Persistance;
using Org.BouncyCastle.Bcpg.OpenPgp;
using PgpCore;
using System.Text;

public class PgpCryptoService : ICryptoService
{
    public async Task<(string ChallengeToken, string EncryptedMessage)> GenerateChallengeAsync(string userPublicKey)
    {
        string challengeToken = Guid.NewGuid().ToString();

        using (var publicKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(userPublicKey)))
        using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(challengeToken)))
        using (var outputStream = new MemoryStream())
        {
            var encryptionKeys = new EncryptionKeys(publicKeyStream);
            using var pgp = new PGP(encryptionKeys);

            // Szyfrowanie challenge'u kluczem publicznym użytkownika
            await pgp.EncryptAsync(inputStream, outputStream, armor: true, withIntegrityCheck: true);

            outputStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(outputStream))
            {
                string encryptedMessage = await reader.ReadToEndAsync();
                return (challengeToken, encryptedMessage);
            }
        }
    }

    public bool ValidatePublicKey(string publicKey)
    {
        try
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(publicKey)))
            {
                var keys = new EncryptionKeys(stream);
                return keys.PublicKeys.Any();
            }
        }
        catch
        {
            return false;
        }
    }

    public string? ExtractUserIdFromPublicKey(string publicKey)
    {
        try
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(publicKey)))
            {
                var keys = new EncryptionKeys(stream);

                foreach (PgpPublicKey key in keys.PublicKeys)
                {
                    // GetUserIds() returns an iterator of strings
                    foreach (string userId in key.GetUserIds())
                    {
                        // Return the first non-empty user identifier encountered
                        if (!string.IsNullOrWhiteSpace(userId))
                        {
                            return userId;
                        }
                    }
                }
            }
            return null; 
        }
        catch
        {
            return null; 
        }
    }
}