using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.Verifications;

/// <summary>
/// Get a verification by verification id.
/// </summary>
/// <remarks>
/// Takes verification id and returns a matching verification record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetVerificationByIdRequest, VerificationRecord>
{
  private const string EndPointId = "ENP-2A4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetVerificationByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Verification End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets verification by specified verification id";
      s.Description = "This endpoint is used to retrieve verification with the provided verification id";
      s.ExampleRequest = new GetVerificationByIdRequest { VerificationId = "verification id to retrieve" };
      s.ResponseExamples[200] = new VerificationRecord(DateTime.Now, string.Empty, "Narration", 0, "Table Name", "1000", "Verification Name", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetVerificationByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationId))
    {
      AddError(request => request.VerificationId, "The verification id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<VerificationDTO, Verification>(CreateEndPointUser.GetEndPointUser(User), request.VerificationId ?? "");
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
      Response = new VerificationRecord(obj.DateOfVerification, obj.LoginId, obj.Narration, obj.RecordId, obj.TableName, obj.Id, obj.VerificationName, obj.VerificationRecordId, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
