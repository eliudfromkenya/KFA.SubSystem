namespace KFA.SubSystem.Web.EndPoints.ProjectIssues;

public class UpdateProjectIssueResponse
{
  public UpdateProjectIssueResponse(ProjectIssueRecord projectIssue)
  {
    ProjectIssue = projectIssue;
  }

  public ProjectIssueRecord ProjectIssue { get; set; }
}
