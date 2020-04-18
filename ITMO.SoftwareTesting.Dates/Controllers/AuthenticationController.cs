using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Datings.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITMO.SoftwareTesting.Datings.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("sign-up")]
        public Task<string> SignUp([FromBody] AuthenticationRequest request)
        {
            return authenticationService.SignUp(request.Nickname, request.Password);
        }

        [HttpPost]
        [Route("sign-in")]
        public Task<string> SignIn([FromBody] AuthenticationRequest request)
        {
            return authenticationService.SignIn(request.Nickname, request.Password);
        }

        [HttpPost]
        [Route("delete-account")]
        [Authorize]
        public Task DeleteAccount([FromBody] AuthenticationRequest request)
        {
            return authenticationService.DeleteAccount(request.Password);
        }
    }
}