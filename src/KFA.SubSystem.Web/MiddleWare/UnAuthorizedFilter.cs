namespace KFA.SubSystem.Web.MiddleWare;

sealed class UnAuthorizedFilter : IEndpointFilter
{
  private readonly ILogger<UnAuthorizedFilter> logger;

  public UnAuthorizedFilter(ILogger<UnAuthorizedFilter> logger)
  {
    this.logger = logger;
  }

  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
    try
    {
      return await next(context);
    }
    catch (Exception x)
    {
      logger.LogDebug(x, "An Error Happenned ");
        throw;
     // return Results.StatusCode(499);
    }
  }
}
