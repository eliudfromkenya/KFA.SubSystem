using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.QRCodesRequests;

public class PatchQRCodesRequestRequest : JsonPatchDocument<QRCodesRequestDTO>, IPlainTextRequest
{
  public const string Route = "/qr_codes_requests/{qRCodeRequestID}";

  public static string BuildRoute(string qRCodeRequestID) => Route.Replace("{qRCodeRequestID}", qRCodeRequestID);

  public string QRCodeRequestID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<QRCodesRequestDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<QRCodesRequestDTO>>(Content)!;
}
