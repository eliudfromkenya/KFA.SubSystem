using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.UserRoles;

public class PatchUserRoleRequest : JsonPatchDocument<UserRoleDTO>, IPlainTextRequest
{
  public const string Route = "/user_roles/{roleId}";

  public static string BuildRoute(string roleId) => Route.Replace("{roleId}", roleId);

  public string RoleId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<UserRoleDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<UserRoleDTO>>(Content)!;
}
