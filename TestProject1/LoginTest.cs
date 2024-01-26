using AuktionBackend.Entities;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.ServerLayer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
   public class LoginTest
    {
        [Fact]
        public void Login_Successful()
        {        
          

            var mockDbServices = new Mock<IUserRepo>();
            mockDbServices.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var loginService = new UserService(mockDbServices.Object);

            bool result = loginService.Login("user", "pass");

            Assert.True(result);
            mockDbServices.Verify(x => x.Authenticate("user", "pass"), Times.Once);

        }
    }
}
