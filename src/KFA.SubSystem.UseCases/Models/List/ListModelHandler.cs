using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.ContributorAggregate.Specifications;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.List;

public class ListModelsHandler<T, X>(IReadRepository<X> _repository)
  : IQueryHandler<ListModelsQuery<T, X>, Result<List<T>>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<List<T>>> Handle(ListModelsQuery<T, X> request, CancellationToken cancellationToken)
  {
    var result = await _repository.ListAsync(new ModelByParamSpec<X>(request.param));

    return Result.Success(result?.Select(c => (T)c.ToBaseDTO())?.ToList() ?? []);
  }
}
