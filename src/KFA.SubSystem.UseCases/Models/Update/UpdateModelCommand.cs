using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Update;

public record UpdateModelCommand<T, X>(EndPointUser user, string id, T? model) : ICommand<Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new();
