namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class UserAtGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
    }
}