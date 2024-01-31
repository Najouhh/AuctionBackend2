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
    public class GetAllAuctions
    {
        [Fact]
        public void GetAllUsers_ReturnsUsers()
        {
            // ARRANGE
            //Arrange
            var AuctionRepo = new Mock<IAuctionRepo>();
            var fakeAuctions = new List<Auction>
    {
        new Auction { AuctionId = 1, Title = "Rolex", Description = "quality", Price = 30000 },
        new Auction { AuctionId = 2, Title = "Gucci bag ", Description = "latest model ", Price = 5000 },

};
            AuctionRepo.Setup(x => x.GetAllAuctions()).Returns(fakeAuctions);

            var userService = new AuctionService(AuctionRepo.Object);

            // Act
            List<Auction> result = userService.GetAllAuctions();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeAuctions.Count, result.Count());
            AuctionRepo.Verify(x => x.GetAllAuctions(), Times.Once);
        }
    }
}

