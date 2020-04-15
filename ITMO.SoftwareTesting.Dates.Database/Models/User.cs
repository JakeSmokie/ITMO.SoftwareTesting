namespace ITMO.SoftwareTesting.Dates.Database.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; }
        public string PasswordHash { get; set; }
    }
}