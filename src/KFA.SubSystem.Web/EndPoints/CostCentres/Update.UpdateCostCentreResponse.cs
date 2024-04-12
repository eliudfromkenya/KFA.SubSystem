using KFA.SubSystem.Web.EndPoints.CostCentres;

namespace KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;

public class UpdateCostCentreResponse
{
  public UpdateCostCentreResponse(CostCentreRecord costCentre)
  {
    CostCentre = costCentre;
  }

  public CostCentreRecord CostCentre { get; set; }
}
