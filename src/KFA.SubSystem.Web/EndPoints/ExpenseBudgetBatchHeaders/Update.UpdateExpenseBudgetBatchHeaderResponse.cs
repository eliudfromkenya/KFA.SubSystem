﻿namespace KFA.SubSystem.Web.EndPoints.ExpenseBudgetBatchHeaders;

public class UpdateExpenseBudgetBatchHeaderResponse
{
  public UpdateExpenseBudgetBatchHeaderResponse(ExpenseBudgetBatchHeaderRecord expenseBudgetBatchHeader)
  {
    ExpenseBudgetBatchHeader = expenseBudgetBatchHeader;
  }

  public ExpenseBudgetBatchHeaderRecord ExpenseBudgetBatchHeader { get; set; }
}
