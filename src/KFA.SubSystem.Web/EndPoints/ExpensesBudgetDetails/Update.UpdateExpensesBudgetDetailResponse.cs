namespace KFA.SubSystem.Web.EndPoints.ExpensesBudgetDetails;

public class UpdateExpensesBudgetDetailResponse
{
  public UpdateExpensesBudgetDetailResponse(ExpensesBudgetDetailRecord expensesBudgetDetail)
  {
    ExpensesBudgetDetail = expensesBudgetDetail;
  }

  public ExpensesBudgetDetailRecord ExpensesBudgetDetail { get; set; }
}
