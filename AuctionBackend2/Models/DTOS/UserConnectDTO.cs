using System.Text.Json.Serialization;
using AuktionBackend.Models.Entities;

namespace Groupwork_my_version_.Models.DTOS
{
    public class UserConnectDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }


        [JsonIgnore]
        
        public List<Auction> Auctions { get; set; }

        [JsonIgnore]
        public List<Bid> Bids { get; set; }
    }
}
