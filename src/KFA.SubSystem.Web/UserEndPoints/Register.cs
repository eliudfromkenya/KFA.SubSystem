using System.Security.Claims;
using FastEndpoints.Security;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Users;
using MediatR;

namespace KFA.SubSystem.Web.UserEndPoints;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class Register(IMediator mediator, IConfiguration config) : Endpoint<RegisterRequest, RegisterResponse>
{
  private readonly IMediator _mediator = mediator;
  private readonly IConfiguration _config = config;

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(RegisterRequest.Route));
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new RegisterRequest { Username = "Username", Password = "password", NameOfTheUser = "Name", EmailAddress = "email" };
    });
  }

  public override async Task HandleAsync(
    RegisterRequest request,
    CancellationToken cancellationToken)
  {
    var tokenSignature = Functions.GetEnviromentVariable("AuthTokenSigningKey");
    var userDTO = new SystemUserDTO
    {
      Contact = request.Contact,
      EmailAddress = request.EmailAddress,
      ExpirationDate = request.ExpirationDate,
      DateInserted___ = DateTime.UtcNow,
      DateUpdated___ = DateTime.UtcNow,
      Id = null,
      IsActive = request.IsActive,
      MaturityDate = request.MaturityDate,
      NameOfTheUser = request.NameOfTheUser,
      Narration = request.Narration,
      RoleId = null,
      Username = request.Username
    };

    var command = new UserRegisterCommand(userDTO, request?.Device, request?.Password!);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }
    if (result.IsSuccess)
    {
      var (user, loginId, rights) = result.Value;
      var jwtToken = Login.CreateAccessToken(tokenSignature!, rights ?? [], user.RoleId, user.Id, loginId);

      await SendAsync(new RegisterResponse(jwtToken, user.RoleId, user.Id, user.Contact, user.EmailAddress, user.ExpirationDate ?? new DateTime(1, 1, 1), user.IsActive == true, user.MaturityDate ?? new DateTime(1, 1, 1), user.NameOfTheUser, user.Narration, user.Username), cancellation: cancellationToken);
    }
    else await SendErrorsAsync(statusCode: 500, cancellationToken);
  }
}
