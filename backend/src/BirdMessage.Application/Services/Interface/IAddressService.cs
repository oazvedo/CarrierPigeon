using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Services.Interface
{
    public interface IAddressService
    {
        Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Address>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Address> CreateAsync(Address address, CancellationToken cancellationToken = default);
        Task UpdateAsync(Address address, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}