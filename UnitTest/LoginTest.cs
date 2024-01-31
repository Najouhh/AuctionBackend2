using AuctionBackend.Services;
using AuktionBackend.Repository.Interfaces;
using Moq;

namespace UnitTest
{
    public class LoginTest
    {
        [Fact]
       

            public void Login_Successful()
            {
                var mockDbServices = new Mock<IUserRepo>();
                mockDbServices.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
                var loginService = new UserService(mockDbServices.Object);

                bool result = loginService.log("user", "pass");

                Assert.True(result);
                mockDbServices.Verify(x => x.Authenticate("user", "pass"), Times.Once);

            }
        
    }
}