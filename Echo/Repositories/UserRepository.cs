using Microsoft.EntityFrameworkCore;

using Echo.Data;
using Echo.Models;

namespace Echo.Repositories
{
    public class UserRepository
	{
		private readonly AppDbContext _dbContext;

		public UserRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			return await _dbContext.Users.ToListAsync();
		}
		
		public async Task<User> FindByIdAsync(Guid id)
		{
			return await _dbContext.Users.FindAsync(id);
		}
	}
}
