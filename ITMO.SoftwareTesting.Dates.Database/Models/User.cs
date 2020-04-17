using System.Collections.Generic;

namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; }
        public string PasswordHash { get; set; }

        public List<UserAtGroup> Groups { get; set; }
        public List<GroupInvitation> Invitations { get; set; }
        public List<FavoriteUser> Favorites { get; set; }
    }
}