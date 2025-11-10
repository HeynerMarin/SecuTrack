using SecuTrack.Application.DTOs;

namespace SecuTrack.Application.Services
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderDto>> GetAllProvidersAsync();
        Task<ProviderDto?> GetProviderByIdAsync(int id);
        Task<ProviderDto> CreateProviderAsync(CreateProviderDto dto);
        Task<bool> UpdateProviderAsync(int id, CreateProviderDto dto);
        Task<bool> DeleteProviderAsync(int id);
    }
}