using Microsoft.EntityFrameworkCore;
using Echo.Models;

namespace Echo.Data
{
	public class EchoDbContext : DbContext
	{
		public EchoDbContext(DbContextOptions<EchoDbContext> options) : base(options)
        {
        }

		public DbSet<EchoUser> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EchoUser>()
				.HasKey(e => e.Id);
		}
	}
}
