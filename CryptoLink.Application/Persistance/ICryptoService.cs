using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Persistance
{
    public interface ICryptoService
    {
        Task<(string ChallengeToken, string EncryptedMessage)> GenerateChallengeAsync(string userPublicKey);
        string? ExtractUserIdFromPublicKey(string publicKey);
        bool ValidatePublicKey(string publicKey);
    }
}
