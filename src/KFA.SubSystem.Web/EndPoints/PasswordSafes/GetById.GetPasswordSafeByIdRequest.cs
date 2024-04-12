namespace KFA.SubSystem.Web.EndPoints.PasswordSafes;

public class GetPasswordSafeByIdRequest
{
  public const string Route = "/password_safes/{passwordId}";

  public static string BuildRoute(string? passwordId) => Route.Replace("{passwordId}", passwordId);

  public string? PasswordId { get; set; }
}
