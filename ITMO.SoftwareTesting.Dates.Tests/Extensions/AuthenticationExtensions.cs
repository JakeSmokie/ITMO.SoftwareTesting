using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Tests.Utils;

namespace ITMO.SoftwareTesting.Dates.Tests.Extensions
{
    public static class AuthenticationExtensions
    {
        public static async Task CreateUser(
            this TestUserContext userContext,
            IAuthenticationService authenticationService,
            string nickname = "RandomFellow",
            string password = "AnotherOneBitesADust"
        )
        {
            var token = await authenticationService.SignUp(nickname, password);
            userContext.UseToken(token);
        }
    }
}