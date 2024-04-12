using KFA.SubSystem.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SubSystem.Web.EndPoints.LedgerAccounts;

public class PatchLedgerAccountRequest : JsonPatchDocument<LedgerAccountDTO>, IPlainTextRequest
{
  public const string Route = "/ledger_accounts/{ledgerAccountId}";

  public static string BuildRoute(string ledgerAccountId) => Route.Replace("{ledgerAccountId}", ledgerAccountId);

  public string LedgerAccountId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<LedgerAccountDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<LedgerAccountDTO>>(Content)!;
}
