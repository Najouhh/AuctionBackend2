using System.Text.Json.Serialization;
using AuktionBackend.Models.Entities;

namespace AuktionBackend.Models.DTOS
{
    public class UserPostDTO
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
       
    }
}
