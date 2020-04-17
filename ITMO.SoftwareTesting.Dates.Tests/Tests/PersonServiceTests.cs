using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Tests.Extensions;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class PersonServiceTests
    {
        private static readonly string[] Nicknames =
        {
            "EXPPPOOO",
            "System",
            "Explorer",
            "Somebody",
            "Expiration",
            "The Experienced Dude",
        };

        [Fact]
        public async Task SearchWorks()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            var firstUser = new TestUserContext();
            await firstUser.CreateUser(new AuthenticationService(firstUser, dbContextFactory), Nicknames[0]);

            foreach (var nickname in Nicknames.Skip(1))
            {
                var userContext = new TestUserContext();
                await userContext.CreateUser(new AuthenticationService(userContext, dbContextFactory), nickname);
            }

            var personsService = new PersonsService(firstUser, dbContextFactory);
            var persons = await personsService.SearchPeopleByNickname("ExP");

            Assert.Equal(3, persons.Count);
            Assert.Equal(
                new[] {"Expiration", "Explorer", "The Experienced Dude"},
                persons.Select(x => x.Nickname).OrderBy(x => x).ToArray()
            );
        }
    }
}