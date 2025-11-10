using SecuTrack.Core.Entities;

namespace SecuTrack.Core.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        Task<IEnumerable<Provider>> GetProvidersWithAuditsAsync();
        Task<Provider?> GetProviderWithAuditsAsync(int id);
    }
}