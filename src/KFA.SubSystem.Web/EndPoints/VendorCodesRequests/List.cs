﻿using Ardalis.Result;
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

namespace KFA.SubSystem.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// List all vendor codes requests by specified conditions
/// </summary>
/// <remarks>
/// List all vendor codes requests - returns a VendorCodesRequestListResponse containing the vendor codes requests.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, VendorCodesRequestListResponse>
{
  private const string EndPointId = "ENP-275";
  public const string Route = "/vendor_codes_requests";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Vendor Codes Requests List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of vendor codes requests as specified";
      s.Description = "Returns all vendor codes requests as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new VendorCodesRequestListResponse { VendorCodesRequests = [new VendorCodesRequestRecord("Attanded By", "Cost Centre Code", "Description", "Narration", "Requesting User", "Status", DateTime.Now, DateTime.Now, "Vendor Code", "1000", "Vendor Type", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<VendorCodesRequestDTO, VendorCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<VendorCodesRequestDTO>>.Success(ans.Select(v => (VendorCodesRequestDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new VendorCodesRequestListResponse
      {
        VendorCodesRequests = result.Value.Select(obj => new VendorCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.Description, obj.Narration, obj.RequestingUser, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.VendorCode, obj.Id, obj.VendorType, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
