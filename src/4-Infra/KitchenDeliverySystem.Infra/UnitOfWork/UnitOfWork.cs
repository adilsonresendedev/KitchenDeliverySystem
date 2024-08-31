using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Infra.Context;

namespace KitchenDeliverySystem.Infra.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _appDbContext.Database.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
            await _appDbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackAscync()
        {
            await _appDbContext.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext?.Dispose();
        }
    }
}
