﻿namespace KFA.SubSystem.Web.EndPoints.PayrollGroups;

public record DeletePayrollGroupRequest
{
  public const string Route = "/payroll_groups/{groupID}";
  public static string BuildRoute(string? groupID) => Route.Replace("{groupID}", groupID);
  public string? GroupID { get; set; }
}
