using System.Net;
using System.Security;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ITMO.SoftwareTesting.Datings.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.ExceptionHandled)
            {
                return;
            }

            switch (exceptionContext.Exception)
            {
                case DatesException e:
                    SetProblemDetails(exceptionContext, new ProblemDetails
                    {
                        Instance = exceptionContext.HttpContext.Request.Path,
                        Status = (int) HttpStatusCode.BadRequest,
                        Type = "hiyou/security-fault",
                        Detail = e.Message,
                        Title = "Bad Request"
                    });

                    return;
                default:
                {
                    exceptionContext.ExceptionHandled = false;
                    break;
                }
            }
        }

        private static void SetProblemDetails(ExceptionContext exceptionContext, ProblemDetails problemDetails)
        {
            var problemDetailsResult = new JsonResult(problemDetails) {StatusCode = problemDetails.Status};
            exceptionContext.Result = problemDetailsResult;
        }
    }
}