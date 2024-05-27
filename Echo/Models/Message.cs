using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using Echo.Interfaces;

namespace Echo.Models
{
	[Table("Messages")]
    public class Message : ITrackingEntity
    {
        public Guid Id { get; set; }

		[Required]
        public Guid UserId { get; set; }
		public User User { get; set; }
		
		[Required]
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
		
		[Required]
		[MaxLength(300)]
		public string Content { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
