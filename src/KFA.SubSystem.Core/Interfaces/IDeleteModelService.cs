﻿using Ardalis.Result;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Interfaces;

public interface IDeleteModelService<T> where T : BaseModel, new()
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public Task<Result> DeleteModel(EndPointUser? user, CancellationToken cancellationToken, params string[] ids);
}
