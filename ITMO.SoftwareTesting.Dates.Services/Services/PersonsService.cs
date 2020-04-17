using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Services.Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IUserContext userContext;
        private readonly IDbContextFactory dbContextFactory;

        public PersonsService(
            IUserContext userContext,
            IDbContextFactory dbContextFactory
        )
        {
            this.userContext = userContext;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<List<PersonListItem>> SearchPeopleByNickname(string nicknameMask)
        {
            await using var context = dbContextFactory.Create();

            var users = await context.Users
                .Where(x => x.Id != userContext.UserId)
                .Where(x => x.Nickname.ToLower().Contains(nicknameMask.ToLower()))
                .Select(x => new PersonListItem
                {
                    Id = x.Id,
                    Nickname = x.Nickname
                })
                .ToListAsync();

            return users;
        }
    }
}