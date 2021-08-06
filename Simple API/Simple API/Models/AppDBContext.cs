using Microsoft.EntityFrameworkCore;

namespace Simple_API.Models
{
    public class AppDBContext : DbContext
    {
        //DbContextOptions carries all the required configuration information such as the connection string, database provider, etc.
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        //Collection of all entities in the context, or that can be queried from the database, of a given type
        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
