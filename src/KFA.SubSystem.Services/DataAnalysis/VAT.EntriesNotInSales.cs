using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Globals.DataLayer;

namespace KFA.SubSystem.Services.DataAnalysis;

class VATEntriesNotInSales
{
  internal static async Task<DataTable> Process(DateOnly dateFrom, DateOnly dateTo)
  {
    var pars = MySQLDbService.CreateParameters(new Dictionary<string, object> { { "dateFrom", dateFrom }, { "dateTo", dateTo } });

    var sql = Functions.ReadManifestData<VATEntriesNotInSales>("KFA.SubSystem.Services.Resources.SQLTexts.VatEntriesNotInSales.sql");
    var ds = await MySQLDbService.MySQLGetDataset(sql!, pars!);
    return ds.Tables[0];
  }
}
