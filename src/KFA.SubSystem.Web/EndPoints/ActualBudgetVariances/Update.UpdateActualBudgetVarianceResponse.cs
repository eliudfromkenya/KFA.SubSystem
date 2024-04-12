namespace KFA.SubSystem.Web.EndPoints.ActualBudgetVariances;

public class UpdateActualBudgetVarianceResponse
{
  public UpdateActualBudgetVarianceResponse(ActualBudgetVarianceRecord actualBudgetVariance)
  {
    ActualBudgetVariance = actualBudgetVariance;
  }

  public ActualBudgetVarianceRecord ActualBudgetVariance { get; set; }
}
