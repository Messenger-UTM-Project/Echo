using System;

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

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasKey(e => e.Id);

			builder.Entity<User>()
				.HasIndex(u => u.UserName)
				.IsUnique();

			builder.Entity<UserRole>().HasData(
				new UserRole { Id = Guid.NewGuid(), ConcurrencyStamp = "1", RoleID = 1, Name = "Admin", NormalizedName = "ADMIN" },
				new UserRole { Id = Guid.NewGuid(), ConcurrencyStamp = "2", RoleID = 2, Name = "Staff", NormalizedName = "STAFF" },
				new UserRole { Id = Guid.NewGuid(), ConcurrencyStamp = "3", RoleID = 3, Name = "User", NormalizedName = "USER" }
			);
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

			return await base.SaveChangesAsync(cancellationToken);
		}
    }
}
