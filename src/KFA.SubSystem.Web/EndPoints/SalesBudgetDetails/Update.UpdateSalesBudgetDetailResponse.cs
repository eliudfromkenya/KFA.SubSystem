﻿namespace KFA.SubSystem.Web.EndPoints.SalesBudgetDetails;

public class UpdateSalesBudgetDetailResponse
{
  public UpdateSalesBudgetDetailResponse(SalesBudgetDetailRecord salesBudgetDetail)
  {
    SalesBudgetDetail = salesBudgetDetail;
  }

  public SalesBudgetDetailRecord SalesBudgetDetail { get; set; }
}
