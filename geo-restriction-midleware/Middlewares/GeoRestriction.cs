using System.Net;
using MaxMind.GeoIP2;

namespace geo_restriction_midleware.Middlewares;

public class GeoRestriction()
{
    private readonly ILogger<GeoRestriction> _logger;
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly DatabaseReader _dbReader;
    private readonly HashSet<string> _blockedCountries;

    public GeoRestriction(ILogger<GeoRestriction> logger, RequestDelegate next, IConfiguration config)
    {
        _logger = logger;
        _next = next;
        _config = config;
        _dbReader = new DatabaseReader("/Resources/Geo/GeoLite2-City.mmdb");
        _blockedCountries = config.GetSection("GeoRestriction:BlockedCountries")
            .Get<string[]>()?
            .ToHashSet() ?? new HashSet<string>() ;

    }


    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress;
        if (ipAddress is null)
        {
            await context.Response.WriteAsync(
                "Sorry we could not  identify your geographical location, try again later");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        _logger.LogInformation("IpAddress {ipAddress}", ipAddress);
        await _next(context);

    }

}