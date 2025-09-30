using geo_restriction_midleware.Middlewares;

namespace geo_restriction_midleware.Extensions;

public static class GeoRestriction
{
    public static IApplicationBuilder UseGeoProtection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Middlewares.GeoRestriction>();
    }
    
}