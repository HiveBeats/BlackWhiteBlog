using Microsoft.EntityFrameworkCore;

namespace BlackWhiteBlog.DomainModel.Context
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext()
        {
            
        }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
            
        }
    }
}