using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Get;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.Suppliers;

/// <summary>
/// Get a supplier by supplier id.
/// </summary>
/// <remarks>
/// Takes supplier id and returns a matching supplier record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetSupplierByIdRequest, SupplierRecord>
{
  private const string EndPointId = "ENP-1Z4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetSupplierByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Supplier End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets supplier by specified supplier id";
      s.Description = "This endpoint is used to retrieve supplier with the provided supplier id";
      s.ExampleRequest = new GetSupplierByIdRequest { SupplierId = "supplier id to retrieve" };
      s.ResponseExamples[200] = new SupplierRecord("Address", "Contact Person", "Cost Centre Code", "Description", "Email", "Narration", "Postal Code", "Supplier Code", "1000", "Telephone", "Town", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetSupplierByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SupplierId))
    {
      AddError(request => request.SupplierId, "The supplier id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<SupplierDTO, Supplier>(CreateEndPointUser.GetEndPointUser(User), request.SupplierId ?? "");
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
      Response = new SupplierRecord(obj.Address, obj.ContactPerson, obj.CostCentreCode, obj.Description, obj.Email, obj.Narration, obj.PostalCode, obj.SupplierCode, obj.Id, obj.Telephone, obj.Town, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
