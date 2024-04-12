using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Delete;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.DataDevices;

/// <summary>
/// Delete a data device.
/// </summary>
/// <remarks>
/// Delete a data device by providing a valid device id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteDataDeviceRequest>
{
  private const string EndPointId = "ENP-172";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteDataDeviceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Data Device End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete data device";
      s.Description = "This endpoint is used to delete data device with specified (provided) device id(s)";
      s.ExampleRequest = new DeleteDataDeviceRequest { DeviceId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteDataDeviceRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DeviceId))
    {
      AddError(request => request.DeviceId, "The device id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<DataDevice>(CreateEndPointUser.GetEndPointUser(User), request.DeviceId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

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
