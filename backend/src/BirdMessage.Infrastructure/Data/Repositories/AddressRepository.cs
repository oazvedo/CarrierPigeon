
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BirdMessage.Infrastructure.Data.Repositories
{
    public class AddressRepository(AppDbContext context) : RepositoryBase<Address>(context), IAddressRepository
    {
        public async Task<Address?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
    }
}