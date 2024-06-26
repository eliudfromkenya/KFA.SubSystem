﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.BaseModelAggregate.Events;
using KFA.SubSystem.Globals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KFA.SubSystem.Core.Interfaces;

public class UpdateModelService<T>(IRepository<T> _repository,
  IMediator _mediator,
  ILogger<UpdateModelService<T>> _logger) : IUpdateModelService<T> where T : BaseModel
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public async Task<Result<T>> UpdateModel(EndPointUser? user, string id, T model, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Updating model {type} - {id}", typeof(T), id);
    if (model == null)
      return Result.Error("No element to update are provided");

   // model.Id = id;
    await _repository.UpdateAsync(model!, cancellationToken);
    var domainEvent = new ModelUpdatedEvent<T>(id, model);
    await _mediator.Publish(domainEvent, cancellationToken);
    return Result.Success(model);
  }
}
