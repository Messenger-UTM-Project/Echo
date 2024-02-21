using System;

namespace Echo.Models
{
    public class EchoUser
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    } 
}
