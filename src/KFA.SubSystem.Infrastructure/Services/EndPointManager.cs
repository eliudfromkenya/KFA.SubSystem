using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Core.Classes;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Core.Services;
using KFA.SubSystem.Globals.DataLayer;

namespace KFA.SubSystem.Infrastructure.Services;
internal class EndPointManager(IRepository<DefaultAccessRight> repo)
: IEndPointManager
{
  public string[] GetDefaultAccessRights(string name, string type)
  {
    return CoreFunctions.GetDefaultAccessRights(repo, name, type) ?? [];
  }

  public string[] GetDefaultAccessRights(string rightId)
  {
    return CoreFunctions.GetDefaultAccessRights(repo, rightId) ?? [];
  }
}
