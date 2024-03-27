using Microsoft.EntityFrameworkCore;
using ToDoList.Backend.Domain.Entity;

namespace ToDoList.Backend.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<TaskEntity> Tasks { get; set; }
    }
}
