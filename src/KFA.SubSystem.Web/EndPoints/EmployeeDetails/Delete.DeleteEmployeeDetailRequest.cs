﻿namespace KFA.SubSystem.Web.EndPoints.EmployeeDetails;

public record DeleteEmployeeDetailRequest
{
  public const string Route = "/employee_details/{employeeID}";
  public static string BuildRoute(string? employeeID) => Route.Replace("{employeeID}", employeeID);
  public string? EmployeeID { get; set; }
}
