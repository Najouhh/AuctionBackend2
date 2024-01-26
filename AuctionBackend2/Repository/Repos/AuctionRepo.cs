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
        // this works
        //public List<Auction> GetAllAuctions()
        //{
        //    var query = "[dbo].[GetAllAuctions]";

        //    var auctionDictionary = new Dictionary<int, Auction>();

        //    using (IDbConnection db = _context.GetConnection())
        //    {
        //        var result = db.Query<Auction, UserConnectDTO, Bid, Auction>(
        //            query,
        //            (auction, user, bid) =>
        //            {
        //                Auction auctionEntry;
        //                if (!auctionDictionary.TryGetValue(auction.AuctionId, out auctionEntry))
        //                {
        //                    auctionEntry = auction;
        //                    auctionEntry.Auctionuser = user;
        //                    auctionEntry.Bids = new List<Bid>();
        //                    auctionDictionary.Add(auction.AuctionId, auctionEntry);
        //                }

        //                if (user != null)
        //                {
        //                    auctionEntry.Auctionuser = user;
        //                }

        //                if (bid != null)
        //                {
        //                    auctionEntry.Bids.Add(bid);
        //                }

        //                return auctionEntry;
        //            },
        //            splitOn: "AuctionId,UserId,BidId"
        //        );

        //        var auctions = result.ToList();
        //        return auctions;
        //    }
        //}
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

                // Retrieve distinct auctions
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

        //public Auction GetAuctionByID(int auctionID)
        //{
        //    using (IDbConnection db = _context.GetConnection())
        //    {
        //        var parameters = new { AuctionID = auctionID };
        //        var result = db.Query<Auction, UserConnectDTO, Auction>(
        //            "GetAuctionByIdd",
        //            (auction, user) =>
        //            {
        //                auction.Auctionuser = user;
        //                return auction;
        //            },
        //            parameters,
        //            splitOn: "UserId",
        //            commandType: CommandType.StoredProcedure
        //        ).FirstOrDefault();

        //        return result;
        //    }
        //}




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


        ////public void ChangeAuctionByID(UpdateDTO auction)
        ////{
        ////    using (IDbConnection db = _context.GetConnection())
        ////    {
        ////        db.Open();

        ////        var parameters = new
        ////        {
        ////            AuctionID = auction.AuctionId,
        ////            auction.Title,
        ////            auction.Description,
        ////            auction.Price,
        ////            auction.StartDate,
        ////            auction.EndDate,
        ////            auction.Status,
        ////            auction.Auctionuser.UserId
        ////        };


        ////        db.Execute("ChangeAuctionByID", parameters, commandType: CommandType.StoredProcedure);
        ////    }
        ////}

        public void DeleteAuctionByID(int auctionID)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                db.Open();

                var parameters = new { AuctionID = auctionID };

                db.Execute("DeleteAuctionByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        //test 
        //public Auction GetAuctionByIddD(int auctionID)
        //{
        //    using (IDbConnection db = _context.GetConnection())
        //    {
        //        var parameters = new { auctionID = auctionID };
        //        var results = db.Query<Auction, Bid, Auction>(
        //            "GetAuctionInformation",
        //            (auction, bid) =>
        //            {
        //                auction.Bids = auction.Bids ?? new List<Bid>();
        //                if (bid != null)
        //                {
        //                    auction.Bids.Add(bid);
        //                }
        //                return auction;
        //            },
        //            parameters,
        //            splitOn: "UserId",
        //            commandType: CommandType.StoredProcedure
        //        );

        //        return results.FirstOrDefault();
        //    }
        //}

        //        public Auction GetAuctionByIddD(int auctionID)
        //        {
        //{
        //    using (IDbConnection db = _context.GetConnection())
        //    {
        //        var parameters = new { AuctionID = auctionID };
        //        Auction auction = null;

        //        var result = db.Query<Auction, Bid, Auction>(
        //            "GetAuctionInformation",
        //            (auctionResult, bid) =>
        //            {
        //                if (auction == null)
        //                {
        //                    auction = auctionResult;
        //                    auction.Bids = new List<Bid>();
        //                }

        //                if (bid != null)
        //                {
        //                    auction.Bids.Add(bid);
        //                }

        //                return auction;
        //            },
        //            parameters,
        //            splitOn: "UserId",
        //            commandType: CommandType.StoredProcedure
        //        );

        //        // If no bids were returned and the auction is closed, ensure that Bids property is initialized
        //        if (auction.Status == "Closed" && auction.Bids.Count == 0)
        //        {
        //            auction.Bids = new List<Bid>();
        //        }

        //        return result.FirstOrDefault();
        //    }
        //}

        //        }

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

                if (auction == null) // Handle case where no auction is found
                {
                    return null;
                }

                // Check if the auction is closed and no bids were returned
                if (auction.Status == "Closed" && (auction.Bids == null || auction.Bids.Count == 0))
                {
                    auction.Bids = new List<Bid>();
                }

                return result.FirstOrDefault();
            }
        }


    }
}

