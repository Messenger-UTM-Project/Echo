using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using Echo.Interfaces;

namespace Echo.Models
{
	[Table("Users")]
	[Comment("Authorised User Model")]
    public class User : IdentityUser<Guid>, IUser, ITrackingEntity
    {
		[Required]
		[MaxLength(24)]
		[Column(Order = 1)]
		public string Name { get; set; }

		[Required]
		[MaxLength(24)]
		[Column(Order = 2)]
		public override string UserName { get; set; }
		
		[Required]
		[Column(Order = 3)]
		public override string PasswordHash { get; set; }

		[Required]
		[MaxLength(128)]
		public string ProfileImagePath { get; set; } = "/media/uploads/default_profile_image.png";

		[Column("Active")]
		public bool IsActive { get; set; } = true;

		public ICollection<Chat> OwnedChats { get; set; }
		public ICollection<Chat> MemberChats { get; set; }
		public ICollection<Message> Messages { get; set; }
		public ICollection<Friendship> InitiatedFriendships { get; set; }
		public ICollection<Friendship> ReceivedFriendships { get; set; }
		
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
