using System;
using System.ComponentModel.DataAnnotations;

namespace Echo.Enum
{
	public enum FriendshipStatus
	{
		[Display(Name = "Accepted")]
		Accepted = 1,
		[Display(Name = "Pending")]
		Pending = 2,
		[Display(Name = "Denied")]
		Denied = 3,
		[Display(Name = "Rejected")]
		Rejected = 4,
	}
}
