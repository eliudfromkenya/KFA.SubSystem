using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem.Globals.DataLayer;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace KFA.SubSystem;
public static class Declarations
{
  public static Func<IServiceScope?>? GetServiceScope { get; set; }
  public static IIdGenerator? IdGenerator { get; set; }
  public static IServiceCollection? DIServices { get; set; }
  public static string ApplicationDataPath { get; internal set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DynamicAssistantHelper");
  public static IGeneralService? GeneralService { get; set; }
  public static string DeviceNumber { get; set; } = "SVR";
  public static Logger? DbLogger { get; set; }
  public static Logger? PermanentTableDbLogger { get; set; }
}
