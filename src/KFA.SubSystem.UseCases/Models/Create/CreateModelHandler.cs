﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.Interfaces;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Create;

public class CreateModelHandler<T, X>(IInsertModelService<X> _addService)
  : ICommandHandler<CreateModelCommand<T, X>, Result<T?[]>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<T?[]>> Handle(CreateModelCommand<T, X> request,
    CancellationToken cancellationToken)
  {
    X[] objs = request?.models?
      .Select(c => c.ToModel())?
      .Where(m => m != null)
      .Select(n => n!)
      .ToArray() ?? [];

    for (int i = 0; i < objs.Length; i++)
    {
      var obj = objs[i];
      obj.___DateInserted___ = DateTime.Now.FromDateTime();
      obj.___DateUpdated___ = DateTime.Now.FromDateTime();
      if (string.IsNullOrWhiteSpace(obj.Id))
        objs[i] = obj with { Id = Declarations.IdGenerator?.GetNextId<X>() };
    }
    var createdItem = await _addService.InsertModel(request?.user, cancellationToken, objs);
    return createdItem.Value?.Select(c => (T)c.ToBaseDTO())?.ToArray() ?? [];
  }
}
