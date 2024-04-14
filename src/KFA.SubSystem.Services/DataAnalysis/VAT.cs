using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Services.Models;

namespace KFA.SubSystem.Services.DataAnalysis;
public class VAT
{
  public async Task<List<LedgerRecord>?> GetVATEntriesNotInSales(DateOnly dateFrom, DateOnly dateTo)
    => ServiceFunctions.GetLedgerRecords(await VATEntriesNotInSales.Process(dateFrom, dateTo));
}
