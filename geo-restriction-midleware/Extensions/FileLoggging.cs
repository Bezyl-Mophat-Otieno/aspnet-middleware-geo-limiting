namespace geo_restriction_midleware.Extensions;

public  static class FileLoggging
{

    public static  void ConfigureFileLogger(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
        loggingBuilder.AddFile("Logs/{Date}.txt");

    }
    
}