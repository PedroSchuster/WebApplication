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

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<UserRelation> UserRelations { get; set; }

        public DbSet<UserLikedPost> UserLikedPosts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-76E2JNA;Initial Catalog=SocialMediaDB;MultipleActiveResultSets=true;" +
                                        "TrustServerCertificate=True;Integrated Security=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostComment>()
                .HasOne(x => x.Comment)
                .WithMany()
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostComment>()
                .HasOne(x => x.Post)
                .WithMany(x => x.PostComments)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<UserRelation>()
            .HasOne(x => x.Following)
            .WithMany()
            .HasForeignKey(x => x.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);
           
            modelBuilder.Entity<UserRelation>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRelations)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Post>()
                .HasOne(x=>x.User)
                .WithMany(x=>x.Posts)
                .HasForeignKey(x=>x.UserId);

        }

    }
}
