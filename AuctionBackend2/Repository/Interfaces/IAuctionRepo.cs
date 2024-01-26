using AuktionBackend.Models.Entities;
using Groupwork_my_version_.Models.DTOS;

namespace AuktionBackend.Repository.Interfaces
{
    public interface IAuctionRepo
    {
        Auction GetAuctionByID(int auctionID);
        public string ChangeAuctionByID(GetAllAuctionsDTO auction);
        public List<Auction> GetAllAuctions();
        //List<UserConnectDTO> GetAllAuctions();
        void DeleteAuctionByID(int auctionID);
       // void ChangeAuctionByID(UpdateDTO auction);
        void CreateAuction(AuctionDTO auction, int UserID);
        Auction GetAuctionByIddD(int auctionID);
    }
}
