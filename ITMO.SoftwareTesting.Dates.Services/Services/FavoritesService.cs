using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Services.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IUserContext userContext;
        private readonly IDbContextFactory dbContextFactory;

        public FavoritesService(
            IUserContext userContext,
            IDbContextFactory dbContextFactory
        )
        {
            this.userContext = userContext;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task Add(int userId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            context.FavoriteUsers.Add(new FavoriteUser
            {
                FirstUserId = userContext.UserId,
                SecondUserId = userId,
            });

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task Remove(int userId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var favoriteUser = await context.FavoriteUsers
                .Where(x => x.FirstUserId == userContext.UserId)
                .Where(x => x.SecondUserId == userId)
                .FirstOrDefaultAsync();

            if (favoriteUser == null)
            {
                return;
            }

            context.FavoriteUsers.Remove(favoriteUser);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<PersonFavorite>> List()
        {
            await using var context = dbContextFactory.Create();

            var favorites = await context.FavoriteUsers
                .Where(x => x.FirstUserId == userContext.UserId)
                .Select(x => new PersonFavorite
                {
                    Id = x.Second.Id,
                    Nickname = x.Second.Nickname
                })
                .ToListAsync();

            return favorites;
        }
    }
}