using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Services.Models;
using KFA.SubSystem.Services.Services;

namespace KFA.SubSystem.Services.DataAnalysis;
public class VAT
{
  public static async Task<byte[]> GetVATEntriesNotInSales(DateOnly dateFrom, DateOnly dateTo)
  {
    var pars = MySQLDbService.CreateParameters(new Dictionary<string, object> { { "@dateFrom", dateFrom}, { "@dateTo", dateTo } });

    var sql = Functions.ReadManifestData<LedgerRecord>("KFA.SubSystem.Services.Resources.SQLTexts.VatIssues.sql");
    using var ds = await MySQLDbService.MySQLGetDataset(sql!, pars!);
    string[] tableNames = ["Purchases with VAT not posted to VAT Input","VAT Input Entries not found in Purchases","Sales with VAT not posted to VAT output Ledger", "VAT output ledger entries not in Sales","Sales not sent to KRA", "Sales VAT different from Ledger VAT","Purchase VAT different from ledger VAT", "VAT to KRA different from VAT to ledgers", "Duplicated VAT Input(Purchases) to Ledgers", "Duplicated VAT Output (Sales) to Ledgers", "Sales with rounding off issues", "VAT to KRA Summary by Branch"];
    for (int i = 0; i < tableNames.Length; i++)
      ds.Tables[i].TableName = tableNames[i];
    return ExcelReporter.ExportToExcel(ds, dateFrom, dateTo);
  }
   // => ServiceFunctions.GetExcelFile(await VATEntriesNotInSales.Process(dateFrom, dateTo)) ?? [];
}
