using KFA.SubSystem.Web.EndPoints.CostCentres;

namespace KFA.SubSystem.Web.BusinessCentralEndPoints.VAT;

public class VATEntriesNotInSalesRequest
{
  public DateOnly DateFrom { get; set; }
  public DateOnly DateTo { get; set; }
  public string? BranchCode { get; set; }
}
