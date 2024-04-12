using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Get;

public record GetModelQuery<T, X>(EndPointUser user, string id) : IQuery<Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new();
