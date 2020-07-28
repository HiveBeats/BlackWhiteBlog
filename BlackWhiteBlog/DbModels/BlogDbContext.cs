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
            Database.EnsureDeleted();
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
                user.Property(u => u.UserName).IsRequired().HasMaxLength(50);
                user.Property(u => u.HashedPassword).IsRequired().HasMaxLength(256);
                user.HasIndex(u => u.UserName).IsUnique();
                user.HasIndex(u => new {u.UserName, u.HashedPassword});
                
                user.HasOne(u => u.Author)
                    .WithOne(a => a.User)
                    .HasForeignKey<User>(u => u.AuthorId);
            });
            
            modelBuilder.Entity<Author>(author =>
            {
                author.HasKey(e => e.AuthorId);
                author.Property(e => e.AuthorName).IsRequired().HasMaxLength(256);
                author.Property(a => a.AuthorPicLink).HasMaxLength(256);
                author.Property(e => e.AuthorDesc).HasColumnType("text");

                /*author.HasOne(a => a.User)
                      .WithOne(u => u.Author)
                      .HasForeignKey<Author>(a => a.UserId);*/
                
                author.HasMany<Post>()
                    .WithOne(p => p.Author)
                    .HasForeignKey(p=> p.AuthorId);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(p => p.PostId);
               
                post.HasIndex(p => p.PostDate);
                //?
                post.HasOne(pc => pc.Author)
                    .WithMany(a => a.Posts)
                    .HasForeignKey(a => a.AuthorId);

                /*post.HasMany(p => p.PostContents)
                    .WithOne(pc => pc.Post);*/
            });
            
            modelBuilder.Entity<PostContent>(postContent =>
            {
                postContent.Property(pc => pc.Title).IsRequired().HasMaxLength(256);
                postContent.Property(pc => pc.Content).HasColumnType("longtext").IsRequired();
                postContent.Property(pc => pc.ImageLink).HasMaxLength(256);
                postContent.Property(pc => pc.PostColor).IsRequired();
                postContent.HasIndex(pc => pc.PostColor);
                postContent.HasIndex(pc => new {pc.PostId, pc.PostColor}).IsUnique();

                postContent.HasOne(pc => pc.Post)
                           .WithMany(p => p.PostContents);
                /*
                postContent.HasOne<Post>()
                    .WithMany(p => p.PostContents)
                    .HasForeignKey(p => p.PostId);
                    */
            });

           

        }
    }
}