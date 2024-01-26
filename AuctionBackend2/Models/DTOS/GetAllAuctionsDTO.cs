namespace Groupwork_my_version_.Models.DTOS
{
    public class GetAllAuctionsDTO
    {
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public string Status
        {
            get
            {
                return DateTime.UtcNow > EndDate ? "Closed" : "Open";
            }
        }
    }
}
