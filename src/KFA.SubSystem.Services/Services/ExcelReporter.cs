using Microsoft.Playwright;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace KFA.SubSystem.Services.Services;

internal static class ExcelReporter
{
  public static (string monthName, string monthCode) GetMonth(DateTime? postingDate)
  {
    try
    {
      return postingDate == null ? ((string monthName, string monthCode))(string.Empty, string.Empty) : ((string monthName, string monthCode))($"{postingDate.Value:MMMM yyyy}", $"{postingDate.Value:yyyy-MM}");
    }
    catch
    {
      return ("", "");
    }
  }

  public static string GetHomePath()
  {
     if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
      return Environment.GetEnvironmentVariable("HOME")!;

    return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
  }

  public static ExcelWorksheet CreateWorkSheet(ref ExcelPackage package, string header, string title, int cols = 10)
  {
    char endCol = (char)('A' + cols - 1);

    var worksheet = package.Workbook.Worksheets.Add(header);
    worksheet.Cells[$"A1:{endCol}1"].Merge = true;
    worksheet.Cells["A1"].Value = "KFA DYNAMICS VAT REPORT";
    worksheet.Row(1).Height = 30;
    worksheet.Row(1).Style.Font.Size = 24;
    worksheet.Row(1).Style.Font.Color.SetColor(Color.Purple);
    worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    worksheet.Row(1).Style.Font.Bold = true;
    worksheet.TabColor = Color.LightBlue;
    worksheet.DefaultRowHeight = 12;

    var currentRow = 2;
    var range = worksheet.Cells[$"A2:{endCol}2"];
    range.Merge = true;
    range.Value = title;
    worksheet.Row(currentRow).Height = 20;
    worksheet.Row(currentRow).Style.Font.Size = 18;
    worksheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkGray);
    worksheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    worksheet.Row(currentRow).Style.Font.Bold = true;

