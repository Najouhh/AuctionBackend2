using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Net;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Models.Entities;

using Groupwork_my_version_.Models.DTOS;


namespace AuktionBackend.Repository.Repos
{
    public class AuctionRepo : IAuctionRepo
    {

        private readonly IJensenContext _context;
        public AuctionRepo(IJensenContext context)
        {
            _context = context;
        }
     
        public List<Auction> GetAllAuctions()
        {
            var query = "[dbo].[GetAllAuctions]";
            var auctionDictionary = new Dictionary<int, Auction>();

            using (IDbConnection db = _context.GetConnection())
            {
                var result = db.Query<Auction, UserConnectDTO, Bid, Auction>(
                    query,
                    (auction, user, bid) =>
                    {
                        Auction auctionEntry;
                        if (!auctionDictionary.TryGetValue(auction.AuctionId, out auctionEntry))
                        {
                            auctionEntry = auction;
                            auctionEntry.Auctionuser = user;
                            auctionEntry.Bids = new List<Bid>();
                            auctionDictionary.Add(auction.AuctionId, auctionEntry);
                        }

                        if (user != null)
                        {
                            auctionEntry.Auctionuser = user;
                        }

                        if (bid != null)
                        {
                            auctionEntry.Bids.Add(bid);
                        }

                        return auctionEntry;
                    },
                    splitOn: "AuctionId,UserId,BidId"
                );

                var distinctAuctions = result.GroupBy(a => a.AuctionId).Select(g => g.First()).ToList();
                return distinctAuctions;
            }
        }


        public void CreateAuction(AuctionDTO auction, int UserID)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                db.Open();

                var parameters = new
                {
                    auction.Title,
                    auction.Description,
                    auction.Price,
                    auction.StartDate,
                    auction.EndDate,
                    auction.Status,
                    userID = UserID
                };

                db.Execute("InsertAuction", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public Auction GetAuctionByID(int auctionID)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var parameters = new { AuctionID = auctionID };
                var result = db.Query<Auction, UserConnectDTO, Auction>(
                    "GetAuctionById",
                    (auction, user) =>
                    {
                        auction.Auctionuser = user;
                        return auction;
                    },
                    parameters,
                    splitOn: "UserId",
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();

                return result;
            }
        }

        


        public string ChangeAuctionByID(GetAllAuctionsDTO auction)
        {
            using (IDbConnection db = _context.GetConnection())

            {
                db.Open();

                var parameters = new
                {
                    AuctionID = auction.AuctionId,
                    Title = auction.Title,
                    Description = auction.Description,
                    Price = auction.Price,
                    StartDate = auction.StartDate,
                    EndDate = auction.EndDate,
                    UserID = auction.UserId
                };

                var resultMessage = db.QueryFirstOrDefault<string>("ChangeAuctionByID", parameters, commandType: CommandType.StoredProcedure);

                return resultMessage ?? "An unexpected error occurred.";
            }
        }


        
        public void DeleteAuctionByID(int auctionID)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                db.Open();

                var parameters = new { AuctionID = auctionID };

                db.Execute("DeleteAuctionByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

      

        public Auction GetAuctionByIddD(int auctionID)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var parameters = new { AuctionID = auctionID };

                Auction auction = null;

                var result = db.Query<Auction, Bid, Auction>(
                    "GetAuctionInformation",
                    (auctionResult, bid) =>
                    {
                        if (auction == null)
                        {
                            auction = auctionResult;
                            auction.Bids = new List<Bid>();
                        }

                        if (bid != null)
                        {
                            auction.Bids.Add(bid);
                        }

                        return auction;
                    },
                    parameters,
                    splitOn: "UserId",
                    commandType: CommandType.StoredProcedure
                );

                if (auction == null) 
                {
                    return null;
                }

                
                if (auction.Status == "Closed" && (auction.Bids == null || auction.Bids.Count == 0))
                {
                    auction.Bids = new List<Bid>();
                }

                return result.FirstOrDefault();
            }
        }


    }
}

