using KFA.SubSystem.Globals.DataLayer;
using Microsoft.Extensions.Primitives;
using Microsoft.Playwright;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KFA.SubSystem.Services.Services;

internal static class TimsAPI
{
  public static async Task GetCurrentCalls()
  {
    try
    {

      using var playwright = await Playwright.CreateAsync();
      await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
      {
        Headless = true,
      });
      var context = await browser.NewContextAsync(new BrowserNewContextOptions
      {
        ViewportSize = new ViewportSize
        {
          Height = 600,
          Width = 800,
        },
      });

      var page = await context.NewPageAsync();

      await page.GotoAsync("http://kfasubserver:8080/MaliPlus_TIMS_API/");

      var headers = await page.QuerySelectorAllAsync("table thead tr th");
      var texts = headers.Select(v => v.TextContentAsync().Result).ToList();
      var obj = page.Locator("#btn_invoices > svg");

      string? text;
      if (obj != null)
        await obj.ClickAsync();
      obj = page.Locator("#invoice_content");
      if (obj != null)
        text = await obj.TextContentAsync();

      int currentIndex = 1, index = 0;
      var sqlDef = new StringBuilder();
      var pars = new List<MySqlParameter>();

      //using var con = new MySqlConnector.MySqlConnection(sql);
      // con.Open();

      _ = long.TryParse((await MySQLDbService.MySQLGetScalar("SELECT MAX(id) num FROM `dynamics_tims`.tbl_tims_cache"))?.ToString(), out long maxValId);
      do
      {
        try
        {
          obj = page.Locator($"#invoice_content > tr:nth-child({currentIndex++})");
          if (obj != null)
          {
            var data = obj?.TextContentAsync()?.Result?.Trim()?.Split("\n")?
                    .Select(c => c?.Trim())
                    ?.Select((v, j) => (texts[j], v))
                    .ToList();

            if (data?.Count <= 0)
              break;

            if (long.TryParse(data?.FirstOrDefault().v ?? "0", out long currentId) && currentId <= maxValId)
              continue;

            sqlDef.AppendLine($@"REPLACE INTO `dynamics_tims`.`tbl_tims_cache` (`id`, `device`, `invoice`, `trans_type`, `system_invoice`, `cu_invoice`, `qr_code`, `cu_serial_number`, `pin_number`, `transaction_date`) VALUES (@p{index + 1},@p{index + 2},@p{index + 3},@p{index + 4},@p{index + 5},@p{index + 6},@p{index + 7},@p{index + 8},@p{index + 9},@p{index + 10});


");
            pars.AddRange(Enumerable.Range(index + 1, 10).Select(c => new MySqlParameter($"@p{c}", data?[c - index - 1].v)));
            index += 10;
          }
        }
        catch (Exception)
        {
          obj = null;
        }
      } while (obj != null);
      if (pars.Count > 0)
        await MySQLDbService.MySQLExecuteQuery(sqlDef.ToString(), [.. pars]);
      else Console.WriteLine("Unable to find and un-saved invoice");
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString());
    }
  }
}
