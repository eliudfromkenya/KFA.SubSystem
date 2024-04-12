namespace KFA.SubSystem.Web.EndPoints.StockCountSheets;

public class GetStockCountSheetByIdRequest
{
  public const string Route = "/stock_count_sheets/{countSheetId}";

  public static string BuildRoute(string? countSheetId) => Route.Replace("{countSheetId}", countSheetId);

  public string? CountSheetId { get; set; }
}
