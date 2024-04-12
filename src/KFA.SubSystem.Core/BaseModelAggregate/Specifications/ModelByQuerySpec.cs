using Ardalis.Specification;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.ContributorAggregate.Specifications;

public class ModelByQuerySpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByQuerySpec(Func<T, bool> func)
  {
    Query.Where(c => func(c));
  }
}
