using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.UseCases.Models.Update;
using KFA.SubSystem.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// Update an existing default access right.
/// </summary>
/// <remarks>
/// Update an existing default access right by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateDefaultAccessRightRequest, UpdateDefaultAccessRightResponse>
{
  private const string EndPointId = "ENP-187";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateDefaultAccessRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Default Access Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Default Access Right";
      s.Description = "This endpoint is used to update  default access right, making a full replacement of default access right with a specifed valuse. A valid default access right is required.";
      s.ExampleRequest = new UpdateDefaultAccessRightRequest { Name = "Name", Narration = "Narration", RightID = "1000", Rights = "Rights", Type = "Type" };
      s.ResponseExamples[200] = new UpdateDefaultAccessRightResponse(new DefaultAccessRightRecord("Name", "Narration", "1000", "Rights", "Type", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateDefaultAccessRightRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RightID))
    {
      AddError(request => request.RightID, "The right id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<DefaultAccessRightDTO, DefaultAccessRight>(CreateEndPointUser.GetEndPointUser(User), request.RightID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the default access right to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<DefaultAccessRightDTO, DefaultAccessRight>(CreateEndPointUser.GetEndPointUser(User), request.RightID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateDefaultAccessRightResponse(new DefaultAccessRightRecord(obj.Name, obj.Narration, obj.Id, obj.Rights, obj.Type, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
