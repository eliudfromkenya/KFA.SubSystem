using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Services.DataAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using KFA.SubSystem.Services.Models;
namespace KFA.SubSystem.Services.Services.OldSystem;
internal static class ExcelStatement
{
   public static async Task<byte[]> Create(string supplierPrefix, string monthFrom, string monthTo)
  {
    var pars = MySQLDbService.CreateParameters(new Dictionary<string, object> { { "@supplierPrefix", supplierPrefix }, { "@fromMonth", monthFrom }, { "@toMonth", monthTo } });

    var sql = Functions.ReadManifestData<LedgerRecord>("KFA.SubSystem.Services.Resources.SQLTexts.SupplierStatementOld.sql");
    using var ds = await MySQLDbService.MySQLGetDataset(sql!, pars!);
    return ExportToExcel(ds, supplierPrefix, monthFrom, monthTo);
  }

  internal static byte[] ExportToExcel(DataSet ds, string supplierPrefix, string from, string to)
  {
    ExcelPackage excel = new ExcelPackage();
    DataTable cashReceipts = ds.Tables[0], cheques = ds.Tables[2], pettyCash = ds.Tables[3], purchases = ds.Tables[4], invoices = ds.Tables[5], journals = ds.Tables[1], costCentres = ds.Tables[6], stockItems = ds.Tables[7], ledgerAccounts = ds.Tables[8];

    var codes = cashReceipts.AsEnumerable().Select(c => c["credit_ledger_account_code"]?.ToString()).Concat(
                cheques.AsEnumerable().Select(c => c["debit_ledger_account_code"]?.ToString()).Concat(
                pettyCash.AsEnumerable().Select(c => c["debit_ledger_account_code"]?.ToString()).Concat(
                purchases.AsEnumerable().Select(c => c["supplier_code"]?.ToString()).Concat(
                invoices.AsEnumerable().Select(c => c["supplier_code"]?.ToString()).Concat(
                journals.AsEnumerable().Select(c => c["credit_ledger_account_code"]?.ToString()).Concat(
                journals.AsEnumerable().Select(c => c["debit_ledger_account_code"]?.ToString())))))))
      .Where(m => m?.StartsWith(supplierPrefix) ?? false).Distinct().OrderBy(mbox => mbox).ToList();

    var xItems = stockItems.AsEnumerable().Select(x => new
    {
      ItemCode = x[0].ToString(),
      ItemName = x[1].ToString()
    }).ToArray();

    var xCostCentres = costCentres.AsEnumerable().Select(x => new
    {
      Code = x[0].ToString(),
      Name = x[1].ToString(),
      Prefix = x[2].ToString()
    }).ToArray();

    var xLedgerAccounts = ledgerAccounts.AsEnumerable().Select(x => new
    {
      Code = x[0].ToString(),
      Name = x[1].ToString()
    }).ToArray();

    var codeWithBranch = codes.Select(c =>
    (Prefix: c, Branch: xCostCentres.FirstOrDefault(x => c?.ToLower()?.StartsWith(x.Prefix?.ToLower() ?? "") ?? false))).ToArray();

    foreach (var code in codeWithBranch)
    {
      var m1 = code.Prefix;
      var m2 = code.Branch?.Prefix;
      var m3 = code.Branch?.Name;
      var m4 = code.Branch?.Code;
    }
      
    foreach (DataTable table in ds.Tables)
    {
      var cols = table.Columns.OfType<DataColumn>().ToList();
      var sheet = ExcelReporter.CreateWorkSheet(ref excel, table.TableName, $"Period: {from:dd-MM-yyyy} to {to:dd-MM-yyyy}", cols.Count);

      var toChar = ((char)('A' + cols.Count)).ToString();
      var currentRow = 2;

      currentRow += 1;
      var range = sheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      //range.Merge = true;
      //range.Value = $"{recs.Key} - {branches.FirstOrDefault(x => x.Key == recs.Key).Value}";
      sheet.Row(currentRow).Height = 20;
      sheet.Row(currentRow).Style.Font.Size = 18;
      sheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkGray);
      sheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      sheet.Row(currentRow).Style.Font.Bold = true;
      currentRow += 1;
      range = sheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 11;

      for (int i = 0; i < cols.Count; i++)
        sheet.Cells[currentRow, i + 1, currentRow, i + 1].Value = cols[i].ColumnName;

      int rowStart = currentRow + 1;

      var branches = Data.Branches.Split("\n")
              .Select(x => x.Split(','))
              .Where(c => c?.Length == 2)
              .ToDictionary(c => c.First().Trim(), v => v.Last()?.Trim());

      table.AsEnumerable()
       .ToList().ForEach(row =>
       {
         currentRow += 1;
         for (int i = 0; i < row.ItemArray.Length; i++)
           sheet.Cells[currentRow, i + 1, currentRow, i + 1].Value = row[i];
       });

      for (int i = 0; i < cols.Count; i++)
      {
        var col = cols[i];
        var toCharLetter = ((char)('A' + i)).ToString();
        if (col.DataType == typeof(decimal) || col.DataType == typeof(double))
        {
          sheet.Cells[$"{toCharLetter}{currentRow + 1}:{toCharLetter}{currentRow + 1}"].Formula = $"SUM({toCharLetter}{rowStart}:{toCharLetter}{currentRow})";
          sheet.Cells[$"{toCharLetter}{rowStart}:{toCharLetter}{currentRow + 2}"].Style.Numberformat.Format = "#,##0.00";

          var rng = sheet.Cells[$"{toCharLetter}{currentRow + 1}:{toCharLetter}{currentRow + 1}"];
          rng.Style.Fill.PatternType = ExcelFillStyle.LightUp;
          rng.Style.Fill.BackgroundColor.SetColor(Color.LightPink);
          rng.Style.Font.Size = 14;
          rng.Style.Font.Bold = true;
          rng.Style.Font.UnderLine = true;
          rng.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
        }
        else if (col.DataType == typeof(DateOnly) || col.DataType == typeof(DateTime))
        {
          sheet.Cells[$"{toCharLetter}{rowStart}:{toCharLetter}{currentRow + 2}"].Style.Numberformat.Format = "dd MMM, yyyy";
        }
      }
      sheet.Cells[sheet.Dimension.Address].AutoFitColumns(); sheet.View.FreezePanes(5, 1);
      for (int i = 0; i < cols.Count; i++)
        sheet.Column(i + 1).Width = sheet.Column(i + 1).Width + 5;
    }

    var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VAT Issues.xlsx"));
    excel.SaveAs(file);
    return excel.GetAsByteArray();
  }
}
