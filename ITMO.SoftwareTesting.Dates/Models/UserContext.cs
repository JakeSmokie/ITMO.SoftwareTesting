using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;

namespace ITMO.SoftwareTesting.Datings.Models
{
    public class UserContext : IUserContext
    {
        public int UserId { get; }

        public UserContext(int userId)
        {
            UserId = userId;
        }
    }
}