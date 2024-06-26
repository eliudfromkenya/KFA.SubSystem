﻿using Ardalis.SharedKernel;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.BaseModelAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a model is deleted.
/// The DeleteModelService is used to dispatch this event.
/// </summary>
internal sealed class ModelInsertedEvent<T>(params T[] models) : DomainEventBase where T : BaseModel
{
  public T[] Models { get; init; } = models;
}
