﻿namespace KFA.SubSystem.Web.EndPoints.ProjectIssues;

public class GetProjectIssueByIdRequest
{
  public const string Route = "/project_issues/{projectIssueID}";

  public static string BuildRoute(string? projectIssueID) => Route.Replace("{projectIssueID}", projectIssueID);

  public string? ProjectIssueID { get; set; }
}
