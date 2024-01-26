using System.Security.Claims;
using AuktionBackend.Models.DTOS;
using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Repository.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuktionBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _UserRepo;
        public UserController(IUserRepo UserRepo)
        {
            _UserRepo= UserRepo ;
        }
        [HttpGet("GeAllUsers")]
        public IActionResult Get()
        {
           var auctions = _UserRepo.GetAllUsers();
           return Ok(auctions);
        }
        [HttpGet("GetUserByID")]
        public IActionResult GetUserByID(int UserID)
        {
            var user = _UserRepo.GetUserByID(UserID);
            return Ok(user);
        }

        [HttpPost("CreateNewUser")]
        public IActionResult CreateUser(UserPostDTO User)
        {
            _UserRepo.CreateUser(User);
            return Ok("User has been added");
        }
        [Authorize]
        [HttpPatch("UpdateUserByID")]
        public IActionResult UpdateUserByID(User user)
        {
            _UserRepo.UpdateUserByID(user);
            return Ok("User has been updated");
        }
        [Authorize]
        [HttpDelete("DeleteUserByID")]
        public IActionResult DeleteAuctionByID(int Userid)
        {
            _UserRepo.DeleteUserID(Userid);
            return Ok("User has been Deleted");
        }
       
}
}
