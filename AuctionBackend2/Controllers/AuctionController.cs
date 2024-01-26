using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Repository.Repos;
using Groupwork_my_version_.Models.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuktionBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {

        private readonly IAuctionRepo _AuctionRepo;
        private readonly IBidRepo _BidRepo;

        public AuctionController(IAuctionRepo auctionRepo, IBidRepo bidRepo)
        {

            _AuctionRepo = auctionRepo;
            _BidRepo = bidRepo;

        }

        [HttpGet("GetAllAuctions")]
        public IActionResult Get()
        {
            var auctions = _AuctionRepo.GetAllAuctions();
            return Ok(auctions);
        }

        [HttpGet("GetAuctionByID")]
       
        public IActionResult GetAuctionByID(int auctionID)
        {

                var auction = _AuctionRepo.GetAuctionByIddD(auctionID);
                return Ok(auction);
            
        }


        [Authorize]
        [HttpPost("CreateAuction")]
        public IActionResult CreateAuction(AuctionDTO auction)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);
            _AuctionRepo.CreateAuction(auction, userId);
            return Ok("Auktion created successfully");

        }
        [Authorize]
        [HttpDelete("DeleteAuctionByID")]
        public IActionResult DeleteAuctionByID(int auctionID)
        {
            var existingBids = _BidRepo.GetBids();
            var existingAuctions = _AuctionRepo.GetAuctionByID(auctionID);
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int parseduserId))
            {
                return BadRequest("Invalid user ID");
            }

            if (existingBids.Any(b => b.Auction.AuctionId == auctionID))
            {
                return BadRequest("You can't delete an auction when it has bids.");
            }
            else if (existingAuctions.Auctionuser.UserId != parseduserId)
            {

                return BadRequest("You can't delete an auction that you didn't create.");
            }
            else
            {
                _AuctionRepo.DeleteAuctionByID(auctionID);
                return Ok("Auction has been deleted");
            }

        }
        [Authorize]
        [HttpPost("ChangeAuctionByID")]
        public IActionResult ChangeAuctionByID(GetAllAuctionsDTO auction)
        {
            string resultMessage = _AuctionRepo.ChangeAuctionByID(auction);

            if (resultMessage.StartsWith("Auction updated"))
            {
                return Ok(resultMessage);
            }
            else
            {
                return BadRequest(resultMessage);
            }
        }


    }

}