namespace KitchenDeliverySystem.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        Task RollbackAscync();
        Task SaveChangesAsync();
    }
}
