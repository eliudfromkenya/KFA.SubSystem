namespace KFA.SubSystem.Web.EndPoints.PurchasesBudgetDetails;

public class UpdatePurchasesBudgetDetailResponse
{
  public UpdatePurchasesBudgetDetailResponse(PurchasesBudgetDetailRecord purchasesBudgetDetail)
  {
    PurchasesBudgetDetail = purchasesBudgetDetail;
  }

  public PurchasesBudgetDetailRecord PurchasesBudgetDetail { get; set; }
}
