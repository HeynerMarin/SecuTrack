namespace SecuTrack.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProviderRepository Providers { get; }
        IAuditRepository Audits { get; }
        IEvaluationRepository Evaluations { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}