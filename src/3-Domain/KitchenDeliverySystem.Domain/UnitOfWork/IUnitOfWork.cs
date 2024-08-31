using KitchenDeliverySystem.Domain.Repositories;

namespace KitchenDeliverySystem.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        Task RollbackAscync();
        Task SaveChangesAsync();
    }
}
