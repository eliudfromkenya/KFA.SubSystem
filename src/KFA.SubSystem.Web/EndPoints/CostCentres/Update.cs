using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.UseCases.Models.Update;
using KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;
using KFA.SubSystem.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

/// <summary>
/// Update an existing CostCentre.
/// </summary>
/// <remarks>
/// Update an existing CostCentre by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator) : Endpoint<UpdateCostCentreRequest, UpdateCostCentreResponse>
{
  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateCostCentreRequest.Route));
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Update Cost Centre"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Update a Cost Centre";
      s.Description = "Create a new CostCentre. A valid name is required. hgklgjk";
      s.ExampleRequest = new CreateCostCentreRequest { Description = "CostCentre Name" };
      s.ResponseExamples[200] = new CreateCostCentreResponse { };
    });
  }

  public override async Task HandleAsync(
    UpdateCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode ?? "Id", "Id of item to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
      ThrowIfAnyErrors();
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the cost centre to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new UpdateCostCentreResponse(new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.IsActive, value?.DateInserted___, value?.DateUpdated___));
      return;
    }
  }
}
