using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Infrastructure.Data.Repositories
{
    public class MessageRepository(AppDbContext context) : RepositoryBase<Message>(context), IMessageRepository
    {
        
    }
}