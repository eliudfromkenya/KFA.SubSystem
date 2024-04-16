using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Services.Models;

namespace KFA.SubSystem.Services;

internal static class ServiceFunctions
{

  public static byte[]? GetExcelFile(DataTable? table, bool isGeneralLedger = true, bool useMainBranch = true)
  {
    return null;
  }

  public static void OpenExcelFile(byte[] bytes)
  {
    try
    {
      var path = Path.GetTempPath();
      var fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), "xlsx");
      path = Path.Combine(path, fileName);
      
      File.WriteAllBytes(path, bytes);

      ProcessStartInfo psi = new ProcessStartInfo
      {
        FileName = path,
        UseShellExecute = true
      };
      Process.Start(psi);
    }
    catch (Exception ex)
    {
      Declarations.DbLogger?.Error(ex.ToString());
    }
  }

  public static List<LedgerRecord>? GetLedgerRecords(DataTable? table, bool isGeneralLedger = true, bool useMainBranch = true)
  {
    return table?.AsEnumerable().Select(row =>
    {
      var code = row[isGeneralLedger ? "G/L Account No." : "Bank Account No."].ToString();
      var branch = string.Empty;
      var xx = code?.Split("-");

      if (xx?.Length > 1)
      {
        code = xx?.LastOrDefault();
        branch = xx?.FirstOrDefault();
      }
      else
      {
        var balAccounts = row["Bal. Account No."].ToString()?.Split('-');
        if (balAccounts?.Length > 1)
        {
          branch = balAccounts?.FirstOrDefault();
        }
        else if (isGeneralLedger)
        {
          balAccounts = row["External Document No."].ToString()?.Split('-');
          if (balAccounts?.Length < 3)
            balAccounts = row["External Document No."].ToString()?.Split('/');
          if (balAccounts?.Length < 3)
            balAccounts = row["External Document No."].ToString()?.Split('\\');
          if (balAccounts?.Length > 2)
            branch = balAccounts[1];
        }
      }

      var postingDate = (DateTime?)row["Posting Date"];
      var balAccount = xx?.Length > 1 ? row["Bal. Account No."].ToString() : row["Bal. Account No."].ToString()?.Split('-')?.Last();

      if (xx?.Length > 1 && (balAccount?.Contains("-") ?? false))
      {
        var yy = balAccount.Split('-');
        if (yy[0] == xx[0])
          balAccount = yy[1];
      }

      if (useMainBranch == true)
      {
        try
        {
          if (!string.IsNullOrWhiteSpace(row["Branch Code"].ToString()))
          {
            branch = row["Branch Code"].ToString();
          }
          else if (!string.IsNullOrWhiteSpace(row["Dept Code"].ToString()))
          {
            branch = row["Dept Code"].ToString();
          }
        }
        catch { }
      }
      var rev = row["Reversed"].ToString();

      bool? isReversed = null;
      if (rev == "0")
        isReversed = false;
      else if (!string.IsNullOrWhiteSpace(rev) && rev != "0")
        isReversed = true;

      return new LedgerRecord(
      isReversed,
      postingDate,
      row["Document Type"].ToString(),
      row["Document No."].ToString(),
      code,
      row["Description"].ToString(),
      decimal.TryParse(row["Amount"].ToString(), out decimal amt) ? amt : 0,
      row[isGeneralLedger ? "Gen. Posting Type" : "Bal. Account Type"].ToString(),
      balAccount,
      row[isGeneralLedger ? "Gen. Bus. Posting Group" : "Bal. Account Name"].ToString(),
      row["External Document No."].ToString(),
      row["Entry No."].ToString(),
      branch,
      branch, false, GetMonth(postingDate)
  //bool.TryParse(row["Reversed"].ToString(), out bool reversed) && reversed
  );
    }).ToList();
  }

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
}
