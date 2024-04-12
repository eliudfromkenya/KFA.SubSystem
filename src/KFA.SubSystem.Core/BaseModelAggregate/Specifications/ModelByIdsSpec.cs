using Ardalis.Specification;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.ContributorAggregate.Specifications;

public class ModelByIdsSpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByIdsSpec(params string[] ids)
  {
    if (ids.Length > 0)
      Query
          .Where(model => ids.Contains(model.Id));
  }
}
