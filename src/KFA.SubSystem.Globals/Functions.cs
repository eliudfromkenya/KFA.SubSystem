﻿using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Ardalis.SharedKernel;
using DotEnv.Core;
using Humanizer;
using KFA.SubSystem;
using KFA.SubSystem.Core.Classes;
using KFA.SubSystem.Globals;
using KFA.SubSystem.Globals.Classes;
using KFA.SubSystem.Globals.DataLayer;
using Microsoft.Extensions.DependencyInjection;

namespace KFA.SubSystem.Globals;

public static class Functions
{
  private static readonly object _backgroundlockObject = new();

  public static DataTable SortDataTable(DataTable codes, params string[] parameters)
  {
    try
    {
      if (!parameters.Any()) return codes;
      codes.DefaultView.Sort = string.Join(", ", parameters);
      return codes.DefaultView.ToTable();
    }
    catch (Exception)
    {
      return codes;
    }
  }

  public static Func<int, int, string> AsMonthString =
 (yy, mm) => GetMonthsFormated(yy, mm);

  public static string GetEnviromentVariable(string name)
  {
    var reader = new EnvReader();
    var ans = reader[name]?.Replace("~", "#") ?? string.Empty;
    if (name == "KFASubSystemMySQLDatabase")
    {
      var objs = ans.Split(';');
      ans = $"server={objs[0]};port={objs[1]};database={objs[2]};user={objs[3]};password={objs[4]};ConvertZeroDateTime=True;AllowUserVariables=True;";
    }
    Console.WriteLine(ans);
    return ans;
  }
  public static string GetMonthsFormated(int? year, int? month)
  {
    if (year < 100)
    {
      year = year > Convert.ToInt32(DateTime.Now.Year.ToString()[2..])
          ? Convert.ToInt32(string.Format("19{0}", year))
          : Convert.ToInt32(string.Format("20{0}", year));
    }

    return string.Format("{0}-{1}", year, month?.ToString("00"));
  }

  public static int AsMonth(string month)
  {
    return Convert.ToInt32(new string(month.Where(char.IsDigit).ToArray()));
  }

  public static IRepository<X>? ResolveRepository<T, X>(BaseDTO<T> dTO) where T : BaseModel where X : BaseModel, T => null;
  public static T? ResolveObject<T>() where T : class
  {
    using var serviceScope = Declarations.GetServiceScope?.Invoke();
    return serviceScope?.ServiceProvider.GetService<T>();
  }
  public static string[] GenerateMonths(string monthFrom, string monthTo)
  {
    var noFrom = monthFrom.GetNumbers();
    var noTo = monthTo.GetNumbers();
    return GenerateMonths((byte)noFrom[1], (short)noFrom[0], (byte)noTo[1], (short)noTo[0]);
  }

  public static string[] GenerateMonths(byte monthFrom, short yearFrom, byte monthTo, short yearTo)
  {
    var startDate = new DateTime(yearFrom, monthFrom, 1);
    var endDate = new DateTime(yearTo, monthTo, 1);

    if (endDate < startDate)
    {
      (startDate, endDate) = (endDate, startDate);
    }

    var months = new List<string>();
    for (var i = 0; i < 2000; i++)
    {
      var date = startDate.AddMonths(i);
      if (date <= endDate)
        months.Add(date.ToString("yyyy-MM"));
      else
        break;
    }
    return months.ToArray();
  }

  private static IDbDataAdapter? GetAdapter(IDbConnection connection)
  {
    var assembly = connection.GetType().Assembly;
    var @namespace = connection.GetType().Namespace;

    // Assumes the factory is in the same namespace
    var factoryType = assembly.GetTypes()
                        .Where(x => x.Namespace == @namespace)
                        .Where(x => x.IsSubclassOf(typeof(DbProviderFactory)))
                        .Single();

    // SqlClientFactory and OleDbFactory both have an Instance field.
    var instanceFieldInfo = factoryType.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
    var factory = (DbProviderFactory?)instanceFieldInfo?.GetValue(null);

    return factory?.CreateDataAdapter();
  }

