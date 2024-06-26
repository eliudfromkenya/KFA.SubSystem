﻿using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Infrastructure.Services;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;
using KFA.SubSystem.UseCases.Models.List;
using KFA.SubSystem.Web.Services;
using Mapster;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SubSystem.Web.EndPoints.EmployeeDetails;

/// <summary>
/// List all employee details by specified conditions
/// </summary>
/// <remarks>
/// List all employee details - returns a EmployeeDetailListResponse containing the employee details.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, EmployeeDetailListResponse>
{
  private const string EndPointId = "ENP-1B5";
  public const string Route = "/employee_details";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Employee Details List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of employee details as specified";
      s.Description = "Returns all employee details as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new EmployeeDetailListResponse { EmployeeDetails = [new EmployeeDetailRecord(0, "Classification", "Cost Centre Code", DateTime.Now, "Email", "1000", "Full Name", "Gender", "Group Number", "Id Number", "Narration", "", "Payroll Number", "Phone Number", DateTime.Now, "Remarks", 0, 0, DateTime.Now, "Status", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<EmployeeDetailDTO, EmployeeDetail>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);

    var config = TypeAdapterConfig<EmployeeDetail, EmployeeDetailDTO>.NewConfig();

    config.Map(dest => dest.Date, src => BaseDTO<EmployeeDetail>.ToDate(src.Date))
    .Map(dest => dest.RejoinDate, src => BaseDTO<EmployeeDetail>.ToDate(src.RejoinDate))
    .Map(dest => dest.RetrenchmentDate, src => BaseDTO<EmployeeDetail>.ToDate(src.RetrenchmentDate));

    var result = Result<List<EmployeeDetailDTO>>.Success(ans.Select(v => v.Adapt<EmployeeDetailDTO>()).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new EmployeeDetailListResponse
      {
        EmployeeDetails = result.Value.Select(obj => new EmployeeDetailRecord(obj.AmountDue, obj.Classification, obj.CostCentreCode, obj.Date, obj.Email, obj.Id, obj.FullName, obj.Gender, obj.GroupNumber, obj.IdNumber, obj.Narration, obj.PayrollGroupID, obj.PayrollNumber, obj.PhoneNumber, obj.RejoinDate, obj.Remarks, obj.RetireeAmount, obj.RetrenchmentAmount, obj.RetrenchmentDate, obj.Status, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
