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

namespace KFA.SubSystem.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// List all issues submissions by specified conditions
/// </summary>
/// <remarks>
/// List all issues submissions - returns a IssuesSubmissionListResponse containing the issues submissions.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, IssuesSubmissionListResponse>
{
  private const string EndPointId = "ENP-1G5";
  public const string Route = "/issues_submissions";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Issues Submissions List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of issues submissions as specified";
      s.Description = "Returns all issues submissions as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new IssuesSubmissionListResponse { IssuesSubmissions = [new IssuesSubmissionRecord("Issue ID", "Narration", Core.Models.Types.IssueStatus.Done, "1000", "Submitted To", "Submitting User",DateTime.Now, "Type", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<IssuesSubmissionDTO, IssuesSubmission>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<IssuesSubmissionDTO>>.Success(ans.Select(v => (IssuesSubmissionDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new IssuesSubmissionListResponse
      {
        IssuesSubmissions = result.Value.Select(obj => new IssuesSubmissionRecord(obj.IssueID, obj.Narration, obj.Status, obj.Id, obj.SubmittedTo, obj.SubmittingUser, obj.TimeSubmitted, obj.Type, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
