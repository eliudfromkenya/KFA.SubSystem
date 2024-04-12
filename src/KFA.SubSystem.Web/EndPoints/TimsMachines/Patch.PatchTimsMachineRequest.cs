using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.TimsMachines;

public class PatchTimsMachineRequest : JsonPatchDocument<TimsMachineDTO>, IPlainTextRequest
{
  public const string Route = "/tims_machines/{machineID}";

  public static string BuildRoute(string machineID) => Route.Replace("{machineID}", machineID);

  public string MachineID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<TimsMachineDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<TimsMachineDTO>>(Content)!;
}
