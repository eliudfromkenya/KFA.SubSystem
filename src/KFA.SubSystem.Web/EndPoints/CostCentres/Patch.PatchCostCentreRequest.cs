﻿using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

public class PatchCostCentreRequest : JsonPatchDocument<CostCentreDTO>, IPlainTextRequest
{
  public const string Route = "/cost_centres/{costCentreCode}";

  public static string BuildRoute(string costCentreCode) => Route.Replace("{costCentreCode}", costCentreCode);

  public string CostCentreCode { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<CostCentreDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<CostCentreDTO>>(Content)!;
}
