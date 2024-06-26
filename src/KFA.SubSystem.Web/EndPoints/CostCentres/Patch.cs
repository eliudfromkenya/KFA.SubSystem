﻿using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Classes;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Patch;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchCostCentreRequest, CostCentreRecord>
{
  private const string EndPointId = "ENP-156";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchCostCentreRequest.Route));
    //RequestBinder(new PatchBinder<CostCentreDTO, CostCentre, PatchCostCentreRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Cost Centre End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a cost centre";
      s.Description = "Used to update part of an existing cost centre. A valid existing cost centre is required.";
      s.ResponseExamples[200] = new CostCentreRecord("1000", "Description", "Narration", "Region", "Supplier Code Prefix", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchCostCentreRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode, "The cost centre of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    CostCentreDTO patchFunc(CostCentreDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<CostCentreDTO, CostCentre>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the cost centre to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(obj.Id, obj.Description, obj.Narration, obj.Region, obj.SupplierCodePrefix, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
