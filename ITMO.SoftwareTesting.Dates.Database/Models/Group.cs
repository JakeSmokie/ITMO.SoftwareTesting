using System.Collections.Generic;

namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Purpose { get; set; }
        public int OwnerUserId { get; set; }

        public List<UserAtGroup> Users { get; set; }
        public User OwnerUser { get; set; }
        public List<GroupInvitation> Invitations { get; set; }
        public List<EventAtGroup> Events { get; set; }
    }
}