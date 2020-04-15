using System.Linq;
using System.Security.Claims;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Services.Tools;

namespace ITMO.SoftwareTesting.Dates.Tests.Utils
{
    public class TestUserContext : IUserContext
    {
        public int UserId { get; private set; }

        public void UseToken(string token)
        {
            var jwt = JwtTool.DecodeToken(token);

            var userId = jwt.Claims
                .Where(x => x.Type == ClaimsIdentity.DefaultNameClaimType)
                .Select(x => int.Parse(x.Value))
                .First();

            UserId = userId;
        }
    }
}