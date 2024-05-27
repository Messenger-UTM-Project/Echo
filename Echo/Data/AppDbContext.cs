using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Echo.Roles;
using Echo.Models;
using Echo.Interfaces;

namespace Echo.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
			{
				entity.ToTable("Users");
                entity.HasKey(e => e.Id);
				entity.HasIndex(u => u.UserName)
					.IsUnique();
				entity.Property(e => e.UpdatedAt).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
			});


            builder.Entity<Friendship>(entity =>
			{
				entity.ToTable("Friendships");
                entity.HasKey(e => e.Id);
				entity.HasOne(f => f.User1)
					.WithMany(u => u.InitiatedFriendships)
					.HasForeignKey(f => f.User1Id)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(f => f.User2)
					.WithMany(u => u.ReceivedFriendships)
					.HasForeignKey(f => f.User2Id)
					.OnDelete(DeleteBehavior.Restrict);
			});

			builder.Entity<Chat>(entity =>
			{
				entity.ToTable("Chats");
                entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired().HasMaxLength(24);
				entity.HasMany(c => c.Owners)
					.WithMany(u => u.OwnedChats)
					.UsingEntity<Dictionary<Guid, object>>(
						"ChatOwner",
						j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
						j => j.HasOne<Chat>().WithMany().HasForeignKey("ChatId")
					);
				entity.HasMany(c => c.Members)
					.WithMany(u => u.MemberChats)
					.UsingEntity<Dictionary<Guid, object>>(
						"ChatMember",
						j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
						j => j.HasOne<Chat>().WithMany().HasForeignKey("ChatId")
					);
				entity.Property(e => e.UpdatedAt).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
			});

			builder.Entity<Message>(entity =>
			{
				entity.HasOne(e => e.Chat)
					.WithMany(c => c.Messages)
					.HasForeignKey(e => e.ChatId);
				entity.HasOne(e => e.User)
					.WithMany(c => c.Messages)
					.HasForeignKey(e => e.UserId);
				entity.ToTable("Messages");
                entity.HasKey(e => e.Id);
				entity.Property(e => e.Content).IsRequired().HasMaxLength(300);
				entity.Property(e => e.UpdatedAt).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
			});

			builder.Entity<UserRole>(entity =>
			{
				entity.HasData(
					new UserRole { Id = Guid.NewGuid(), ConcurrencyStamp = "1", RoleID = 1, Name = "Admin", NormalizedName = "ADMIN" },
					new UserRole { Id = Guid.NewGuid(), ConcurrencyStamp = "2", RoleID = 2, Name = "Staff", NormalizedName = "STAFF" },
					new UserRole { Id = Guid.NewGuid(), ConcurrencyStamp = "3", RoleID = 3, Name = "User", NormalizedName = "USER" }
				);
			});
        }

		public override int SaveChanges()
		{
			ChangeTracker.DetectChanges();

			var now = DateTime.UtcNow;

			foreach (var entry in ChangeTracker.Entries<ITrackingEntity>())
			{
				if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedAt = now;
				}
			}

			ValidateUserFriendships();

			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ChangeTracker.DetectChanges();

			var now = DateTime.UtcNow;

			foreach (var entry in ChangeTracker.Entries<ITrackingEntity>())
			{
				if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedAt = now;
				}
			}

			ValidateUserFriendships();

			return await base.SaveChangesAsync(cancellationToken);
		}

		private void ValidateUserFriendships()
		{
			foreach (var entry in ChangeTracker.Entries<Friendship>())
			{
				if (entry.State == EntityState.Added && entry.Entity.User1Id == entry.Entity.User2Id)
				{
					throw new InvalidOperationException("A user cannot be friends with themselves.");
				}
			}
		}
    }
}
