using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Delete;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.Verifications;

/// <summary>
/// Delete a verification.
/// </summary>
/// <remarks>
/// Delete a verification by providing a valid verification id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteVerificationRequest>
{
  private const string EndPointId = "ENP-2A2";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteVerificationRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Verification End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete verification";
      s.Description = "This endpoint is used to delete verification with specified (provided) verification id(s)";
      s.ExampleRequest = new DeleteVerificationRequest { VerificationId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteVerificationRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationId))
    {
      AddError(request => request.VerificationId, "The verification id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<Verification>(CreateEndPointUser.GetEndPointUser(User), request.VerificationId ?? "");
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
