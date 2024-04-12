using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.Verifications;

public class PatchVerificationRequest : JsonPatchDocument<VerificationDTO>, IPlainTextRequest
{
  public const string Route = "/verifications/{verificationId}";

  public static string BuildRoute(string verificationId) => Route.Replace("{verificationId}", verificationId);

  public string VerificationId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<VerificationDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<VerificationDTO>>(Content)!;
}
