using System.Text.Json.Serialization;
using Groupwork_my_version_.Models.DTOS;

namespace AuktionBackend.Models.Entities
{
    public class Auction
    {
       
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserConnectDTO Auctionuser { get; set; }
        
        public List<Bid> Bids { get; set; }

        public string Status
        {
            get
            {
                return DateTime.UtcNow > EndDate ? "Closed" : "Open";
            }
        }
      
    }
}
