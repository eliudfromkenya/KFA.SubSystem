using Ardalis.Result;
using System.Security.Claims;
using FastEndpoints.Security;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals.Models;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Org.BouncyCastle.Ocsp;
using System.Security;
using System.Diagnostics.CodeAnalysis;

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

  public override async Task HandleAsync(
    LoginRequest request,
    CancellationToken cancellationToken)
  {
    var tokenSignature = Config.GetValue<string>("Auth:TokenSigningKey");

    var command = new UserLoginCommand(request.Username!, request.Password!, request.Device);
    Result<LoginResult>? result = null;
    try
    {
      result = await _mediator.Send(command, cancellationToken);
    }
    catch (UnauthorizedAccessException ex)
    {
      await ErrorsConverter.CheckErrors(HttpContext, ResultStatus.Unauthorized, [ex.Message], cancellationToken);
      return;
    }
    catch (Exception ex)
    {
      await ErrorsConverter.CheckErrors(HttpContext, ResultStatus.CriticalError, [ex.Message], cancellationToken);
      return;
    }

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      var value = result.Value;
      var jwtToken = CreateAccessToken(tokenSignature!, value.UserRights, value.UserRole, value.UserId, value.LoginId);
      await SendAsync(new LoginResponse(value.LoginId, jwtToken, value.UserId, value.UserRole, DateTime.Now, value.UserRights, value.User as SystemUserDTO), cancellation: cancellationToken);
    }
    else
    {
      AddError("The supplied credentials are invalid!");
    }
  }

  internal static string CreateAccessToken([NotNull] string key, string?[] userRights, string? userRole, string? userId, string? loginId)
  {
    var rights = userRights?
      .Where(c => !string.IsNullOrWhiteSpace(c))
      ?.Select(c => c!)?.ToArray() ?? [];
    return JwtBearer.CreateToken(
                o =>
                {
                  o.SigningKey = key;
                  o.User.Permissions.AddRange(rights);
                  o.ExpireAt = DateTime.UtcNow.AddDays(2);
                  o.User.Roles.Add("RoleId", userRole!);
                  o.User.Claims.Add(("LoginId", loginId!));
                  o.User["UserId"] = userId!; //indexer based claim setting
                });
  }
}
