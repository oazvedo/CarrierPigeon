using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Infrastructure.Data.Repositories
{
    public class UserRepository(AppDbContext context) : RepositoryBase<User>(context), IUserRepository;
}