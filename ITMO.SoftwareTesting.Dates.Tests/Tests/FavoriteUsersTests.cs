using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Tests.Extensions;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class FavoriteUsersTests
    {
        private readonly TestUserContext firstContext;
        private readonly IFavoriteUsersService firstFavoriteUsers;
        private readonly IAuthenticationService firstAuth;

        private readonly TestUserContext secondContext;
        private readonly IFavoriteUsersService secondFavoriteUsers;
        private readonly IAuthenticationService secondAuth;


        public FavoriteUsersTests()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            firstContext = new TestUserContext();
            firstFavoriteUsers = new FavoriteUsersService(firstContext, dbContextFactory);
            firstAuth = new AuthenticationService(firstContext, dbContextFactory);

            secondContext = new TestUserContext();
            secondFavoriteUsers = new FavoriteUsersService(secondContext, dbContextFactory);
            secondAuth = new AuthenticationService(secondContext, dbContextFactory);
        }

        [Fact]
        public async Task FavoritesWork()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var firstState = await firstFavoriteUsers.List();
            await firstFavoriteUsers.Add(secondContext.UserId);

            var secondState = await firstFavoriteUsers.List();
            await firstFavoriteUsers.Remove(secondContext.UserId);

            var thirdState = await firstFavoriteUsers.List();
            
            Assert.Empty(firstState);
            Assert.Single(secondState);
            Assert.Contains(secondContext.UserId, secondState.Select(x => x.Id));
            Assert.Empty(thirdState);
        }
        
        [Fact]
        public async Task FavoritesDeletionIsSuppressedOnInvalidInput()
        {
            await firstContext.CreateUser(firstAuth);
            await firstFavoriteUsers.Remove(200);
        }
    }
}