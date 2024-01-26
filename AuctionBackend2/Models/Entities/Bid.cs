
using System.Text.Json.Serialization;
using Groupwork_my_version_.Models.DTOS;

namespace AuktionBackend.Models.Entities
{
    public class Bid
    {

        public int BidId { get; set; }
        public decimal Price { get; set; }
        public DateTime BidTime { get; set; }
       
        [JsonIgnore]
        public Auction Auction { get; set; }
         
        public UserConnectDTO BidUser { get; set; }

       
    }
}
