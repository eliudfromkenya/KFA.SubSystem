using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// Get a computer anydesk by anydesk id.
/// </summary>
/// <remarks>
/// Takes anydesk id and returns a matching computer anydesk record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAnydeskByIdRequest, ComputerAnydeskRecord>
{
  private const string EndPointId = "ENP-144";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAnydeskByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Anydesk End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer anydesk by specified anydesk id";
      s.Description = "This endpoint is used to retrieve computer anydesk with the provided anydesk id";
      s.ExampleRequest = new GetComputerAnydeskByIdRequest { AnyDeskId = "anydesk id to retrieve" };
      s.ResponseExamples[200] = new ComputerAnydeskRecord("1000", "AnyDesk Number", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Password", Core.DataLayer.Types.AnyDeskComputerType.Sales, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAnydeskByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AnyDeskId))
    {
      AddError(request => request.AnyDeskId, "The anydesk id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAnydeskDTO, ComputerAnydesk>(CreateEndPointUser.GetEndPointUser(User), request.AnyDeskId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAnydeskRecord(obj.Id, obj.AnyDeskNumber, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.Password, obj.Type, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
