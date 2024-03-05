using System;
using Microsoft.AspNetCore.Identity;

using Echo.Interfaces;

namespace Echo.Models
{
    public class User : IdentityUser<Guid>, ITrackingEntity
    {
		public bool Active { get; set; } = true;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public bool IsActive()
		{
			return Active;
		}
	}
}
