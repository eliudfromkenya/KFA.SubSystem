using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.PasswordSafes;

public class PatchPasswordSafeRequest : JsonPatchDocument<PasswordSafeDTO>, IPlainTextRequest
{
  public const string Route = "/password_safes/{passwordId}";

  public static string BuildRoute(string passwordId) => Route.Replace("{passwordId}", passwordId);

  public string PasswordId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<PasswordSafeDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<PasswordSafeDTO>>(Content)!;
}
