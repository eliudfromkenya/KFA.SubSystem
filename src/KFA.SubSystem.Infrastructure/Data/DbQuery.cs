using KFA.SubSystem.Core.Services;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Infrastructure.Data;

internal class DbQuery<T>(AppDbContext dbContext) : IDbQuery<T> where T : BaseModel, new()
{
  public IQueryable<T> GetQuery()
  {
    return dbContext.Set<T>().AsQueryable();
  }
}
