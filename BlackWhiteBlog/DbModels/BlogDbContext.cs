using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace BlackWhiteBlog.DbModels
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostContent> PostContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>(author =>
            {
                author.HasKey(e => e.AuthorId);
                author.Property(e => e.AuthorName).IsRequired();
                author.Property(e => e.AuthorDesc).HasColumnType("text");
                author.HasMany<Post>()
                    .WithOne(p => p.Author);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(p => p.PostId);
               
                post.HasIndex(p => p.PostDate);
                post.HasMany<PostContent>()
                    .WithOne(pc => pc.Post);
            });
            
            modelBuilder.Entity<PostContent>(postContent =>
            {
                postContent.Property(pc => pc.Title).IsRequired().HasMaxLength(1000);
                postContent.Property(pc => pc.Content).HasColumnType("text").IsRequired();
                postContent.Property(pc => pc.PostColor).IsRequired();
                postContent.HasIndex(pc => pc.PostColor);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.UserId);
                user.Property(u => u.UserName).IsRequired();
                user.Property(u => u.HashedPassword).IsRequired();
                user.HasIndex(u => u.UserName).IsUnique();
                user.HasIndex(u => new {u.UserName, u.HashedPassword});
                user.HasOne<Author>();
            });

        }
    }
}