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
using KFA.SubSystem.Web.BusinessCentralEndPoints.VAT;
using System.Collections;

namespace KFA.SubSystem.Web.KFAInternalSystemsEndPoints;

/// <summary>
/// List all cost centres by specified conditions
/// </summary>
/// <remarks>
/// List all cost centres - returns a CostCentreListResponse containing the cost centres.
/// </remarks>
public class ExcelSupplierTransactionsEndPoint(IEndPointManager endPointManager) : Endpoint<ExcelSupplierTransactionsRequest, byte[]>
{
  private const string EndPointId = "KFA-01";
  public const string Route = "/kfa-internal-systems/supplier-transactions";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Excel  Supplier Transactions end point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves all supplier transactions in a given period";
      s.Description = "Returns an excel and order to specify order criteria";
      s.ExampleRequest = new ExcelSupplierTransactionsRequest
      {
        MonthFrom = "2016-07",
        MonthTo = "2022-10",
        SupplierPrefix = "3300"
      };
    });
  }

  public override async Task<byte[]> HandleAsync(ExcelSupplierTransactionsRequest request,
    CancellationToken cancellationToken)
  {
    var ans = await SubSystem.Services.DataAnalysis.SupplierStatementOld.GetSupplierTransactions(request.SupplierPrefix ?? "3100", request.MonthFrom ?? "2016-07", request.MonthTo ?? "2022-10") ?? [];
    await SendBytesAsync(ans!, "Supplier Transactions.xlsx", cancellation: cancellationToken);
    return ans;
  }
}
