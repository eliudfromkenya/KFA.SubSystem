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
      .Where(m => m?.StartsWith(supplierPrefix) ?? false).Distinct().Select(h => h?.ToUpper()).OrderBy(mbox => mbox).ToList();

    var xItems = stockItems.AsEnumerable().Select(x => new
    {
      ItemCode = x[0].ToString()?.ToUpper(),
      ItemName = x[1].ToString()
    }).ToArray();

    var xCostCentres = costCentres.AsEnumerable().Select(x => new
    {
      Code = x[0].ToString()?.ToUpper(),
      Name = x[1].ToString(),
      Prefix = x[2].ToString()?.ToUpper()
    }).ToArray();

    var xLedgerAccounts = ledgerAccounts.AsEnumerable().Select(x => new
    {
      Code = x[0].ToString()?.ToUpper(),
      Name = x[1].ToString()
    }).ToArray();

    var codeWithBranch = codes
      .Select(c =>
      new
      {
        Prefix = c?[..3],
        SupplierCode = c,
        Branch = xCostCentres.FirstOrDefault(x => c?[..3] == x.Prefix)
      }).GroupBy(c => c.Prefix).ToList();

    //var codeWithBranch = codes.Select(c =>
    //(SupplierCode: c, Branch: xCostCentres.FirstOrDefault(x => c?.ToLower()?.StartsWith(x.Prefix?.ToLower() ?? "") ?? false))).GroupBy(c => c.Branch?.Code).ToArray();

    int detailedCols = 8;
    var detailedSheet = ExcelReporter.CreateWorkSheet(ref excel, "Detailed Transactions", $"Period: {from:dd-MM-yyyy} to {to:dd-MM-yyyy}", detailedCols);
    //var summarySheet = ExcelReporter.CreateWorkSheet(ref excel, "Summarized Transactions", $"Period: {from:dd-MM-yyyy} to {to:dd-MM-yyyy}", summarizedCols);

    detailedSheet.Cells["A1"].Value = "KFA Supplier Transactions";

    DataTable table = new DataTable("Supplier Summaries");
    table.Columns.Add("Branch Code");
    table.Columns.Add("Branch Name");
    table.Columns.Add("Supplier Code");
    table.Columns.Add("Supplier Name");
    table.Columns.Add("Receipts Count", typeof(int));
    table.Columns.Add("Journals Count", typeof(int));
    table.Columns.Add("Cheques Count", typeof(int));
    table.Columns.Add("Petty Cash Count", typeof(int));
    table.Columns.Add("Invoices Count", typeof(int));
    table.Columns.Add("Net Change Count", typeof(int));
    table.Columns.Add("Cheques", typeof(decimal));
    table.Columns.Add("Petty Cash", typeof(decimal));
    table.Columns.Add("Receipts", typeof(decimal));
    table.Columns.Add("Journals", typeof(decimal));
    table.Columns.Add("Invoices", typeof(decimal));
    table.Columns.Add("Net Change", typeof(decimal));

    int currentRow = 4;
    foreach (var code in codeWithBranch)
    {
      currentRow += 2;
      var toChar = ((char)('A' + detailedCols)).ToString();
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"{code.Key} - {code.FirstOrDefault()?.Branch?.Name ?? ""}";
      detailedSheet.Row(currentRow).Style.Font.Size = 24;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkBlue);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      //int count = 1;
      foreach (var obj in code)
      {
        var row = table.NewRow();
        table.Rows.Add(row);

        var supplierCode = obj.SupplierCode;
        var supplierName = xLedgerAccounts.FirstOrDefault(c => c.Code == supplierCode)?.Name;
        var branchPrefix = obj.Prefix;
        var branchName = obj.Branch?.Name;
        var branchCode = obj.Branch?.Code;

        row["Branch Code"] = branchCode;
        row["Branch Name"] = branchName;
        row["Supplier Code"] = supplierCode;
        row["Supplier Name"] = supplierName;

        currentRow += 1;
        range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
        detailedSheet.Row(currentRow).Height = 20;
        range.Merge = true;
        range.Value = $"{supplierCode} - {supplierName}";
        detailedSheet.Row(currentRow).Style.Font.Size = 18;
        detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.RebeccaPurple);
        detailedSheet.Row(currentRow).Style.Font.UnderLine = true;

        detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        detailedSheet.Row(currentRow).Style.Font.Bold = true;

        var journalsSet = journals.Select($"debit_ledger_account_code = '{supplierCode}' or credit_ledger_account_code = \'{supplierCode}\'");
        var receiptsSet = cashReceipts.Select($"credit_ledger_account_code = \'{supplierCode}\'");
        var chequesSet = cheques.Select($"debit_ledger_account_code = \'{supplierCode}\'");
        var pettyCashSet = pettyCash.Select($"debit_ledger_account_code = \'{supplierCode}\'");
        var purchasesSet = purchases.Select($"supplier_code = \'{supplierCode}\'");
        var invoicesSet = invoices.Select($"supplier_code = \'{supplierCode}\'");


        row["Receipts Count"] = receiptsSet.Length;
        row["Journals Count"] = journalsSet.Length;
        row["Cheques Count"] = chequesSet.Length;
        row["Petty Cash Count"] = pettyCashSet.Length;
        row["Invoices Count"] = invoicesSet.Length;

        var rctAmt = receiptsSet.Sum(row => decimal.TryParse(row["credit_amount"].ToString(), out var amt2) ? amt2 : 0);
        //var jnlCrt = journalsSet.Where(m => m.).Sum(row => decimal.TryParse(row["credit_amount"].ToString(), out var amt3) ? amt3 : 0);
        var jnlDbt = journalsSet.Sum(row => decimal.TryParse(row["credit_amount"].ToString(), out var amt3) ? amt3 : 0);
        var chqAmt = chequesSet.Sum(row => decimal.TryParse(row["amount"].ToString(), out var amt4) ? amt4 : 0);
        var pcvAmt = pettyCashSet.Sum(row => decimal.TryParse(row["amount"].ToString(), out var amt5) ? amt5 : 0);
        var invsAmt = invoicesSet.Sum(row => decimal.TryParse(row["amount"].ToString(), out var amt6) ? amt6 : 0);
        row["Receipts"] = rctAmt;
        row["Journals"] = jnlDbt;
        row["Cheques"] = chqAmt;
        row["Petty Cash"] = pcvAmt;
        row["Invoices"] = invsAmt;
        row["Net Change"] = chqAmt + pcvAmt - (jnlDbt + rctAmt + invsAmt);



        if (purchasesSet.Length != 0)
          currentRow = UpdatePurchases(detailedSheet, purchasesSet, invoicesSet, currentRow, toChar);
        if (journalsSet.Length != 0)
          currentRow = UpdateJournals(detailedSheet, journalsSet, currentRow, toChar);
        if (receiptsSet.Length != 0)
          currentRow = UpdateCashReceipts(detailedSheet, receiptsSet, currentRow, toChar);
        if (chequesSet.Length != 0)
          currentRow = UpdateCheques(detailedSheet, chequesSet, currentRow, toChar);
        if (pettyCashSet.Length != 0)
          currentRow = UpdatePettyCash(detailedSheet, pettyCashSet, currentRow, toChar);
      }      
    }
    table.Columns.Remove("Branch Code");
    table.Columns.Remove("Branch Name");
    table.Columns.Remove("Receipts Count");
    table.Columns.Remove("Journals Count");
    table.Columns.Remove("Cheques Count");
    table.Columns.Remove("Petty Cash Count");
    table.Columns.Remove("Invoices Count");
    table.Columns.Remove("Net Change Count");
    table.Columns.Remove("Receipts");
    {
      var cols = table.Columns.OfType<DataColumn>().ToList();
      var sheet = ExcelReporter.CreateWorkSheet(ref excel, table.TableName, $"Period: {from:dd-MM-yyyy} to {to:dd-MM-yyyy}", cols.Count);

      var toChar = ((char)('A' + cols.Count)).ToString();
      currentRow = 2;

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

      for (int i = 0; i < (table.Rows.Count + 1); i++)
        sheet.Cells[$"G{i + 5}:G{i + 5}"].Formula = $"C{i + 5}+D{i + 5}-E{i + 5}-F{i + 5}";

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

      sheet.Cells[$"G{rowStart}:G{currentRow + 2}"].Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
      sheet.Cells[sheet.Dimension.Address].AutoFitColumns(); sheet.View.FreezePanes(5, 1);
      for (int i = 0; i < cols.Count; i++)
        sheet.Column(i + 1).Width = sheet.Column(i + 1).Width + 5;
    }

    var bytes = excel.GetAsByteArray();
    ServiceFunctions.OpenExcelFile(bytes);
    return bytes;
  }


  private static int UpdateCheques(ExcelWorksheet detailedSheet, DataRow[] chequesSet, int currentRow, string toChar)
  {
    if (chequesSet.Length > 0)
    {
      currentRow++;
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"Cheque Payments";
      detailedSheet.Row(currentRow).Style.Font.Size = 16;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkOliveGreen);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      currentRow++;
      range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 12;

      detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = "Cheque No.";
      detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = "Process Month";
      detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = "Posting Date";
      detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = "Description";
      detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = "Debit Ledger";
      detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = "Debit Amount";
      detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = "Credit Ledger";
      detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = "Credit Amount";

      int rowStart = currentRow + 1;
      chequesSet.ToList().ForEach(row =>
      {
        currentRow++;

        detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = row["cheque_number"];
        detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = $"{row["month"]} ({row["batch_key"]})";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = row["date"];
        detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = row["description"];
        detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = row["debit_ledger_account_code"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = row["amount"];
        detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = row["credit_ledger_account_code"];
        detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = row["amount"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"G{currentRow}:H{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Style.Numberformat.Format = "dd MMM, yyyy";
      });

      detailedSheet.Cells[$"F{currentRow + 1}:F{currentRow + 1}"].Formula = $"SUM(F{rowStart}:F{currentRow})";
      detailedSheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.Bold = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLine = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Numberformat.Format = "#,##0.00";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.PatternType = ExcelFillStyle.LightUp;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    }
    return (currentRow += 2);
  }


  private static int UpdatePettyCash(ExcelWorksheet detailedSheet, DataRow[] pettyCashSet, int currentRow, string toChar)
  {
    if (pettyCashSet.Length > 0)
    {
      currentRow++;
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"Petty Cash Payments";
      detailedSheet.Row(currentRow).Style.Font.Size = 16;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkOliveGreen);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      currentRow++;
      range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 12;

      detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = "Voucher No.";
      detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = "Process Month";
      detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = "Posting Date";
      detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = "Description";
      detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = "Debit Ledger";
      detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = "Debit Amount";
      detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = "Credit Ledger";
      detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = "Credit Amount";

      int rowStart = currentRow + 1;
      pettyCashSet.ToList().ForEach(row =>
      {
        currentRow++;

        detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = row["voucher_number"];
        detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = $"{row["month"]} ({row["batch_key"]})";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = row["date"];
        detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = row["description"];
        detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = row["debit_ledger_account_code"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = row["amount"];
        detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = row["credit_ledger_account_code"];
        detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = row["amount"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"G{currentRow}:H{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Style.Numberformat.Format = "dd MMM, yyyy";
      });

      detailedSheet.Cells[$"F{currentRow + 1}:F{currentRow + 1}"].Formula = $"SUM(F{rowStart}:F{currentRow})";
      detailedSheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.Bold = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLine = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Numberformat.Format = "#,##0.00";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.PatternType = ExcelFillStyle.LightUp;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    }
    return (currentRow += 2);
  }


  private static int UpdateCashReceipts(ExcelWorksheet detailedSheet, DataRow[] cashReceiptsSet, int currentRow, string toChar)
  {
    if (cashReceiptsSet.Length > 0)
    {
      currentRow++;
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"Cash Receipts";
      detailedSheet.Row(currentRow).Style.Font.Size = 16;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkOliveGreen);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      currentRow++;
      range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 12;

      detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = "505 No.";
      detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = "Process Month";
      detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = "Posting Date";
      detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = "Description";
      detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = "Debit Ledger";
      detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = "Debit Amount";
      detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = "Credit Ledger";
      detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = "Credit Amount";

      int rowStart = currentRow + 1;
      cashReceiptsSet.ToList().ForEach(row =>
      {
        currentRow++;

        detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = row["document_number"];
        detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = $"{row["month"]} ({row["batch_key"]})";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = row["date"];
        detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = row["description"];
        detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = row["debit_ledger_account_code"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = row["debit_amount"];
        detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = row["credit_ledger_account_code"];
        detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = row["credit_amount"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"G{currentRow}:H{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Style.Numberformat.Format = "dd MMM, yyyy";
      });

      detailedSheet.Cells[$"F{currentRow + 1}:F{currentRow + 1}"].Formula = $"SUM(F{rowStart}:F{currentRow})";
      detailedSheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.Bold = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLine = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Numberformat.Format = "#,##0.00";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.PatternType = ExcelFillStyle.LightUp;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    }
    return (currentRow += 2);
  }

  private static int UpdateJournals(ExcelWorksheet detailedSheet, DataRow[] journalsSet, int currentRow, string toChar)
  {
    if (journalsSet.Length > 0)
    {
      currentRow++;
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"Journals";
      detailedSheet.Row(currentRow).Style.Font.Size = 16;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkOliveGreen);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      currentRow++;
      range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 12;

      detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = "Document No.";
      detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = "Process Month";
      detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = "Posting Date";
      detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = "Description";
      detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = "Debit Ledger";
      detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = "Debit Amount";
      detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = "Credit Ledger";
      detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = "Credit Amount";

      int rowStart = currentRow + 1;
      journalsSet.ToList().ForEach(row =>
      {
        currentRow++;

        detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = row["document_number"];
        detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = $"{row["month"]} ({row["batch_key"]})";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = row["date"];
        detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = row["description"];
        detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = row["debit_ledger_account_code"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = row["debit_amount"];
        detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = row["credit_ledger_account_code"];
        detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = row["credit_amount"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"G{currentRow}:H{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Style.Numberformat.Format = "dd MMM, yyyy";
      });

      detailedSheet.Cells[$"F{currentRow + 1}:F{currentRow + 1}"].Formula = $"SUM(F{rowStart}:F{currentRow})";
      detailedSheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.Bold = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLine = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Numberformat.Format = "#,##0.00";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.PatternType = ExcelFillStyle.LightUp;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    }
    return (currentRow += 2);
  }



  private static int UpdateJournalse(ExcelWorksheet detailedSheet, DataRow[] journalsSet, int currentRow, string toChar)
  {
    if (journalsSet.Length > 0)
    {
      currentRow++;
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"Journals";
      detailedSheet.Row(currentRow).Style.Font.Size = 16;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkOliveGreen);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      currentRow++;
      range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 12;

      detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = "Batch";
      detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = "Process Month";
      detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = "Posting Date";
      detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = "Description";
      detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = "Debit Ledger";
      detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = "Debit Amount";
      detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = "Credit Ledger";
      detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = "Credit Amount";

      int rowStart = currentRow + 1;
      journalsSet.ToList().ForEach(row =>
      {
        currentRow++;

        detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = row["batch_key"];
        detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = row["month"];
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = row["date"];
        detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = row["description"];
        detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = row["debit_ledger_account_code"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Value = row["debit_amount"];
        detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = row["credit_ledger_account_code"];
        detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = row["credit_amount"];
        detailedSheet.Cells[$"F{currentRow}:F{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"G{currentRow}:H{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Style.Numberformat.Format = "dd MMM, yyyy";
      });

      detailedSheet.Cells[$"F{currentRow + 1}:F{currentRow + 1}"].Formula = $"SUM(F{rowStart}:F{currentRow})";
      detailedSheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.Bold = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLine = true;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Numberformat.Format = "#,##0.00";
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.PatternType = ExcelFillStyle.LightUp;
      detailedSheet.Cells[$"E{currentRow + 1}:H{currentRow + 1}"].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    }
    return (currentRow += 2);
  }
  private static int UpdatePurchases(ExcelWorksheet detailedSheet, DataRow[] purchasesSet, DataRow[] invoicesSet, int currentRow, string toChar)
  {
    if (purchasesSet.Length > 0)
    {
      currentRow++;
      var range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      detailedSheet.Row(currentRow).Height = 20;
      range.Merge = true;
      range.Value = $"Purchases";
      detailedSheet.Row(currentRow).Style.Font.Size = 16;
      detailedSheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkOliveGreen);
      detailedSheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      detailedSheet.Row(currentRow).Style.Font.Bold = true;

      currentRow++;
      range = detailedSheet.Cells[$"A{currentRow}:{toChar}{currentRow}"];
      range.Style.Font.Color.SetColor(Color.MediumPurple);
      range.Style.Font.UnderLine = true;
      range.Style.Font.Size = 12;

      detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = "Batch";
      detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = "Process Month";
      detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = "Date of Supply";
      detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = "LPO Number";
      detailedSheet.Cells[$"E{currentRow}:F{currentRow}"].Merge = true;
      detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = "Supplier Invoices";
      detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = "Stock Value";
      detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = "Invoice Amount";

      int rowStart = currentRow + 1;
      purchasesSet.GroupBy(c => c["lpo_number"].ToString()).OrderBy(c => c.First()["date"]).ToList().ForEach(rows =>
      {
        currentRow++;
        var row = rows.First();
        var stockAmount = rows.Sum(v => decimal.TryParse(v["amount"].ToString(), out var num) ? num : 0);
        var invoices = invoicesSet.Where(c => c["lpo_number"].ToString() == rows.Key)
        .Select(n => new
        {
          Invoice = n["invoice_number"].ToString(),
          Amount = decimal.TryParse(n["amount"].ToString(), out var num) ? num : 0
        }).ToList();

        detailedSheet.Cells[$"A{currentRow}:A{currentRow}"].Value = row["batch_key"];
        detailedSheet.Cells[$"B{currentRow}:B{currentRow}"].Value = row["month"];
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Value = row["date"];
        detailedSheet.Cells[$"D{currentRow}:D{currentRow}"].Value = row["lpo_number"];
        detailedSheet.Cells[$"E{currentRow}:F{currentRow}"].Merge = true;
        detailedSheet.Cells[$"E{currentRow}:E{currentRow}"].Value = string.Join(", ", invoices.Select(n => n.Invoice));
        detailedSheet.Cells[$"G{currentRow}:G{currentRow}"].Value = stockAmount;
        detailedSheet.Cells[$"H{currentRow}:H{currentRow}"].Value = invoices.Sum(n => n.Amount);
        detailedSheet.Cells[$"G{currentRow}:H{currentRow}"].Style.Numberformat.Format = "#,##0.00";
        detailedSheet.Cells[$"C{currentRow}:C{currentRow}"].Style.Numberformat.Format = "dd MMM, yyyy";
      });

      detailedSheet.Cells[$"G{currentRow + 1}:G{currentRow + 1}"].Formula = $"SUM(G{rowStart}:G{currentRow})";
      detailedSheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
      detailedSheet.Cells[$"G{currentRow + 1}:H{currentRow + 1}"].Style.Font.Bold = true;
      detailedSheet.Cells[$"G{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLine = true;
      detailedSheet.Cells[$"G{currentRow + 1}:H{currentRow + 1}"].Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
      detailedSheet.Cells[$"G{currentRow + 1}:H{currentRow + 1}"].Style.Numberformat.Format = "#,##0.00";
      detailedSheet.Cells[$"G{currentRow + 1}:H{currentRow + 1}"].Style.Fill.PatternType = ExcelFillStyle.LightUp;
      detailedSheet.Cells[$"G{currentRow + 1}:H{currentRow + 1}"].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    }
    return (currentRow += 2);
  }
}
