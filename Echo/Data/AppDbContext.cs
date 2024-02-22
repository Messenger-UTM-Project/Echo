using Microsoft.EntityFrameworkCore;
using Echo.Models;

namespace Echo.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasKey(e => e.Id);
		}
	}
}
