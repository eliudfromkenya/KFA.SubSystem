using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Services;

public interface IDbQuery<T> where T : BaseModel, new()
{
  IQueryable<T> GetQuery();
}
