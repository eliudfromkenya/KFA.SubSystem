namespace KFA.SubSystem.Web.EndPoints.StockItems;

public class UpdateStockItemResponse
{
  public UpdateStockItemResponse(StockItemRecord stockItem)
  {
    StockItem = stockItem;
  }

  public StockItemRecord StockItem { get; set; }
}
