using AuktionBackend.Models.DTOS;
using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using Dapper;
using Groupwork_my_version_.Models.DTOS;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace AuktionBackend.Repository.Repos
{
  
    public class BidRepo:IBidRepo
    {
        
        private readonly IJensenContext _context;
        public BidRepo(IJensenContext context)
        {
            _context = context;
        }

        public List<Bid> GetBids()
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var bids = db.Query<Bid, Auction, UserConnectDTO, Bid>(
                    "ShowAllBids",
                    (bid, auction, user) =>
                    {
                        bid.Auction = auction;
                        bid.BidUser = user;
                        return bid;
                    },
                    splitOn: "AuctionId, UserId",
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return bids;
            }
        }
        public Bid GetBidByID(int bidId)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var bid = db.Query<Bid, UserConnectDTO, Bid>(
                    "GetBidById",
                    (b, u) =>
                    {
                        b.BidUser = u;
                        return b;
                    },
                    new { BidId = bidId },
                    splitOn: "UserId",
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();

                return bid;
            }
        }

        public void Delete(int BidID)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var parametara = new DynamicParameters();
                parametara.Add("@BidID", BidID);
                db.Execute("BidDelete", parametara,
                  commandType: CommandType.StoredProcedure);

            }
        }
        public void InsertBid(int AuctionID,int UserID, BidDTO bid,DateTime BidTime)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AucktionID", AuctionID);
                parameters.Add("@UserID", UserID);
                parameters.Add("@Price", bid.Price);
                parameters.Add("@BidTime", BidTime);
                db.Execute("BidInsert", parameters, commandType: CommandType.StoredProcedure);
            }
        }
      
    }
}