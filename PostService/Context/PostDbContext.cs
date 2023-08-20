using Microsoft.EntityFrameworkCore;
using PostService.Entity;

namespace PostService.Context
{
    public class PostDbContext: DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext>options):base(options)
        {
                
        }

        public DbSet<Post> Posts{ get; set; }
        public DbSet<User> Users{ get; set; }
    }
}
