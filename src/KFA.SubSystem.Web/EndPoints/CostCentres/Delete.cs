﻿using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Delete;
using KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

/// <summary>
/// Delete a CostCentre.
/// </summary>
/// <remarks>
/// Delete a CostCentre by providing a valid integer id.
/// </remarks>
public class Delete(IMediator mediator) : Endpoint<DeleteCostCentreRequest>
{
  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteCostCentreRequest.Route));
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Delete Cost Centre"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Delete a Cost Centre";
      s.Description = "Used to delete cost centre with specified id(s)";
      s.ExampleRequest = new DeleteCostCentreRequest { CostCentreCode = "AAA-01" };
      s.ResponseExamples = new Dictionary<int, object> { { 200, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode ?? "Id", "Item to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      ThrowIfAnyErrors();
    }

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    };
  }
}
