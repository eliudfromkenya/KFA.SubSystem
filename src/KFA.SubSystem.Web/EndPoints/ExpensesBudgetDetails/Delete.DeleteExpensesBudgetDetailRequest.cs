namespace KFA.SubSystem.Web.EndPoints.ExpensesBudgetDetails;

public record DeleteExpensesBudgetDetailRequest
{
  public const string Route = "/expenses_budget_details/{expenseBudgetDetailId}";
  public static string BuildRoute(string? expenseBudgetDetailId) => Route.Replace("{expenseBudgetDetailId}", expenseBudgetDetailId);
  public string? ExpenseBudgetDetailId { get; set; }
}
