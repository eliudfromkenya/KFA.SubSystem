using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Services.Services;
using KFA.SubSystem.Services.Services.OldSystem;

namespace KFA.SubSystem.Services.DataAnalysis;
public class SupplierStatementOld
{
  public static async Task<byte[]> GetSupplierTransactions(string supplierPrefix, string monthFrom, string monthTo)
  {    
    return await ExcelStatement.Create(supplierPrefix, monthFrom, monthTo);
  }
}
