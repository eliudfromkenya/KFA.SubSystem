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
using KFA.SubSystem.Web.EndPoints.CostCentres;
using KFA.SubSystem.Services.Models;

namespace KFA.SubSystem.Web.BusinessCentralEndPoints.VAT;

/// <summary>
/// List all cost centres by specified conditions
/// </summary>
/// <remarks>
/// List all cost centres - returns a CostCentreListResponse containing the cost centres.
/// </remarks>
public class List(IEndPointManager endPointManager) : Endpoint<VATEntriesNotInSalesRequest,object>
{
  private const string EndPointId = "BC-VAT-01";
  public const string Route = "/business-central/vat";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("VAT ledger entries with no sales end point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of ledger entries not in posted sales as specified dates or cost centers";
      s.Description = "Returns all VAT ledger entries that can not be traced in cash sales, order to specify order criteria";
      s.ExampleRequest = new VATEntriesNotInSalesRequest
      {
        DateFrom = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
        DateTo = DateOnly.FromDateTime(DateTime.Now),
        BranchCode = "3300"
      };
    });
  }

  public override async Task<object> HandleAsync(VATEntriesNotInSalesRequest request,
    CancellationToken cancellationToken)
  {
    var ans = await SubSystem.Services.DataAnalysis.VAT.GetVATEntriesNotInSales(request.DateFrom, request.DateTo);
    await SendBytesAsync(ans!, "VAT Data Analysis.xlsx", cancellation: cancellationToken);
    return ans;
  }
}
