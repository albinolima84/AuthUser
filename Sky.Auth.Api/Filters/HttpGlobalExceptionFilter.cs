using Microsoft.AspNetCore.Mvc.Filters;
using Sky.Auth.CrossCutting.Exceptions;
using System.Net;

namespace Sky.Auth.Api.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public HttpGlobalExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ForbiddenException ex:
                    ManageException(context, HttpStatusCode.Forbidden);
                    break;
                default:
                    ManageException(context, HttpStatusCode.InternalServerError);
                    break;
            }
        }

        private void ManageException(ExceptionContext context, HttpStatusCode httpStatusCode)
        {
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;
            context.ExceptionHandled = true;
        }
    }
}
