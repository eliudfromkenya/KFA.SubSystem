namespace KFA.SubSystem.Web.EndPoints.SalesBudgetBatchHeaders;

public class UpdateSalesBudgetBatchHeaderResponse
{
  public UpdateSalesBudgetBatchHeaderResponse(SalesBudgetBatchHeaderRecord salesBudgetBatchHeader)
  {
    SalesBudgetBatchHeader = salesBudgetBatchHeader;
  }

  public SalesBudgetBatchHeaderRecord SalesBudgetBatchHeader { get; set; }
}
