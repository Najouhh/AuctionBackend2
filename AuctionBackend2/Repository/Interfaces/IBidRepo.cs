using AuktionBackend.Models.DTOS;
using AuktionBackend.Models.Entities;

namespace AuktionBackend.Repository.Interfaces
{
    public interface IBidRepo 
    {
        public List<Bid> GetBids();
        void Delete(int BidID);
        void InsertBid(int AuctionID, int UserID, BidDTO bid, DateTime BidTime);
        Bid GetBidByID(int bidId);
    }
}
