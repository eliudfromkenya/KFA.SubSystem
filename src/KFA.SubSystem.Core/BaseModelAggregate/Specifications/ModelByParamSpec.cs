using Ardalis.Specification;
using KFA.SubSystem.Globals;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;

namespace KFA.SubSystem.Core.ContributorAggregate.Specifications;

public class ModelByParamSpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByParamSpec(ListParam param)
  {
    Query.Skip(param.Skip ?? 0).Take(param.Take ?? 1000);
  }
}
