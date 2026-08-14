
using BirdMessage.Domain.Entities;

namespace BirdMessage.Domain.Interfaces
{
    public interface IAddressRepository : IRepositoryBase<Address> 
    { 
        Task<Address?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}