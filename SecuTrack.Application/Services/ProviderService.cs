using AutoMapper;
using SecuTrack.Application.DTOs;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Interfaces;

namespace SecuTrack.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProviderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllProvidersAsync()
        {
            var providers = await _unitOfWork.Providers.GetProvidersWithAuditsAsync();
            return _mapper.Map<IEnumerable<ProviderDto>>(providers);
        }

        public async Task<ProviderDto?> GetProviderByIdAsync(int id)
        {
            var provider = await _unitOfWork.Providers.GetProviderWithAuditsAsync(id);
            return provider != null ? _mapper.Map<ProviderDto>(provider) : null;
        }

        public async Task<ProviderDto> CreateProviderAsync(CreateProviderDto dto)
        {
            var provider = _mapper.Map<Provider>(dto);
            await _unitOfWork.Providers.AddAsync(provider);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProviderDto>(provider);
        }

        public async Task<bool> UpdateProviderAsync(int id, CreateProviderDto dto)
        {
            var provider = await _unitOfWork.Providers.GetByIdAsync(id);
            if (provider == null) return false;

            provider.Name = dto.Name;
            provider.Type = dto.Type;
            provider.Region = dto.Region;
            provider.Description = dto.Description;
            provider.ContactEmail = dto.ContactEmail;
            provider.WebsiteUrl = dto.WebsiteUrl;
            provider.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Providers.Update(provider);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProviderAsync(int id)
        {
            var provider = await _unitOfWork.Providers.GetByIdAsync(id);
            if (provider == null) return false;

            provider.IsActive = false;
            provider.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Providers.Update(provider);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}