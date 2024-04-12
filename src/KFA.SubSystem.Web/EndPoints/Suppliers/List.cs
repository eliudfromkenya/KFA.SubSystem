using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;
using KFA.SubSystem.UseCases.Models.List;
using KFA.SubSystem.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SubSystem.Web.EndPoints.Suppliers;

/// <summary>
/// List all suppliers by specified conditions
/// </summary>
/// <remarks>
/// List all suppliers - returns a SupplierListResponse containing the suppliers.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, SupplierListResponse>
{
  private const string EndPointId = "ENP-1Z5";
  public const string Route = "/suppliers";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Suppliers List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of suppliers as specified";
      s.Description = "Returns all suppliers as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new SupplierListResponse { Suppliers = [new SupplierRecord("Address", "Contact Person", "Cost Centre Code", "Description", "Email", "Narration", "Postal Code", "Supplier Code", "1000", "Telephone", "Town", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<SupplierDTO, Supplier>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<SupplierDTO>>.Success(ans.Select(v => (SupplierDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new SupplierListResponse
      {
        Suppliers = result.Value.Select(obj => new SupplierRecord(obj.Address, obj.ContactPerson, obj.CostCentreCode, obj.Description, obj.Email, obj.Narration, obj.PostalCode, obj.SupplierCode, obj.Id, obj.Telephone, obj.Town, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
