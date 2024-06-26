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

namespace KFA.SubSystem.Web.EndPoints.QRCodesRequests;

/// <summary>
/// List all qr codes requests by specified conditions
/// </summary>
/// <remarks>
/// List all qr codes requests - returns a QRCodesRequestListResponse containing the qr codes requests.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, QRCodesRequestListResponse>
{
  private const string EndPointId = "ENP-1R5";
  public const string Route = "/qr_codes_requests";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get QR Codes Requests List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of qr codes requests as specified";
      s.Description = "Returns all qr codes requests as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new QRCodesRequestListResponse { QRCodesRequests = [new QRCodesRequestRecord("0", true, "Narration", "1000", "Request Data", "Response Data", Core.DataLayer.Types.QRResponseType.Recieved, DateTime.Now, "Tims Machine used", "VAT Class", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<QRCodesRequestDTO, QRCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<QRCodesRequestDTO>>.Success(ans.Select(v => (QRCodesRequestDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new QRCodesRequestListResponse
      {
        QRCodesRequests = result.Value.Select(obj => new QRCodesRequestRecord(obj.CostCentreCode, obj.IsDuplicate, obj.Narration, obj.Id, obj.RequestData, obj.ResponseData, obj.ResponseStatus, obj.Time, obj.TimsMachineUsed, obj.VATClass, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
