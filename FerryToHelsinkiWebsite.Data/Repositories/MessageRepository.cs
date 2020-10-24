using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FerryToHelsinkiWebsite.Data.Repositories
{
    public class MessageRepository
    {
        private readonly FerryToHelsinkiDBContext _dbContext;

        public MessageRepository(FerryToHelsinkiDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _dbContext.Messages.ToListAsync();
        }

        public async Task CreateMessageAsync(Message message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }

    }
}
