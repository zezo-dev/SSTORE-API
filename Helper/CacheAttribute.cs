using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheService;
using System.Text;

namespace STORE.Api.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheServisce = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cahcekey = GenerateCachedkeyFromResponse(context.HttpContext.Request);
            var cachedresponse = await CacheServisce.GetCacheResponseAsync(cahcekey);

            if (!string.IsNullOrEmpty(cachedresponse))
            {
                var contentresult = new ContentResult
                {
                    Content = cachedresponse,
                    ContentType = "application/json",
                    StatusCode = 200,

                };
                context.Result = contentresult;
             
                return;

            }

            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult response)
            {
                await CacheServisce.SetCacheResponseAsync(cahcekey, response.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }


        private string GenerateCachedkeyFromResponse(HttpRequest request)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                cacheKey.Append($"|{key}-{value}");

            return cacheKey.ToString();
        }
    }
}
