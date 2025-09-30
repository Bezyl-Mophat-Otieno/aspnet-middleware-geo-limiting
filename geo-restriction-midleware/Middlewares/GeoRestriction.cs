using System.Net;

namespace geo_restriction_midleware.Middlewares;

public class GeoRestriction(ILogger<GeoRestriction> logger, RequestDelegate next)
{


    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress;
        if (ipAddress is null)
        {
            await context.Response.WriteAsync(
                "Sorry we could not  identify your geographical location, try again later");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        logger.LogInformation("IpAddress {ipAddress}", ipAddress);
        await next(context);

    }

}