  public static DataSet GetDbDataSet(IDbConnection con, string sql)
  {
    using var cmd = con.CreateCommand();
    if (con.State != ConnectionState.Open)
      con.Open();

    var ds = new DataSet();
    var adapter = GetAdapter(con) ?? throw new NullReferenceException("Unable to create table adapter");

    var dbCommand = con.CreateCommand();
    dbCommand.CommandText = sql;
    dbCommand.CommandType = CommandType.Text;
    adapter.SelectCommand = dbCommand;
    adapter.Fill(ds);
    return ds;
  }

  public static DataSet GetDbDataSet(string sql, IDbConnection con)
  {
    using var cmd = con.CreateCommand();
    if (con.State != ConnectionState.Open)
      con.Open();

    var ds = new DataSet();
    var adapter = GetAdapter(con) ?? throw new NullReferenceException("Unable to create table adapter");

    var dbCommand = con.CreateCommand();
    dbCommand.CommandText = sql;
    dbCommand.CommandType = CommandType.Text;
    adapter.SelectCommand = dbCommand;
    adapter.Fill(ds);
    return ds;
  }

  public static string MakeAllFirstLetterCapital(string myStr, bool lowerOthers)
  {
    if (string.IsNullOrWhiteSpace(myStr))
      return myStr;

    short i;
    var makeFirstUCase = "";

    var strArr = myStr.Split(' ');
    for (i = 0; i <= strArr.Length - 1; i++)
      try
      {
        if (strArr[i] == string.Empty) continue;
        var others = lowerOthers && strArr.Length > 4 ? strArr[i][1..].ToLower() : strArr[i][1..];
        var ser = strArr[i][..1];
        strArr[i] = ser.ToUpper() + others;
        makeFirstUCase = makeFirstUCase + strArr[i] + " ";
      }
      catch
      {
        // ignored
      }
    return makeFirstUCase.Trim();
  }

