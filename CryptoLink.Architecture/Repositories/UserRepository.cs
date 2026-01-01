using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Domain.Aggregates.Users;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;




namespace CryptoLink.Architecture.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptoLinkDbContext _dbContext;

        public UserRepository(CryptoLinkDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task<User?> GetByEmailAsync(string name, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

            return user;
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(r => r.Id.Equals(UserId.Create(id)), cancellationToken);

            return user;
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext
                .Users
                .AddAsync(user, cancellationToken);
        }

        public Task<bool> AnyUserAsync(string name, CancellationToken cancellationToken = default)
        {
            return _dbContext
                .Users
                .AnyAsync(r => r.Name == name, cancellationToken);
        }

        public async Task<int?> GetIdByEmailAsync(string name, CancellationToken cancellationToken = default)
        {
            var userId = await _dbContext
                .Users
                .Where(r => r.Name == name)
                .Select(r => r.Id.Value)
                .FirstOrDefaultAsync(cancellationToken);

            return userId;
        }

        public Task<string> CreateAccount(string email, string publicKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> Challenge(string email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task VerifyChallenge(string email, string challengeResponse, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
