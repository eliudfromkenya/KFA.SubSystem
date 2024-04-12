using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastEndpoints.Security;
using KFA.SubSystem.Core;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace KFA.SubSystem.Web.UserEndPoints;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class Login : Endpoint<LoginRequest, LoginResponse>
{
  private readonly IMediator _mediator;

  //private readonly Microsoft.Extensions.Configuration.ConfigurationManager _manager;

  // private readonly WebApplicationBuilder _builder;

  public Login(IMediator mediator)
  {
    _mediator = mediator;
    //_manager = manager;
    // _builder = builder;
  }

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(LoginRequest.Route));
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new LoginRequest { Username = "Username", Password = "password" };
    });
  }

  internal static string GetToken(IConfiguration config, string? userId, string? userRole, string? loginId, string?[] userRights)
  {
    var tokenSignature = config?.GetValue<string>("Auth:TokenSigningKey")!;
    var tokenHandler = new JwtSecurityTokenHandler();

    var tokenKey = Encoding.ASCII.GetBytes(tokenSignature);
    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
    {
      Expires = DateTime.UtcNow.AddDays(1),
      EncryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(tokenKey), "Sha512"),
      Claims = new Dictionary<string, object>
          {
            { "UserId", userId! } ,
            { "LoginId", loginId! },
            { "RoleId", userRole! },
            { "Permissions", userRights! }
          }
    });
    return token?.ToString() ?? string.Empty;
  }

  public override async Task HandleAsync(
    LoginRequest request,
    CancellationToken cancellationToken)
  {
    var tokenSignature = Config.GetValue<string>("Auth:TokenSigningKey")!;

    var command = new UserLoginCommand(request.Username!, request.Password!, request.Device);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      var value = result.Value;
      var token = GetToken(Config, value.UserId, value.UserRole, value.LoginId, value.UserRights);
      await SendAsync(new LoginResponse(value.LoginId, token, value.UserId, value.UserRole, DateTime.Now, value.UserRights), cancellation: cancellationToken);
    }
    else
    {
      AddError("The supplied credentials are invalid!");
    }
  }
}