  public static void RecordPreviousLogs((DateTime, string, object, string)[] ps)
  {
    if (!ps.Any())
      return;

    RunOnBackground(() =>
    {
      try
      {
        var dir = Path.Combine(Declarations.ApplicationDataPath, "AppLogs");
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);

        try
        {
          var files = Directory.GetFiles(dir, "*.zip");
          if (files.Length > 10)
          {
            var filesToRetain = files.OrderByDescending(v => v).Take(9);
            foreach (var fil in files)
            {
              try
              {
                if (filesToRetain.Contains(fil))
                  continue;
                File.Delete(fil);
              }
              catch { }
            }
          }
        }
        catch (Exception ex)
        {
          Notifier.NotifyError(ex);
        }

        var sb = new StringBuilder();
        ps.ToList().ForEach(tt =>
                {
                  sb.Append("******************************************************************");
                  sb.Append($"\r\n\r\nTIME: {tt.Item1:yyyy dd, MMMM => HH:mm:ss} => ");
                  sb.Append(tt.Item2);
                  sb.Append("\r\n");
                  sb.Append("___________________________________________________________________");
                  sb.Append("\r\n");
                  sb.Append(tt.Item3);
                  sb.Append("\r\n");
                  sb.Append("###################################################################\r\n");
                  sb.Append(tt.Item4);
                  sb.Append("\r\n");
                  sb.Append("___________________________________________________________________");
                  sb.Append($"\r\n\r\n\r\n\r\n");
                });
        var path = Path.Combine(dir, $"General_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.zip");
        var file = Path.Combine(dir, "data.txt");
        File.WriteAllText(file, sb.ToString());
        //ZipMaker.Create(path, new string[] { file }, "KfaLogs654321Eliud");
        //ZipMaker.Create(path, new string[] { file }, "KfaSubSystem_654321_Logs");
      }
      catch (Exception ex)
      {
        var dir = Path.Combine(Declarations.ApplicationDataPath, "AppLogs");
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "error.txt");
        File.WriteAllText(file, ex.ToString());
      }
    });
  }

  public static string? ReadManifestData<T>(string fileName) where T : class
  {
    try
    {
      var assembly = typeof(T).GetTypeInfo().Assembly;

      var resourceName = assembly.GetManifestResourceNames()
          .FirstOrDefault(s => s.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

      using var stream = assembly.GetManifestResourceStream(resourceName ?? "") ?? throw new InvalidOperationException("Could not load the specified resource " + fileName);

      using var reader = new StreamReader(stream);
      return reader.ReadToEnd();
    }
    catch (Exception ex)
    {
      Notifier.NotifyError(ex);
      return null;
    }
  }

  public static string OperatingSystem => System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;

  public static Exception? ExtractInnerException(Exception? exception)
  {
    try
    {
      if (exception == null)
        return exception;

      static Exception getInnerError(Exception ex)
      {
        // if (ex is DbEntityValidationException objEx)
        // {
        //    var mm = string.Join("\r\n\r\n", objEx.EntityValidationErrors
        //        .Select(x => string.Join("\r\n\r\n",
        // x.ValidationErrors.Select(y => y.PropertyName + ": " + y.ErrorMessage))));

        //    return new ValidationException(mm);
        // }
        // else
        if (ex.InnerException != null)
        {
          return ex.InnerException;
        }

        return ex;
      }

      var mEx = exception;

      while (true)
      {
        var mm = getInnerError(mEx);
        if (mm == mEx) break;
        mEx = mm;
      }

      return mEx;
    }
    catch (Exception)
    {
      throw;
    }
  }

  public static bool IsValidDate(out DateTime? date, object year, object month, object day)
  {
    date = null;
    try
    {
      var xYear = Convert.ToInt32(year);
      var xMonth = Convert.ToInt32(month);
      var xDay = Convert.ToInt32(day);

      static string getYear(string ans)
      {
        if (short.TryParse(ans, out var oo))
        {
          if (100 > oo)
          {
            var mm = Convert.ToInt32((DateTime.Now.Year + 3).ToString()[2..]);
            return oo < mm ? $"20{oo}" : $"19{oo}";
          }
          else
          {
            var mm = Convert.ToInt32($"{ans}0000"[..4]);
            return mm.ToString("0000");
          }
        }
        return ans;
      }

      try
      {
        date = new DateTime(xYear, xMonth, xDay);
        return true;
      }
      catch { }
      try
      {
        var text = xDay.ToString();
        if (text.Length == 2 && xDay > 28)
        {
          date = new DateTime(xYear, Convert.ToInt32(text[1]), Convert.ToInt32(text[0]));
          return true;
        }
        else if (text.Length == 3)
        {
          try
          {
            date = new DateTime(xYear, Convert.ToInt32(text[2]), Convert.ToInt32(text[..2]));
            return true;
          }
          catch { }
          try
          {
            date = new DateTime(xYear, Convert.ToInt32(text.Substring(1, 2)), Convert.ToInt32(text[0]));
            return true;
          }
          catch { }
        }
        if (byte.TryParse(text[..2], out var mn) && mn > 0 && mn < 32)
        {
          if (byte.TryParse(text.AsSpan(2, 2), out var kn) && kn > 0 && kn < 13)
          {
            date = new DateTime(Convert.ToInt32(getYear(text[4..])),
                Convert.ToInt32(text.Substring(2, 2)), Convert.ToInt32(text[..2]));
            return true;
          }
          else if (byte.TryParse(text.AsSpan(2, 1), out var kn1) && kn1 > 0 && kn1 < 13)
          {
            date = new DateTime(Convert.ToInt32(getYear(text[3..])), kn1,
                Convert.ToInt32(text[..2]));
            return true;
          }
        }
        else if (byte.TryParse(text[..1], out var mn1) && mn1 > 0 && mn1 < 32)
        {
          if (byte.TryParse(text.AsSpan(1, 2), out var kn) && kn > 0 && kn < 13)
          {
            date = new DateTime(Convert.ToInt32(getYear(text[3..])), Convert.ToInt32(text.Substring(1, 2)), Convert.ToInt32(mn1));
            return true;
          }
          else if (byte.TryParse(text.AsSpan(1, 1), out var kn1) && kn1 > 0 && kn1 < 13)
          {
            date = new DateTime(Convert.ToInt32(getYear(text[2..])), kn1, mn1);
            return true;
          }
        }
      }
      catch { }
      return false;
    }
    catch (Exception)
    {
      return false;
    }
  }

  public static string GetMonthFormated(int? year, int? month)
  {
    if (year < 100)
    {
      year = year > Convert.ToInt32(DateTime.Now.Year.ToString()[2..])
          ? Convert.ToInt32(string.Format("19{0}", ((int)year).ToString("00")))
          : Convert.ToInt32(string.Format("20{0}", ((int)year).ToString("00")));
    }
    return string.Format("{0}-{1}", year, month?.ToString("00"));
  }

  public static string StrimLineObjectName(string name)
  {
    var _name = "";

    return name.Where(chr => char.IsLetterOrDigit(chr) || chr == ' ' || chr == '_')
        .Aggregate(_name, (current, chr) => current + chr);
  }

  public static void RunOnBackground(Action action)
  {
    try
    {
      RunOnBackground(out var worker, action);
    }
    catch (Exception ex)
    {
      Notifier.NotifyError(ex.InnerError()?.Message, "Unhandled Background Error", ex);
    }
  }

  public static void RunOnBackground(out BackgroundWorker? worker, Action action)
  {
    var helper = new BackgroundWorkHelper();
    worker = helper.BackgroundWorker;

    lock (_backgroundlockObject)
    {
      try
      {
        var action1 = action;

        action = () =>
        {
          try
          {
            action1();
          }
          catch (Exception ex)
          {
            Notifier.NotifyError(ex.Message, "Unhandled Background Error", ex);
          }
        };

        var actions = new List<Action> { action };
        helper.SetActionsTodo(actions);
        helper.IsParallel = true;

        if (helper.BackgroundWorker.IsBusy)
          helper.SetActionsTodo(actions);
        else
          helper.BackgroundWorker.RunWorkerAsync();
      }
      catch (Exception e)
      {
        Notifier.NotifyError(e.Message, "Unhandled Background Error", e);
        if (worker != null)
          worker?.Dispose();
      }
    }
  }

  internal static string GetSupplierCodeSuffix(string supplierCode)
  {
    if (string.IsNullOrWhiteSpace(supplierCode))
      return supplierCode;

    if (supplierCode?.Length <= 6 && supplierCode?.Length > 3)
    {
      return supplierCode[3..];
    }

    return "";
  }

  public static void RunOnBackground(Action action, int sleepTime)
  {
    RunOnBackground(() =>
    {
      try
      {
        Thread.Sleep(sleepTime);
        action();
      }
      catch (Exception ex)
      {
        Notifier.NotifyError(ex.InnerError()?.Message, "Unhandled Background Error", ex);
      }
    });
  }

  public static string GetRandomString(int length, int trial = 0)
  {
    var rnd = new Random();
    var sb = new StringBuilder();

    for (var i = 0; i < length; i++)
    {
      var nn = rnd.Next(0, 35);

      if (nn < 10)
        sb.Append(nn);
      else
      {
        const int CON = (byte)'A' - 10;
        sb.Append((char)(CON + nn));
      }
    }

    var ans = sb.ToString();

    if (trial++ < 10 && (ans.Contains('I') || ans.Contains('O')))
    {
      return GetRandomString(length, trial);
    }

    return ans;
  }

  public static string Singularize(string name) => name.Singularize();
}
