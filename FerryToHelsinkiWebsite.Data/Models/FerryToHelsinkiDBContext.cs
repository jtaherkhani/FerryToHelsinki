using Microsoft.EntityFrameworkCore;

namespace FerryToHelsinkiWebsite.Data.Models
{
    public class FerryToHelsinkiDBContext : DbContext
    {
        public FerryToHelsinkiDBContext (DbContextOptions<FerryToHelsinkiDBContext> options)
            : base(options)
        { 
        }

        public DbSet<Message> Messages { get; set; }
    }
}
