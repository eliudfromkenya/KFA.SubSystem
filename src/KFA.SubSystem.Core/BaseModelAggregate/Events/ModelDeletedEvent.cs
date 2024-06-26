﻿using Ardalis.SharedKernel;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.BaseModelAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a model is deleted.
/// The DeleteModelService is used to dispatch this event.
/// </summary>
internal sealed class ModelDeletedEvent<T>(params T[] ids) : DomainEventBase where T : BaseModel
{
  public T[] Ids { get; init; } = ids;
}
