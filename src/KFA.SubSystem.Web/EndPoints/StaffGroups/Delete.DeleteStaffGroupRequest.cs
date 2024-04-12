namespace KFA.SubSystem.Web.EndPoints.StaffGroups;

public record DeleteStaffGroupRequest
{
  public const string Route = "/staff_groups/{groupNumber}";
  public static string BuildRoute(string? groupNumber) => Route.Replace("{groupNumber}", groupNumber);
  public string? GroupNumber { get; set; }
}
