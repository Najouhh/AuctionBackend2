using System.Text.Json.Serialization;

namespace AuktionBackend.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}