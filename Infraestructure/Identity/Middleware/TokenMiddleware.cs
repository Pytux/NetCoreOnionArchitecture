using System.Net;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Middleware;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var requiresAuthorization =
            context.GetEndpoint()?.Metadata?.GetMetadata<IAuthorizeData>() != null;

        using (var scope = context.RequestServices.CreateScope())
        {
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

            if (!requiresAuthorization || await tokenService.IsCurrentActiveToken())
            {
                await _next(context);

                return;
            }
        }


        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}