    currentRow = 3;
    range = worksheet.Cells[$"A3:{endCol}3"];
    range.Merge = true;
    range.Value = header;
    worksheet.Row(currentRow).Height = 20;
    worksheet.Row(currentRow).Style.Font.Size = 18;
    worksheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkBlue);
    worksheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    worksheet.Row(currentRow).Style.Font.Bold = true;

    return worksheet;
  }

  public static string GetDownloadFolderPath()
  {
    //if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
    //{
    //    string pathDownload = System.IO.Path.Combine(GetHomePath(), "Downloads");
    //    return pathDownload;
    //}

    //return Convert.ToString(
    //    Microsoft.Win32.Registry.GetValue(
    //         @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
    //        , "{374DE290-123F-4565-9164-39C4925E467B}"
    //        , string.Empty
    //    )
    //) ?? string.Empty;

    string pathDownload = System.IO.Path.Combine(GetHomePath(), "Downloads");
    return pathDownload;
  }

  public static string GetLastDownloadFile()
  {
    var dir = GetDownloadFolderPath() ?? throw new Exception("Can't locat downloads folder");
    var file = new DirectoryInfo(dir).EnumerateFiles("Posted Sales Invoices*.xlsx").OrderBy(c => c.LastWriteTime).LastOrDefault();
    return file?.FullName ?? string.Empty;
  }

  public static void GenerateVATDetailsReport(ref ExcelPackage package, string fileName)
  {
    List<string> errors = [];
    using var ds = MsExcel.ExcelToDataTable(fileName, ref errors);

    var data = ds.Tables[0].AsEnumerable().Select(row => (date: DateTime.TryParse(row["Posting Date"].ToString(), out DateTime date) ? date : new DateTime(1, 1, 1), receiptNo: row["No."].ToString(), branchCode: row["Branch Code"].ToString(), amount: decimal.TryParse(row["Amount"].ToString(), out decimal amt) ? amt : 0, customerNo: row["Customer No."].ToString(), customer: row["Customer"].ToString(), vatWithAmount: decimal.TryParse(row["Amount Including VAT"].ToString(), out decimal vatAmount) ? vatAmount : 0)).ToArray();

    var dates = string.Join(",", data.Select(c => GetMonth(c.date).monthName).Distinct());

    var worksheet = CreateWorkSheet(ref package, "POSTED SALES DETAILS WITH VAT", dates, 9);
    var currentRow = 2;

    currentRow += 1;
    var range = worksheet.Cells[$"A{currentRow}:Q{currentRow}"];
    //range.Merge = true;
    //range.Value = $"{recs.Key} - {branches.FirstOrDefault(x => x.Key == recs.Key).Value}";
    worksheet.Row(currentRow).Height = 20;
    worksheet.Row(currentRow).Style.Font.Size = 18;
    worksheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkGray);
    worksheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    worksheet.Row(currentRow).Style.Font.Bold = true;
    currentRow += 1;
    range = worksheet.Cells[$"A{currentRow}:Q{currentRow}"];
    range.Style.Font.Color.SetColor(Color.MediumPurple);
    range.Style.Font.UnderLine = true;
    range.Style.Font.Size = 11;

    worksheet.Cells[currentRow, 1, currentRow, 1].Value = "Cash Sale No.";
    worksheet.Cells[currentRow, 2, currentRow, 2].Value = "Branch Code";
    worksheet.Cells[currentRow, 3, currentRow, 3].Value = "Branch Name";
    worksheet.Cells[currentRow, 4, currentRow, 4].Value = "Posting Date";
    worksheet.Cells[currentRow, 5, currentRow, 5].Value = "Customer No.";
    worksheet.Cells[currentRow, 6, currentRow, 6].Value = "Customer Name";
    worksheet.Cells[currentRow, 7, currentRow, 7].Value = "Amount without VAT";
    worksheet.Cells[currentRow, 8, currentRow, 8].Value = "Amount with VAT";
    worksheet.Cells[currentRow, 9, currentRow, 9].Value = "VAT Amount";
    //worksheet.Cells[currentRow, 10, currentRow, 10].Value = "Id";

    int rowStart = currentRow + 1;

    var branches = Data.Branches.Split("\n")
            .Select(x => x.Split(','))
            .Where(c => c?.Length == 2)
            .ToDictionary(c => c.First().Trim(), v => v.Last()?.Trim());

    data.Where(n => n.vatWithAmount != n.amount)
      //.Where(v => v.ProcessedBy == "Personnel")
      //.GroupBy(c => c.branchCode)
      .OrderBy(c => c.date)
     .ToList().ForEach(code =>
     {
       currentRow += 1;
       var ledgerName = $"{branches.FirstOrDefault(x => x.Key == code.branchCode).Value}";

       worksheet.Cells[currentRow, 1, currentRow, 1].Value = code.receiptNo;
       worksheet.Cells[currentRow, 2, currentRow, 2].Value = code.branchCode;
       worksheet.Cells[currentRow, 3, currentRow, 3].Value = ledgerName;
       worksheet.Cells[currentRow, 4, currentRow, 4].Value = code.date;
       worksheet.Cells[currentRow, 5, currentRow, 5].Value = code.customerNo;
       worksheet.Cells[currentRow, 6, currentRow, 6].Value = code.customer;
       worksheet.Cells[currentRow, 7, currentRow, 7].Value = code.amount;
       worksheet.Cells[currentRow, 8, currentRow, 8].Value = code.vatWithAmount;
       worksheet.Cells[currentRow, 9, currentRow, 9].Formula = $"H{currentRow}-G{currentRow}";
     });

    worksheet.Cells[$"G{currentRow + 1}:G{currentRow + 1}"].Formula = $"SUM(G{rowStart}:G{currentRow})";
    worksheet.Cells[$"H{currentRow + 1}:H{currentRow + 1}"].Formula = $"SUM(H{rowStart}:H{currentRow})";
    worksheet.Cells[$"I{currentRow + 1}:I{currentRow + 1}"].Formula = $"SUM(I{rowStart}:I{currentRow})";

    worksheet.Cells[$"D{rowStart}:D{currentRow + 2}"].Style.Numberformat.Format = "dd MMM, yyyy";
    worksheet.Cells[$"G{rowStart}:I{currentRow + 2}"].Style.Numberformat.Format = "#,##0.00";
    var rng = worksheet.Cells[$"G{currentRow + 1}:I{currentRow + 1}"];
    rng.Style.Fill.PatternType = ExcelFillStyle.LightUp;
    rng.Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    rng.Style.Font.Size = 14;
    rng.Style.Font.Bold = true;
    rng.Style.Font.UnderLine = true;
    rng.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;

    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(); worksheet.View.FreezePanes(5, 1);
    worksheet.Column(5).Width = worksheet.Column(5).Width + 5;
    worksheet.Column(4).Width = worksheet.Column(4).Width + 5;
  }

  public static void GenerateVATSummaryReport(ref ExcelPackage package, string fileName)
  {
    System.Collections.Generic.List<string> errors = [];
    using var ds = MsExcel.ExcelToDataTable(fileName, ref errors);

    var data = ds.Tables[0].AsEnumerable().Select(row => (date: DateTime.TryParse(row["Posting Date"].ToString(), out DateTime date) ? date : new DateTime(1, 1, 1), receiptNo: row["No."].ToString(), branchCode: row["Branch Code"].ToString(), amount: decimal.TryParse(row["Amount"].ToString(), out decimal amt) ? amt : 0, customerNo: row["Customer No."].ToString(), customer: row["Customer"].ToString(), vatWithAmount: decimal.TryParse(row["Amount Including VAT"].ToString(), out decimal vatAmount) ? vatAmount : 0)).ToArray();

    var dates = string.Join(",", data.Select(c => GetMonth(c.date).monthName).Distinct());

    var worksheet = CreateWorkSheet(ref package, "POSTED SALES SUMMARY BY BRANCH WITH VAT", dates, 6);
    var currentRow = 2;

    currentRow += 1;
    var range = worksheet.Cells[$"A{currentRow}:F{currentRow}"];
    //range.Merge = true;
    //range.Value = $"{recs.Key} - {branches.FirstOrDefault(x => x.Key == recs.Key).Value}";
    worksheet.Row(currentRow).Height = 20;
    worksheet.Row(currentRow).Style.Font.Size = 18;
    worksheet.Row(currentRow).Style.Font.Color.SetColor(Color.DarkGray);
    worksheet.Row(currentRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    worksheet.Row(currentRow).Style.Font.Bold = true;
    currentRow += 1;
    range = worksheet.Cells[$"A{currentRow}:F{currentRow}"];
    range.Style.Font.Color.SetColor(Color.MediumPurple);
    range.Style.Font.UnderLine = true;
    range.Style.Font.Size = 11;

    worksheet.Cells[currentRow, 1, currentRow, 1].Value = "Code";
    worksheet.Cells[currentRow, 2, currentRow, 2].Value = "Name";
    worksheet.Cells[currentRow, 3, currentRow, 3].Value = "Recs";
    worksheet.Cells[currentRow, 4, currentRow, 4].Value = "Amount without VAT";
    worksheet.Cells[currentRow, 5, currentRow, 5].Value = "Amount with VAT";
    worksheet.Cells[currentRow, 6, currentRow, 6].Value = "VAT Amount";
    //worksheet.Cells[currentRow, 10, currentRow, 10].Value = "Id";

    int rowStart = currentRow + 1;

    var branches = Data.Branches.Split("\n")
            .Select(x => x.Split(','))
            .Where(c => c?.Length == 2)
            .ToDictionary(c => c.First().Trim(), v => v.Last()?.Trim());

    data//.Where(n => n.IsReversed != true)
        //.Where(v => v.ProcessedBy == "Personnel")
      .GroupBy(c => c.branchCode)
      .OrderBy(c => c.Key)
     .ToList().ForEach(code =>
     {
       currentRow += 1;
       var ledgerName = $"{branches.FirstOrDefault(x => x.Key == code.Key).Value}";

       worksheet.Cells[currentRow, 1, currentRow, 1].Value = code.Key;
       worksheet.Cells[currentRow, 2, currentRow, 2].Value = ledgerName;
       worksheet.Cells[currentRow, 3, currentRow, 3].Value = code.Count();
       worksheet.Cells[currentRow, 4, currentRow, 4].Value = code.Sum(c => c.amount);
       worksheet.Cells[currentRow, 5, currentRow, 5].Value = code.Sum(c => c.vatWithAmount);
       worksheet.Cells[currentRow, 6, currentRow, 6].Formula = $"E{currentRow}-D{currentRow}";
     });

    worksheet.Cells[$"F{currentRow + 1}:F{currentRow + 1}"].Formula = $"SUM(F{rowStart}:F{currentRow})";
    worksheet.Cells[$"D{currentRow + 1}:D{currentRow + 1}"].Formula = $"SUM(D{rowStart}:D{currentRow})";
    worksheet.Cells[$"E{currentRow + 1}:E{currentRow + 1}"].Formula = $"SUM(E{rowStart}:E{currentRow})";

    worksheet.Cells[$"D{rowStart}:F{currentRow + 2}"].Style.Numberformat.Format = "#,##0.00";
    var rng = worksheet.Cells[$"D{currentRow + 1}:F{currentRow + 1}"];
    rng.Style.Fill.PatternType = ExcelFillStyle.LightUp;
    rng.Style.Fill.BackgroundColor.SetColor(Color.LightPink);
    rng.Style.Font.Size = 14;
    rng.Style.Font.Bold = true;
    rng.Style.Font.UnderLine = true;
    rng.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;

    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(); worksheet.View.FreezePanes(5, 1);
    worksheet.Column(5).Width = worksheet.Column(5).Width + 5;
    worksheet.Column(4).Width = worksheet.Column(4).Width + 5;
  }

  internal static byte[] ExportToExcel(DataSet ds, DateOnly from, DateOnly to)
  {
    // Creating an instance
    // of ExcelPackage
    ExcelPackage excel = new ExcelPackage();

    foreach (DataTable table in ds.Tables)
    {
      var cols = table.Columns.OfType<DataColumn>().ToList();
      var sheet = CreateWorkSheet(ref excel, table.TableName, $"Period: {from:dd-MM-yyyy} to {to:dd-MM-yyyy}", cols.Count);

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

    var bytes = excel.GetAsByteArray();
    ServiceFunctions.OpenExcelFile(bytes);
    return bytes;
  }
}
