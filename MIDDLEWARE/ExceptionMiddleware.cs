using Store.Service.HandleResponse;
using System.Net;
using System;
using System.Text.Json;

namespace STORE.Api.MIDDLEWARE
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<ExceptionMiddleware> _looger;

        public ExceptionMiddleware(RequestDelegate next
            ,IHostEnvironment environment,
            ILogger<ExceptionMiddleware> looger)
        {
            _next = next;
            _environment = environment;
            _looger = looger;
        }




        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Longer.LogError(ex, ...Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment()
                    ? new CustomException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new CustomException((int)HttpStatusCode.InternalServerError);



                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
                // ... (rest of the code)
            }
        }

    }
}
