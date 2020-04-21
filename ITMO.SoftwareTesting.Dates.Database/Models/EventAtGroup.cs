namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class EventAtGroup
    {
        public int GroupId { get; set; }
        public int EventId { get; set; }

        public Group Group { get; set; }
    }
}