using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.IssuesProgresses;

public class PatchIssuesProgressRequest : JsonPatchDocument<IssuesProgressDTO>, IPlainTextRequest
{
  public const string Route = "/issues_progresses/{progressID}";

  public static string BuildRoute(string progressID) => Route.Replace("{progressID}", progressID);

  public string ProgressID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<IssuesProgressDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<IssuesProgressDTO>>(Content)!;
}
