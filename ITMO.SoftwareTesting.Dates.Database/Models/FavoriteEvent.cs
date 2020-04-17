namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class FavoriteEvent
    {
        public int UserId { get; set; }
        public int EventId { get; set; }

        public User User { get; set; }
    }
}