﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.ContributorAggregate.Specifications;
using KFA.SubSystem.Globals;
using KFA.SubSystem.UseCases.Models.Get;

namespace KFA.SubSystem.UseCases.Xs.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetModelHandler<T, X>(IReadRepository<X> _repository)
  : IQueryHandler<GetModelQuery<T, X>, Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<T>> Handle(GetModelQuery<T, X> request, CancellationToken cancellationToken)
  {
    var spec = new ModelByIdSpec<X>(request.id);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null)
    {
      return Result.NotFound("Element was not found");
    }

    if (entity.ToBaseDTO() is T obj)
      return obj;
    return Result.Error("Unable to convert the result");
  }
}
