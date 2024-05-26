using Microsoft.EntityFrameworkCore;
using CleverTagProject.Models;

namespace CleverTagProject.DataContext
{
    public class CleverTagContext : DbContext
    {
        public CleverTagContext(DbContextOptions<CleverTagContext> options)
        : base(options)
        {
        
        }

        public DbSet<CleverTag> CleverTags { get; set; }
    }
}
