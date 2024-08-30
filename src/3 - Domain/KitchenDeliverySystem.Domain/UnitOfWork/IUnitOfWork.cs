using KitchenDeliverySystem.Domain.Repositories;

namespace KitchenDeliverySystem.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task CommitAsync();
        Task RollbackAscync();
        Task SaveChangesAsync();
    }
}
