namespace MovieDB.Api.Bootstrap;

public static class Routes
{
    public static void Prefix(
        string prefix,
        WebApplication app,
        (string httpMethod, string pattern, RequestDelegate handler)[] definitions)
    {
        foreach (var (httpMethod, pattern, handler) in definitions)
        {
            var path = $"{prefix}{pattern}";
            app.MapMethods(path, new[] { httpMethod }, handler);
        }
    }
}
