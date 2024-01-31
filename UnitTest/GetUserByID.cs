using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionBackend.Services;
using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using Moq;

namespace UnitTest
{
    public class GetUserByID
    {
        [Fact]
        public void Test1_GetUserBy_Id_Method()
        {

            //här fejkas databaset 
            //Arrange
            var user = new User();
            {
                user.UserId = 1;
                user.Username = "test";
                user.Password = "123";
            };
            var userRepo = new Mock<IUserRepo>();
            userRepo.Setup(x => x.GetUserByID(It.IsAny<int>())).Returns(user);

            var logic = new UserService(userRepo.Object);

            //Act

            var Result = logic.GetUserByID;

            //Assert
            Assert.NotNull(Result);
        }
    }
}

