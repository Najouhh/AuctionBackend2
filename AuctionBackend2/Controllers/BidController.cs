
using AuktionBackend.Models.DTOS;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Repository.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuktionBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepo _BidRepo;
        private readonly IAuctionRepo  _AuctionRepo;
        private readonly IUserRepo _UserRepo;
        public BidController(IBidRepo repo, IAuctionRepo auctionRepo,IUserRepo userRepo)
        {
            _BidRepo = repo;
            _AuctionRepo = auctionRepo;
            _UserRepo = userRepo;
        }

        [HttpGet("Get All Bids")]
        public IActionResult Get()
        {
            var bid =_BidRepo.GetBids();
            return Ok(bid);

        }
        [HttpGet("GetBidByID")]
        public IActionResult GetBidByID(int BidID)
        {
            var bid = _BidRepo.GetBidByID(BidID);
            return Ok(bid);

        }


        [Authorize]
        [HttpDelete("Delete A bid ")]
        public IActionResult Delete(int BidID)
        {
            var userId = _UserRepo.GetUserId(HttpContext.Request);
            int.TryParse(userId, out int parsedUserId);
            var existingBids = _BidRepo.GetBids();
            var existingAuctions = _AuctionRepo.GetAllAuctions();
            var ownBid = existingBids.FirstOrDefault(b => b.BidId == BidID && b.BidUser.UserId == parsedUserId);
            if (ownBid == null)
            {
                return BadRequest("You cant delete because you dont own this bid");
            }
            var Auction = existingAuctions.FirstOrDefault(a => a.AuctionId == ownBid.Auction.AuctionId);

            if (Auction != null && Auction.EndDate < DateTime.Now)
            {
                return BadRequest("This auction has ended, so you can't delete your bid");
            }
            _BidRepo.Delete(BidID);
            return Ok("Bid has been deleted");
        }

        [Authorize]
        [HttpPost("Create a bid")]
        public IActionResult AddBid(BidDTO bid, int id)
        {
            var userId = _UserRepo.GetUserId(HttpContext.Request);
            int.TryParse(userId, out int parsedUserId);
            var existingBids = _BidRepo.GetBids();
            var auction = _AuctionRepo.GetAuctionByID(id);
            var bidTime = DateTime.Now;

            if (auction == null)
            {
                return BadRequest("Auction is not found");
            }
            if (auction.Auctionuser != null && parsedUserId == auction.Auctionuser.UserId)
            {
                return BadRequest("You cant bide on your own auction");
            }
            
            if (existingBids.Any(b => b.Auction.AuctionId == auction.AuctionId && b.Price > bid.Price))
            {
                return BadRequest("Bid price is lower than the current bid price on this auction");
            }

            if (existingBids.Any(b => b.Auction.AuctionId == auction.AuctionId && b.BidUser.UserId == parsedUserId))
            {
                return BadRequest("You have already bid on this auction");
            }
            if (auction.EndDate < bidTime)
            {
                return BadRequest("you are no longer able to bid due to deadline");
            }
            _BidRepo.InsertBid(auction.AuctionId, parsedUserId, bid, bidTime);

            return Ok("Bid was created successfully");
        }
    }




}
