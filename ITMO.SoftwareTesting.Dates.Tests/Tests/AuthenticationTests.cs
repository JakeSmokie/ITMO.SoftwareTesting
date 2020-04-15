using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Exceptions;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Services.Tools;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class AuthenticationTests
    {
        private readonly IAuthenticationService authenticationService;
        private readonly TestUserContext userContext;

        public AuthenticationTests()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            userContext = new TestUserContext();
            authenticationService = new AuthenticationService(userContext, dbContextFactory);
        }

        [Theory]
        [InlineData("Vasya", "Pupkeen_1")]
        [InlineData("Jules", "1234567890")]
        public async Task SignsUpOkOnValidData(string nickname, string password)
        {
            var token = await authenticationService.SignUp(nickname, password);

            Assert.NotNull(token);
            Assert.NotNull(JwtTool.DecodeToken(token));
        }

        [Theory]
        [InlineData("Vasya", null)]
        [InlineData("Jules", "123")]
        public async Task SignUpFailsOnInvalidPassword(string nickname, string password)
        {
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.SignUp(nickname, password);
            });

            Assert.Equal("Invalid password", exception.Message);
        }

        [Theory]
        [InlineData(null, "1231231231")]
        [InlineData("", "1231231231")]
        [InlineData("        ", "1231231231")]
        public async Task SignUpFailsOnInvalidNickname(string nickname, string password)
        {
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.SignUp(nickname, password);
            });

            Assert.Equal("Invalid nickname", exception.Message);
        }

        [Fact]
        public async Task SignUpFailsOnExistingUser()
        {
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.SignUp("Dude", "1234567890");
                await authenticationService.SignUp("Dude", "0987654321");
            });

            Assert.Equal("User already exists", exception.Message);
        }

        [Theory]
        [InlineData("Vasya", "OkDude123")]
        [InlineData("TestUser", "923472389423")]
        public async Task SignsInOkOnValidData(string nickname, string password)
        {
            var token1 = await authenticationService.SignUp(nickname, password);
            var token2 = await authenticationService.SignIn(nickname, password);

            Assert.NotNull(token1);
            Assert.NotNull(token2);
            Assert.NotNull(JwtTool.DecodeToken(token1));
            Assert.NotNull(JwtTool.DecodeToken(token2));
        }

        [Theory]
        [InlineData("Vasya", "OkDude123")]
        [InlineData("TestUser", "923472389423")]
        public async Task SignInFailsOnUnexistentUser(string nickname, string password)
        {
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.SignIn(nickname, password);
            });

            Assert.Equal("No such user was found", exception.Message);
        }

        [Theory]
        [InlineData("Vasya", "OkDude123", "123123123")]
        [InlineData("TestUser", "923472389423", "23948237948234")]
        public async Task SignInFailsOnBadPassword(string nickname, string password, string badPassword)
        {
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.SignUp(nickname, password);
                await authenticationService.SignIn(nickname, badPassword);
            });

            Assert.Equal("Incorrect password", exception.Message);
        }

        [Theory]
        [InlineData("Vasya", "OkDude123")]
        public async Task AccountDeletionWorks(string nickname, string password)
        {
            userContext.UseToken(await authenticationService.SignUp(nickname, password));

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.DeleteAccount(password);
                await authenticationService.SignIn(nickname, password);
            });

            Assert.Equal("No such user was found", exception.Message);
        }

        [Theory]
        [InlineData("Vasya", "OkDude123", "123123123")]
        public async Task AccountDeletionFailsOnBadPassword(string nickname, string password, string badPassword)
        {
            userContext.UseToken(await authenticationService.SignUp(nickname, password));

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await authenticationService.DeleteAccount(badPassword);
            });

            Assert.Equal("Incorrect password", exception.Message);
        }
    }
}