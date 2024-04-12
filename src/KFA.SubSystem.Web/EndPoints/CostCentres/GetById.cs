using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

/// <summary>
/// Get a CostCentre by integer ID.
/// </summary>
/// <remarks>
/// Takes a positive integer ID and returns a matching CostCentre record.
/// </remarks>
public class GetById(IMediator mediator) : Endpoint<GetCostCentreByIdRequest, CostCentreRecord>
{
  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetCostCentreByIdRequest.Route));
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Get Cost Centre"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Gets a cost centre by specified id";
      s.Description = "Used to retrieved saved cost centre with the provided id";
      s.ExampleRequest = new GetCostCentreByIdRequest { CostCentreCode = "id to retrieve" };
      s.ResponseExamples[200] = new CostCentreRecord("Id", "Description", "narration", "Region", "supplier prefix", true,DateTime.UtcNow, DateTime.UtcNow);
    });
  }

  public override async Task HandleAsync(GetCostCentreByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode ?? "CostCentreCode", "Cost Centre Code of item to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      ThrowIfAnyErrors();
    }

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var value = result.Value;
    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix,true, value?.DateInserted___, value?.DateUpdated___);
    }
  }
}
