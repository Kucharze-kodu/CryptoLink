using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Domain.Aggregates.Users;
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

        public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

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
    }
}
