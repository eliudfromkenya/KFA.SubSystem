using System.ComponentModel.DataAnnotations;
using KFA.SubSystem.Web.EndPoints.CostCentres;

namespace KFA.SubSystem.Web.BusinessCentralEndPoints.VAT;

public class ExcelSupplierTransactionsRequest
{
  public string? MonthFrom { get; set; } = "2016-07";
  public string? MonthTo { get; set; } = "2022-10";
//[Dii("Supplier Prefix or branch code")]
  public string? SupplierPrefix { get; set; }
}
