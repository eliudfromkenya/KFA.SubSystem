﻿namespace KFA.SubSystem.Web.EndPoints.PurchasesBudgetBatchHeaders;

public class UpdatePurchasesBudgetBatchHeaderResponse
{
  public UpdatePurchasesBudgetBatchHeaderResponse(PurchasesBudgetBatchHeaderRecord purchasesBudgetBatchHeader)
  {
    PurchasesBudgetBatchHeader = purchasesBudgetBatchHeader;
  }

  public PurchasesBudgetBatchHeaderRecord PurchasesBudgetBatchHeader { get; set; }
}
