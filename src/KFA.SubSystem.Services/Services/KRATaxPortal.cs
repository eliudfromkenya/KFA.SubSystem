using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Globals.DataLayer;
using Microsoft.Playwright;
using MySqlConnector;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KFA.SubSystem.Services.Services;

  static internal class KRATaxPortal
  {
      internal static async Task<List<(string? invoice, Dictionary<string, string?>? data,  bool? isValid)>> ValidateInvoices(params string[] invoiceNos)
      {
          if (invoiceNos?.Length == 0)
              return [(null, null, null)];

          using var playwright = await Playwright.CreateAsync();
          await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
          {
              Headless = true
          });

          List<(string? invoice, Dictionary<string, string?>? data, bool? isValid)> ans = [];
          var context = await browser.NewContextAsync(new()
          {
              //HttpCredentials = new HttpCredentials
              //{
              //    Username = username,
              //    Password = password
              //},
              ViewportSize = null
          });
          // context.SetDefaultTimeout(400000000);

          var page = await context.NewPageAsync();

          var invDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DownloadedInvoices");
          if (!Directory.Exists(invDir))
              Directory.CreateDirectory(invDir);

          int index = 1;
          foreach (var item in invoiceNos!)
          {
              var invUrl = item;
              if (string.IsNullOrWhiteSpace(invUrl)) continue;
              if (!invUrl.StartsWith("http"))
                  invUrl = $"https://itax.kra.go.ke/KRA-Portal/invoiceChk.htm?actionCode=loadPage&invoiceNo={invUrl}";

              Console.Write($@"{index++}/{invoiceNos.Length}. {invUrl}

");

              page.SetDefaultTimeout(1000);
              for (int i = 0; i < 5; i++)
              {
                  try
                  {
                      await page.GotoAsync(invUrl);
                  }
                  catch (System.TimeoutException) 
                  {
                      continue;
                  }
                  break;
              }

              ILocator? iTaxPaymentTable = null;
              List<string?>? data = null;
              try
              {
                  iTaxPaymentTable = page.Locator("body > div.templateMainDiv > div.templateContentDiv > table > tbody > tr > td > div > div:nth-child(5) > center > div > table > tbody > tr:nth-child(2) > td > fieldset > table > tbody");

                  page.SetDefaultTimeout(1000);
                  data = iTaxPaymentTable?.TextContentAsync()?.Result?.Trim()?.Split("\n")?
                          .Select(c => c?.Trim())
                          ?.Where(v => !string.IsNullOrWhiteSpace(v))
                          ?.Select(j => j)
                          ?.ToList();
              }
              catch { }            

              if(data?.Count > 0)
              {
                  try
                  {
                      var objs = Enumerable.Range(0, data?.Count ?? 0)
                          .Where(c => c % 2 == 0)
                          .ToDictionary(c => data?[c]!, d => data?[d + 1]);

                      objs["CU Invoice Number"] = item;

                      ans.Add((item, objs, true));
                      var pdfBytes = await page.PdfAsync();

                      await File.WriteAllBytesAsync(Path.Combine(invDir, $"Invoice_{item}_{(objs?.FirstOrDefault(c => c.Key== "Trader System Invoice No").Value?.ToString() ?? DateTime.Now.ToString("yyyy_MM -dd_HH_mm_ss"))}.pdf"), pdfBytes);
                  }
                  catch { }
              }
              else
              {
                  //ans.Add((item,null, false));
              }
          }
          return ans; 
      }

      internal static async Task ValidateAndSaveInvoice()
      {
          bool wasAnError = false;
          do
          {
              try
              {                  
                  var sql = @"SELECT id, cu_invoice FROM	`dynamics_tims`.tbl_tims_cache WHERE cu_invoice NOT IN (SELECT cu_invoice_number FROM dynamics_tims.tbl_kra_portal_confirmations WHERE LENGTH(cu_invoice_number) > 5); SELECT cu_invoice_number FROM dynamics_tims.tbl_kra_portal_confirmations WHERE LENGTH(cu_invoice_number) > 5";
                  using var ds = await MySQLDbService.MySQLGetDataset(sql);
                  var invNos = ds.Tables[0].AsEnumerable().Select(k => new { id = k[0]?.ToString(), cuNo = k[1]?.ToString() }).ToList();
                  var doneInvNos = ds.Tables[1].AsEnumerable()
                          .Select(k => k[0]?.ToString())
                          .Where(c => !string.IsNullOrWhiteSpace(c))
                          .Select(k => k!).ToList();

                  string[] invoices = invNos?
                   .Where(n => !string.IsNullOrWhiteSpace(n.cuNo))?
                   .Select(n => n.cuNo!)?.ToArray() ?? [];

                  var maxId = invNos?.Max(c => c.cuNo);
                  if (int.TryParse(maxId?[9..], out var maxNum))
                      invoices = Enumerable.Range(1, maxNum).Select(c => $"017049184{c:0000000000}").ToArray();

                  invoices = invoices?.Except(doneInvNos)?.ToArray() ?? [];

                  //string[] invoices = ["", "0170491840000044254", "0170491840000098660", "0170491840000044678", "https://itax.kra.go.ke/KRA-Portal/invoiceChk.htm?actionCode=loadPage&invoiceNo=0170491840000044254", "https://itax.kra.go.ke/KRA-Portal/invoiceChk.htm?actionCode=loadPage&invoiceNo=0170491840000098623"];



                  foreach (var batch in invoices.Chunk(100))
                  {
                      Console.Clear();
                      batch.Reverse();
                      var values = await ValidateInvoices(batch?.ToArray() ?? []);
                      var objs = values.Select(n => n.data).Where(c => c?.Count > 0).ToList();
                      //var  dsx = string.Join(", ", objs?.FirstOrDefault()?.Keys!);    

                      if (objs?.Count > 0)
                      {
                          int index = 0;
                          var sqlDef = new StringBuilder();
                          var pars = new List<MySqlParameter>();

                          foreach (var data in objs)
                          {
                              sqlDef.AppendLine($@"REPLACE INTO `dynamics_tims`.`tbl_kra_portal_confirmations` (`control_unit_invoice_number`, `trader_system_invoice_no`, `invoice_date`, `total_taxable_amount`, `total_tax_amount`, `total_invoice_amount`, `supplier_name`, `cu_invoice_number`, `confirmed`) VALUES (@p{index + 1},@p{index + 2},@p{index + 3},@p{index + 4},@p{index + 5},@p{index + 6},@p{index + 7},@p{index + 8},@p{index + 9});


");
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Control Unit Invoice Number"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Trader System Invoice No"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Invoice Date"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Total Taxable Amount"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Total Tax Amount"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Total Invoice Amount"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["Supplier Name"]));
                              pars.Add(new MySqlParameter($"@p{++index}", data?["CU Invoice Number"]));
                              pars.Add(new MySqlParameter($"@p{++index}", 1));
                          }
                          if (pars.Count > 0)
                              await MySQLDbService.MySQLExecuteQuery(sqlDef.ToString(), [.. pars]);
                          else Console.WriteLine("Unable to find and un-saved KRA portal invoice");
                      }
                  }
                  wasAnError = false;
              }
              catch (Exception ex)
              {
                  wasAnError = true;
                  Console.Write($@" Error. {ex.Message}

");
                  Thread.Sleep(10000);
              }

          } while (wasAnError);
      }

          //internal async Task <bool?> ValidateInvoice(string invoiceNo)
          //{
          //    using var playwright = await Playwright.CreateAsync();
          //    await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
          //    {
          //        Headless = false
          //    });

          //    var context = await browser.NewContextAsync(new()
          //    {
          //        //HttpCredentials = new HttpCredentials
          //        //{
          //        //    Username = username,
          //        //    Password = password
          //        //},
          //        ViewportSize = null
          //    });
          //    // context.SetDefaultTimeout(400000000);

          //    var page = await context.NewPageAsync();
          //    await page.GotoAsync(INVOICE_URL);

          //    var details = await page.QuerySelectorAllAsync("td a");
          //    int count = 1;
          //    var allObjs = details
          //       .Select(c => (title: c.TextContentAsync().Result, link: c.GetAttributeAsync("href")?.Result))
          //       .Where(c => c.link?.StartsWith("keyboard") ?? false)
          //       .ToList();

          //    allObjs.ForEach(cc =>
          //       {
          //           async Task tttt()
          //           {
          //               try
          //               {
          //                   var keyBoardUrl = $"https://learn.microsoft.com/en-us/globalization/{cc.link}";
          //                   var page = await context.NewPageAsync();
          //                   await page.GotoAsync(keyBoardUrl);
          //                   var mm = await page.PdfAsync();

          //                   await File.WriteAllBytesAsync($"G:\\Desktop\\Activation\\pdfs\\{cc.title}.pdf", mm);
          //                   try
          //                   {
          //                       await page.FrameLocator("iframe").GetByRole(AriaRole.Img, new() { Name = "Shift key with Shift enabled" }).First.ClickAsync();

          //                       Thread.Sleep(30);

          //                       mm = await page.PdfAsync();
          //                       await File.WriteAllBytesAsync($"G:\\Desktop\\Activation\\pdfs\\{cc.title} with shift.pdf", mm);

          //                       //var objs = await page.QuerySelectorAllAsync("p strong");
          //                       //var obj = objs.Where(c => c != null && c.TextContentAsync().Result == "Shift").LastOrDefault();

          //                       //if (obj != null) {
          //                       //    await obj!.ClickAsync();
          //                       //    Thread.Sleep(3000);

          //                       //    mm = await page.PdfAsync();
          //                       //    await File.WriteAllBytesAsync($"G:\\Desktop\\Activation\\pdfs\\{cc.title} with shift.pdf", mm);
          //                       //}
          //                   }
          //                   catch { }
          //                   Console.WriteLine($"{count++} / {allObjs.Count} - {cc.title} - {cc.link}");
          //                   await page.CloseAsync();
          //               }
          //               catch (Exception ex)
          //               {
          //                   Console.WriteLine(ex);
          //               }
          //           }
          //           tttt().Wait();
          //       });
          //}
      }
