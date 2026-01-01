using CryptoLink.Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Persistance
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user, CancellationToken cancellationToken = default);
        Task<string> Challenge(string email, CancellationToken cancellationToken = default);

        Task VerifyChallenge(string email, string challengeResponse, CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> AnyUserAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int?> GetIdByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
