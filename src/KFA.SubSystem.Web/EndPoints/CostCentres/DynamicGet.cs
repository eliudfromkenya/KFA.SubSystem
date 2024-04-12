using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;
using KFA.SubSystem.UseCases.Models.List;
using KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;
using KFA.SubSystem.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

/// <summary>
/// List all CostCentres
/// </summary>
/// <remarks>
/// Dynamically Get all Cost Centres as specified - returns a dynamic list of the Cost Centres.
/// </remarks>
public class DynamicGet(IMediator mediator) : Endpoint<ListParam, string>
{
  public override void Configure()
  {
    Get(CoreFunctions.GetURL(DynamicGetCostCentreRequest.Route));
    AllowAnonymous();
    //Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Get Cost Centres Dynamically"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Retrieves dynamically of cost centres as specified";
      s.Description = "Returns all cost centres within specified range";
      // s.ResponseExamples[200] = new CostCentreListResponse { CostCentres = [] };
      s.ExampleRequest = new DynamicGetCostCentreRequest { ListParam = new ListParam { FilterParam = new FilterParam { Predicate = "SupplierCodePrefix.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Description, SupplierCodePrefix}", Parameters = ["S3", "3100"], OrderByConditions = ["Description", "SupplierCodePrefix"] }, Skip = 0, Take = 1000 } };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    //var lstPrm = request?.ListParam.Adapt<ListParam>();
    var command = new DynamicsListModelsQuery<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request ?? new ListParam { });
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      ThrowIfAnyErrors();
    }

    if (result.IsSuccess)
    {
      Response = result.Value;
    }
  }
}
