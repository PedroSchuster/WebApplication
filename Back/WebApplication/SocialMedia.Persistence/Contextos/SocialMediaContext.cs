using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;

namespace SocialMedia.Persistence.Contextos
{
    public class SocialMediaContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public SocialMediaContext(DbContextOptions<SocialMediaContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-76E2JNA;Initial Catalog=SocialMediaDB;MultipleActiveResultSets=true;" +
                                        "TrustServerCertificate=True;Integrated Security=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
