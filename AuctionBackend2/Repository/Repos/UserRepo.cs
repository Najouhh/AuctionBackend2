using AuktionBackend.Models.DTOS;
using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuktionBackend.Repository.Repos
{
    public class UserRepo:IUserRepo
    {
        private readonly IJensenContext _context;
        public UserRepo(IJensenContext context)
        {
            _context = context;
        }


        public  List<User> GetAllUsers()
        {
            using (IDbConnection db = _context.GetConnection())
            {
                return db.Query<User>("ShowAlLUsers", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public  User GetUserByID(int Userid)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var parameters = new { Userid };
                return db.Query<User>("GetUserById", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public  void CreateUser(UserPostDTO user)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                db.Open();

                var parameters = new
                {
                    user.Username,
                    user.Password


                };

                db.Execute("InsertUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public  void UpdateUserByID(User user)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                db.Open();

                var parameters = new
                {
                    userid = user.UserId,
                    user.Username,
                    user.Password
                };

                db.Execute("UpdateUSER", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public  void DeleteUserID(int Userid)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                db.Open();

                var parameters = new { Userid };

                db.Execute("DeleteUserByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }
       
        public UserPostDTO Login(string username, string password)
        {
            using (IDbConnection db = _context.GetConnection())
            {
                var parameters = new
                {
                    UserName = username,
                    Password = password
                };

                return db.QueryFirstOrDefault<UserPostDTO>("UserLogin", parameters, commandType: CommandType.StoredProcedure);
            }

        }
        public string GetUserId(HttpRequest request)
        {
            var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim;
        }
    }
}
