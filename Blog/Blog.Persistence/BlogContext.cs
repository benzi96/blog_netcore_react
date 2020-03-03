using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Persistence
{
    public class BlogContext : DbContext
    {
        public DbSet<FileUpload> FileUploads { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostTag>()
                .HasKey(bc => new { bc.PostId, bc.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(bc => bc.Post)
                .WithMany(b => b.PostTags)
                .HasForeignKey(bc => bc.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.PostTags)
                .HasForeignKey(bc => bc.TagId);

            modelBuilder.Entity<Post>()
                .HasOne(c => c.Author)
                .WithMany(d => d.Posts)
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<Category>().HasData(
                    new Category() { Id = Guid.NewGuid(), Name = "Technology" },
                    new Category() { Id = Guid.NewGuid(), Name = "Life" },
                    new Category() { Id = Guid.NewGuid(), Name = "Culture" },
                    new Category() { Id = Guid.NewGuid(), Name = "Science" },
                    new Category() { Id = Guid.NewGuid(), Name = "Other" });
        }
    }
}
