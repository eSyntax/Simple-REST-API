using Microsoft.EntityFrameworkCore;

namespace Simple_API.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

    }
}
