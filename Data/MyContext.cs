using LoginAuthDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAuthDemo.Data
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public MyContext(DbContextOptions<MyContext> options) :base(options) { }
    }
}
