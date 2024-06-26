﻿namespace KFA.SubSystem.Web.EndPoints.LedgerAccounts;

public class GetLedgerAccountByIdRequest
{
  public const string Route = "/ledger_accounts/{ledgerAccountId}";

  public static string BuildRoute(string? ledgerAccountId) => Route.Replace("{ledgerAccountId}", ledgerAccountId);

  public string? LedgerAccountId { get; set; }
}
