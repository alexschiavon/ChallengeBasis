using BookDomain.Helper.Exceptions;
using Finance.Api.Model.Error;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace BookApi.Middleware
{
    public class ErrorMiddleware
    {
        private readonly string error500;
        private readonly RequestDelegate next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
            error500 = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ErrorResponse errorResponse;
            string exName = ex is ValidationException ? "ValidationException" : HttpStatusCode.InternalServerError.ToString();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ||
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Qa")
            {
                errorResponse = new ErrorResponse(exName,
                                                      $"{ex.Message} {ex?.InnerException?.Message}", error500);
            }
            else
            {
                errorResponse = new ErrorResponse(exName,
                                                      "An internal server error has occurred.", error500);
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var result = JsonConvert.SerializeObject(errorResponse, settings);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
