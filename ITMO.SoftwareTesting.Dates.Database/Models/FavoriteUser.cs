namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class FavoriteUser
    {
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }

        public User First { get; set; }
        public User Second { get; set; }
    }
}