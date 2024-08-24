using EchoPage.Models;
using EchoPage.Security;

namespace EchoPage.Middleware
{
    public class GetId : IMiddleware
    {
        private readonly ILogger<GetId> logger;
        private readonly UserCreds creds;
        public GetId(ILogger<GetId> logger, UserCreds creds)
        {
            this.logger = logger;
            this.creds = creds;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context.Request.Path.StartsWithSegments("/api/User") && context.Request.Method.ToUpperInvariant() == "POST"
                || context.Request.Path.StartsWithSegments("/api/User/login") && context.Request.Method.ToUpperInvariant() == "POST")
            {
                await next(context);
                return;
            }

            var bearerToken = context.Request.Headers.Authorization.ToString();
            var token = bearerToken.Split(' ')[1];
            creds.userid = TokenDecoder.decodeToken(token);
            await next(context);
            return;
        }
    }
}
