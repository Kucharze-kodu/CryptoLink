using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Architecture.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CryptoLinkDbContext _dbContext;

        public UnitOfWork(CryptoLinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
