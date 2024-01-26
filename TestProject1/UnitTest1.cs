using AuktionBackend.Controllers;
using AuktionBackend.Entities;
using AuktionBackend.Repository;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.ServerLayer;
using Moq;


namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1_GetUserBy_Id_Method()
        {

            //här fajkas databaset 
            //Arrange
            var user = new User();
            {
                //vdvdv d
                user.UserId = 1;
                user.Username = "test";
                user.Password = "123";
            };
            var userRepo = new Mock<IUserRepo>();
            userRepo.Setup(x => x.GetUserByID(It.IsAny<int>())).Returns(user);
          //  var controller = new UserController(userRepo.Object);
          var logic = new LogicLayer (userRepo.Object);

            //Act
           // var getUSerByID = controller.GetUserByID(1);
           var getUserByID = logic.GetUserByID;

            //Assert
            Assert.NotNull(getUserByID);
            
        }
    }
}