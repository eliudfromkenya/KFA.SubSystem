﻿using KFA.SubSystem.Core;
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
public class ClearRights(IMediator mediator) : Endpoint<ClearRightsRequest, string[]>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(ClearRightsRequest.Route));
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new ClearRightsRequest { UserId = "Username or user id", UserRightIds = ["user right id"] };
    });
  }

  public override async Task HandleAsync(
    ClearRightsRequest request,
    CancellationToken cancellationToken)
  {
    var command = new UserClearRightsCommand(request.UserId!, request.UserRightIds!, request.Device);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }
    if (result.IsSuccess)
    {
      await SendAsync(result.Value, cancellation: cancellationToken);
    }
    else await SendErrorsAsync(statusCode: 500, cancellationToken);
  }
}
