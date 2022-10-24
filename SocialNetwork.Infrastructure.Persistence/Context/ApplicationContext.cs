using SocialNetwork.Core.Domain.Common;
using SocialNetwork.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Infrastructure.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Posts>? Posts { get; set; }
        public DbSet<Comments>? Comments { get; set; }
        public DbSet<Users>? Users { get; set; }
        public DbSet<Friends>? Friends { get; set; }

        //public override Posts<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        //{
        //    foreach(var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.Entity.Created = DateTime.Now;
        //                entry.Entity.CreateBy = "DefaultAppUser";
        //                break;

        //            case EntityState.Modified:
        //                entry.Entity.LastModified = DateTime.Now;
        //                entry.Entity.LastModifiedBy = "DefaultAppUser";
        //                break;
        //        }
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API

            #region tables
            modelBuilder.Entity<Posts>().ToTable("Posts");
            modelBuilder.Entity<Comments>().ToTable("Comments");
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Friends>().ToTable("Friends");
            #endregion

            #region "primary keys"
            modelBuilder.Entity<Posts>().HasKey(post => post.Id);
            modelBuilder.Entity<Comments>().HasKey(comment => comment.Id);
            modelBuilder.Entity<Users>().HasKey(user => user.Id);
            modelBuilder.Entity<Friends>().HasKey(friend => friend.Id);
            #endregion

            #region relationships

            modelBuilder.Entity<Users>()
                 .HasMany(user => user.Posts)
                 .WithOne(post => post.Users)
                 .HasForeignKey(post => post.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                 .HasMany(user => user.Comments)
                 .WithOne(comment => comment.Users)
                 .HasForeignKey(comment => comment.UserId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Posts>()
                .HasMany(post => post.Comments)
                .WithOne(comment => comment.Posts)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasMany(friend => friend.Friends)
                .WithOne(comment => comment.Users)
                .HasForeignKey(comment => comment.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region "property configurations"

            #region posts
            modelBuilder.Entity<Posts>()
                .Property(post => post.Content)
                .IsRequired();

            modelBuilder.Entity<Posts>()
                .Property(post => post.ImagePath)
                .IsRequired();

            modelBuilder.Entity<Posts>()
                .Property(post => post.UserId)
                .IsRequired();
            #endregion

            #region comments
            modelBuilder.Entity<Comments>()
                .Property(comment => comment.Content)
                .IsRequired();

            modelBuilder.Entity<Comments>()
                .Property(comment => comment.UserId)
                .IsRequired();

           modelBuilder.Entity<Comments>()
                .Property(comment => comment.PostId)
                .IsRequired();

           modelBuilder.Entity<Comments>()
                .Property(comment => comment.Created)
                .IsRequired();
            #endregion

            #region users
            modelBuilder.Entity<Users>()
                .HasIndex(user => user.Username)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Users>()
                .Property(user => user.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Users>()
                .Property(user => user.ProfileImage)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Users>()
                .Property(user => user.Phone)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Users>()
                .Property(user => user.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Users>()
                .Property(user => user.Password)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(friend => friend.IsActive)
                .HasDefaultValue(false);
            #endregion

            #region friends
            modelBuilder.Entity<Friends>()
                .Property(friend => friend.SenderId)
                .IsRequired();

            modelBuilder.Entity<Friends>()
                .Property(friend => friend.ReceptorId)
                .IsRequired();

            modelBuilder.Entity<Friends>()
                .Property(friend => friend.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Friends>()
                .Property(friend => friend.IsAccepted)
                .HasDefaultValue(false);

            #endregion

            #endregion
        }

    }
}
