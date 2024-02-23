using System;

namespace Echo.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

        public User(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username must not be null or empty.", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password must not be null or empty.", nameof(password));
            }

            Id = Guid.NewGuid();
            Username = username;
            Password = password;
            CreatedAt = DateTime.Now;
        }
    }
}
