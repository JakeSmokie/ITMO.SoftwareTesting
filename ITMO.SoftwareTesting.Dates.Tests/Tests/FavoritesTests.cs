using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Tests.Extensions;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class FavoritesTests
    {
        private readonly TestUserContext firstContext;
        private readonly IFavoritesService firstFavorites;
        private readonly IAuthenticationService firstAuth;

        private readonly TestUserContext secondContext;
        private readonly IFavoritesService secondFavorites;
        private readonly IAuthenticationService secondAuth;


        public FavoritesTests()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            firstContext = new TestUserContext();
            firstFavorites = new FavoritesService(firstContext, dbContextFactory);
            firstAuth = new AuthenticationService(firstContext, dbContextFactory);

            secondContext = new TestUserContext();
            secondFavorites = new FavoritesService(secondContext, dbContextFactory);
            secondAuth = new AuthenticationService(secondContext, dbContextFactory);
        }

        [Fact]
        public async Task FavoritesWork()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var firstState = await firstFavorites.List();
            await firstFavorites.Add(secondContext.UserId);

            var secondState = await firstFavorites.List();
            await firstFavorites.Remove(secondContext.UserId);

            var thirdState = await firstFavorites.List();
            
            Assert.Empty(firstState);
            Assert.Single(secondState);
            Assert.Contains(secondContext.UserId, secondState.Select(x => x.Id));
            Assert.Empty(thirdState);
        }
        
        [Fact]
        public async Task FavoritesDeletionIsSuppressedOnInvalidInput()
        {
            await firstContext.CreateUser(firstAuth);
            await firstFavorites.Remove(200);
        }
    }
}