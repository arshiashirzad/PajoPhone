namespace PajoPhone.Middlewares;

public class LoggerMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggerMiddleWare> _logger;
    public LoggerMiddleWare(RequestDelegate next, ILogger<LoggerMiddleWare> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        var routeData = context.GetRouteData();
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        _logger.LogInformation("Incoming Request: {Method} {Path} | Controller: {Controller} | Action: {Action}",
            context.Request.Method, context.Request.Path, controllerName, actionName);
        await _next(context);
        _logger.LogInformation("Outgoing Response: {StatusCode}", context.Response.StatusCode);
    }
}