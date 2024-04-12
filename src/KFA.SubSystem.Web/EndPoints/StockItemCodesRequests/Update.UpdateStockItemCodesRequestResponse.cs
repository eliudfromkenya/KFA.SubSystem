namespace KFA.SubSystem.Web.EndPoints.StockItemCodesRequests;

public class UpdateStockItemCodesRequestResponse
{
  public UpdateStockItemCodesRequestResponse(StockItemCodesRequestRecord stockItemCodesRequest)
  {
    StockItemCodesRequest = stockItemCodesRequest;
  }

  public StockItemCodesRequestRecord StockItemCodesRequest { get; set; }
}
