using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
 
using Echo.Enum;
using Echo.Interfaces;

namespace Echo.Models
{
	[Table("Chats")]
    public class Chat : ITrackingEntity
    {
        public Guid Id { get; set; }

		[Required]
        public string Name { get; set; }

		public ChatType Type { get; set; } = ChatType.Plain;
		
		public ICollection<User> Owners { get; set; }
		public ICollection<User> Members { get; set; }
        public ICollection<Message> Messages { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
