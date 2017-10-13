using ListsWebAPi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ListsWebAPi.Entity
{
    /// <summary>
    /// The Database context for the Entity Framework so that the classes can be generated to schemas and tables after database Migration
    /// </summary>
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<ListItem> ListItem { get; set; }
        public DbSet<Lists> Lists { get; set; }
        
    }
}