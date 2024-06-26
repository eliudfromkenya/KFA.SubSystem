﻿using Ardalis.Result;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Interfaces;

public interface IUpdateModelService<T> where T : BaseModel
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public Task<Result<T>> UpdateModel(EndPointUser? user, string id, T model, CancellationToken cancellationToken);
}
