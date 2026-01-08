using System;

namespace LostAndFound.Models
{
    public class User
    {
        // Properties matching the Users table columns
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }

        // Constructor - runs when you create a new User object
        public User()
        {
            CreatedDate = DateTime.Now;
            IsAdmin = false;
        }

        // Constructor with parameters - for creating users with initial values
        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
            CreatedDate = DateTime.Now;
            IsAdmin = false;
        }
    }
}