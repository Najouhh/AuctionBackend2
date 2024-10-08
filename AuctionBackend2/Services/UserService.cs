﻿using AuktionBackend.Models.DTOS;
using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Repository.Repos;

namespace AuctionBackend.Services
{
    public class UserService : IUserRepo
    {
        private readonly IUserRepo _userRepo;

        // här injectas repot till controllen 
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public void CreateUser(UserPostDTO user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserID(int Userid)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserByID(int Userid)
        {
            var user = _userRepo.GetUserByID(Userid);
            //var user = UserRepo.GetUserByID(UserID);

            return (user);
        }

        public string GetUserId(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public UserPostDTO Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserByID(User user)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string username, string password)
        {
            return username == "user" && password == "pass";
        }


        public bool log(string username, string password)
        {
            return _userRepo.Authenticate(username, password);
        }
    }
}
