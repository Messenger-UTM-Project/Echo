using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using Echo.Enum;
using Echo.Interfaces;

namespace Echo.Models
{
	[Table("Users")]
	[Comment("Authorised User Model")]
    public class Friendship : ITrackingEntity
    {
        public Guid Id { get; set; }

		public Guid User1Id { get; set; }
		public User User1 { get; set; }

		public Guid User2Id { get; set; }
		public User User2 { get; set; }

		public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;

		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	}
}
