using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.DuesPaymentDetails;

public class PatchDuesPaymentDetailRequest : JsonPatchDocument<DuesPaymentDetailDTO>, IPlainTextRequest
{
  public const string Route = "/dues_payment_details/{paymentID}";

  public static string BuildRoute(string paymentID) => Route.Replace("{paymentID}", paymentID);

  public string PaymentID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<DuesPaymentDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<DuesPaymentDetailDTO>>(Content)!;
}
