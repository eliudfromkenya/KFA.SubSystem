using KFA.SubSystem.Web.EndPoints.CostCentres;

namespace KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;

public class CostCentreListResponse
{
  public List<CostCentreRecord> CostCentres { get; set; } = [];
}
