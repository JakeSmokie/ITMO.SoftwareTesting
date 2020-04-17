namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class GroupInvitation
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public User User { get; set; }
        public Group Group { get; set; }
    }
}