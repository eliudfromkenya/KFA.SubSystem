namespace KFA.SubSystem.Web.EndPoints.StockCountSheets;

public class UpdateStockCountSheetResponse
{
  public UpdateStockCountSheetResponse(StockCountSheetRecord stockCountSheet)
  {
    StockCountSheet = stockCountSheet;
  }

  public StockCountSheetRecord StockCountSheet { get; set; }
}
