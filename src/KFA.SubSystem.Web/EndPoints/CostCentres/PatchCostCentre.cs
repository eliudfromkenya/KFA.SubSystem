﻿using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Classes;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.Models.Patch;
using KFA.SubSystem.Web.Binders;
using KFA.SubSystem.Web.EndPoints.CostCentres;
using KFA.SubSystem.Web.Services;
using MediatR;

namespace KFA.SubSystem.UseCases.Models.Update;

public class PatchCostCentre(IMediator mediator) : Endpoint<PatchCostCentreRequest, CostCentreRecord>
{
  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchCostCentreRequest.Route));
    //RequestBinder(new PatchBinder<CostCentreDTO, CostCentre, PatchCostCentreRequest>());
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Partial Update Cost Centre"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Update partially a cost centre";
      s.Description = "Updates part of an existing CostCentre. A valid existing is required.";
      //s.ExampleRequest = new PatchRequest { Id = "Id to update", Content = @"{{Description: ""New Cost center Name""}}" };
      s.ResponseExamples[200] = new CostCentreRecord("Id", "Name", "Narration", "Region", "Supplier Code", true, DateTime.UtcNow, DateTime.UtcNow);
    });
  }

  public override async Task HandleAsync(PatchCostCentreRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode ?? "Id", "Id of item to be updated is required please");
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
      ThrowIfAnyErrors();
    }

    var value = result.Value;
    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.IsActive, value?.DateInserted___, value?.DateUpdated___);
    }
  }
}