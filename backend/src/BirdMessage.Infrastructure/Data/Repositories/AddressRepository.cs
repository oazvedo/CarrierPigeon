
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Infrastructure.Data.Repositories
{
    public class AddressRepository(AppDbContext context) : RepositoryBase<Address>(context), IAddressRepository {}
}