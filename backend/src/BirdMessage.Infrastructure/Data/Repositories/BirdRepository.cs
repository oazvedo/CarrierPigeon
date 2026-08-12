using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Infrastructure.Data.Repositories;

public class BirdRepository(AppDbContext context) : RepositoryBase<Bird>(context), IBirdRepository;
