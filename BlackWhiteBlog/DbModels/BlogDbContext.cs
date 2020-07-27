using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace BlackWhiteBlog.DbModels
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
            //Database.EnsureCreated();
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostContent> PostContents { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.UserId);
                user.Property(u => u.UserName).IsRequired();
                user.Property(u => u.HashedPassword).IsRequired();
                user.HasIndex(u => u.UserName).IsUnique();
                user.HasIndex(u => new {u.UserName, u.HashedPassword});
                user.HasOne(u => u.Author)
                    .WithOne(a => a.User)
                    .HasForeignKey<Author>(u => u.AuthorId);
            });
            
            modelBuilder.Entity<Author>(author =>
            {
                author.HasKey(e => e.AuthorId);
                author.Property(e => e.AuthorName).IsRequired();
                author.Property(e => e.AuthorDesc).HasColumnType("text");
                
                author.HasOne(a => a.User)
                    .WithOne(u => u.Author)
                    .HasForeignKey<User>(u => u.UserId);
                
                author.HasMany<Post>()
                    .WithOne(p => p.Author)
                    .HasForeignKey(p=> p.AuthorId);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(p => p.PostId);
               
                post.HasIndex(p => p.PostDate);
                post.HasMany<PostContent>()
                    .WithOne(pc => pc.Post)
                    .HasForeignKey(pc => pc.PostId);
            });
            
            modelBuilder.Entity<PostContent>(postContent =>
            {
                postContent.Property(pc => pc.Title).IsRequired().HasMaxLength(1000);
                postContent.Property(pc => pc.Content).HasColumnType("text").IsRequired();
                postContent.Property(pc => pc.PostColor).IsRequired();
                postContent.HasIndex(pc => pc.PostColor);
                postContent.HasKey(pc => new {pc.PostId, pc.PostColor});
            });

           

        }
    }
}