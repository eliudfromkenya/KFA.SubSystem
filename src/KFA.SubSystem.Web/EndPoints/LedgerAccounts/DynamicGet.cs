﻿using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;
using KFA.SubSystem.UseCases.Models.List;
using KFA.SubSystem.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SubSystem.Web.EndPoints.LedgerAccounts;

/// <summary>
/// List all ledger account.
/// </summary>
/// <remarks>
/// Dynamically Get all ledger account as specified - returns a dynamic list of the ledger account.
/// </remarks>
public class DynamicGet(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, string>
{
  private const string EndPointId = "ENP-1J3";
  public const string Route = "/ledger_accounts/dynamically";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Ledger Accounts Dynamically End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves dynamically of ledger accounts as specified";
      s.Description = "Dynamically returns all ledger accounts as specified, i.e filter to specify which records or rows to return, selector to specify which properties or columns to return, order to specify order criteria. It returns a JSON result in form of a string.";
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new (Id, Narration)", Parameters = ["SVR", "SVR-01"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new DynamicsListModelsQuery<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), request ?? new ListParam { });
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = result.Value;
    }
  }
}
