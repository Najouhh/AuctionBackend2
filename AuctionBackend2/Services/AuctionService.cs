using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Repository.Repos;
using Groupwork_my_version_.Models.DTOS;

namespace AuctionBackend.Services
{
    public class AuctionService :IAuctionRepo
    {
        private readonly IAuctionRepo _auctionRepo;
        public AuctionService(IAuctionRepo auctionRepo)
        {
            _auctionRepo = auctionRepo;
        }

        public string ChangeAuctionByID(GetAllAuctionsDTO auction)
        {
            throw new NotImplementedException();
        }

        public void CreateAuction(AuctionDTO auction, int UserID)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuctionByID(int auctionID)
        {
            throw new NotImplementedException();
        }

        public List<Auction> GetAllAuctions()
        {
            return _auctionRepo.GetAllAuctions();   
        }

        public Auction GetAuctionByID(int auctionID)
        {
            throw new NotImplementedException();
        }

        public Auction GetAuctionByIddD(int auctionID)
        {
            throw new NotImplementedException();
        }
    }
}
