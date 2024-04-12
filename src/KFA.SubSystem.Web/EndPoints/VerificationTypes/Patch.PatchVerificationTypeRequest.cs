using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.VerificationTypes;

public class PatchVerificationTypeRequest : JsonPatchDocument<VerificationTypeDTO>, IPlainTextRequest
{
  public const string Route = "/verification_types/{verificationTypeId}";

  public static string BuildRoute(string verificationTypeId) => Route.Replace("{verificationTypeId}", verificationTypeId);

  public string VerificationTypeId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<VerificationTypeDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<VerificationTypeDTO>>(Content)!;
}
