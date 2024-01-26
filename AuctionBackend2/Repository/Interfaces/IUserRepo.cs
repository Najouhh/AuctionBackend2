using AuktionBackend.Models.DTOS;
using AuktionBackend.Models.Entities;

namespace AuktionBackend.Repository.Interfaces
{
    public interface IUserRepo
    {
        public List<User> GetAllUsers();
        User GetUserByID(int Userid);
        void CreateUser(UserPostDTO user);
        void UpdateUserByID(User user);
        void DeleteUserID(int Userid);
        UserPostDTO Login(string username, string password);
        string GetUserId(HttpRequest request);

    }
}
