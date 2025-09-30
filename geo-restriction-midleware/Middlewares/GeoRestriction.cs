using System.Net;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;

namespace geo_restriction_midleware.Middlewares;

public class GeoRestriction
{
    private readonly ILogger<GeoRestriction> _logger;
    private readonly RequestDelegate _next;
    private readonly DatabaseReader _dbReader;
    private readonly HashSet<string> _blockedCountries;

    public GeoRestriction(ILogger<GeoRestriction> logger, RequestDelegate next, IConfiguration config)
    {
        _logger = logger;
        _next = next;
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "Geo", "GeoLite2-Country.mmdb");
        _dbReader = new DatabaseReader(path);
        _blockedCountries = config.GetSection("GeoRestriction:BlockedCountries")
            .Get<string[]>()?
            .ToHashSet() ?? new HashSet<string>() ;

    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            if (ipAddress is null || IPAddress.IsLoopback(ipAddress))
            {            
                _logger.LogInformation("Skipping geo check for local request {IP}", ipAddress);
                await _next(context);
                return;
            }

            var country = _dbReader.Country(ipAddress);
            var countryCode = country.Country.IsoCode;
            if (string.IsNullOrEmpty(countryCode) || _blockedCountries.Contains(countryCode))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = "We are currently not available in your country"});
                return;
            }
            _logger.LogInformation("My Country {country}", country.Country.Name);
            

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogWarning("Address not found");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "Unable to determine your location." });
            return;
        }
        
        await _next(context);
        
    }

}