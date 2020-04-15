using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Exceptions;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using ITMO.SoftwareTesting.Dates.Database.Models;
using ITMO.SoftwareTesting.Dates.Services.Tools;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserContext userContext;
        private readonly IDbContextFactory dbContextFactory;

        public AuthenticationService(
            IUserContext userContext,
            IDbContextFactory dbContextFactory
        )
        {
            this.userContext = userContext;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<string> SignUp(string nickname, string password)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                throw new DatesException("Invalid nickname");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new DatesException("Invalid password");
            }

            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var userExists = await context.Users
                .Where(x => x.Nickname == nickname)
                .AnyAsync();

            if (userExists)
            {
                throw new DatesException("User already exists");
            }

            var user = new User
            {
                Nickname = nickname,
                PasswordHash = Hash(password)
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return JwtTool.IssueToken(user.Id);
        }

        public async Task<string> SignIn(string nickname, string password)
        {
            await using var context = dbContextFactory.Create();

            var user = await context.Users
                .Where(x => x.Nickname == nickname)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new DatesException("No such user was found");
            }

            if (Hash(password) != user.PasswordHash)
            {
                throw new DatesException("Incorrect password");
            }

            return JwtTool.IssueToken(user.Id);
        }

        public async Task DeleteAccount(string password)
        {
            var userId = userContext.UserId;

            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var user = await context.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                // throw new DatesException("No such user was found");
                return;
            }

            if (Hash(password) != user.PasswordHash)
            {
                throw new DatesException("Incorrect password");
            }

            context.Remove(user);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private static string Hash(string input)
        {
            using var md5 = MD5.Create();

            return string.Join(
                "",
                md5.ComputeHash(Encoding.UTF8.GetBytes(input))
                    .Select(x => x.ToString("x2"))
            );
        }
    }
}