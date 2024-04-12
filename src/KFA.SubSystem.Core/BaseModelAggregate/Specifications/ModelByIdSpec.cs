using Ardalis.Specification;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.ContributorAggregate.Specifications;

public class ModelByIdSpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByIdSpec(string id)
  {
    Query
        .Where(model => model.Id == id);
  }
}
