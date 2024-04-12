using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Delete;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.StockCountSheets;

/// <summary>
/// Delete a stock count sheet.
/// </summary>
/// <remarks>
/// Delete a stock count sheet by providing a valid count sheet id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteStockCountSheetRequest>
{
  private const string EndPointId = "ENP-1W2";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteStockCountSheetRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Stock Count Sheet End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete stock count sheet";
      s.Description = "This endpoint is used to delete stock count sheet with specified (provided) count sheet id(s)";
      s.ExampleRequest = new DeleteStockCountSheetRequest { CountSheetId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteStockCountSheetRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CountSheetId))
    {
      AddError(request => request.CountSheetId, "The count sheet id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), request.CountSheetId ?? "");
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
