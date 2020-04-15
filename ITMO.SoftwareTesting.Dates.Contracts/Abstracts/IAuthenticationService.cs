using System.Threading.Tasks;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IAuthenticationService
    {
        Task<string> SignUp(string nickname, string password);
        Task<string> SignIn(string nickname, string password);
        Task DeleteAccount(string password);
    }
}