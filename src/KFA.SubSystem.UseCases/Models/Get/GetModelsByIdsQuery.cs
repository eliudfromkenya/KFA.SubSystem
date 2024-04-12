using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Get;

public record GetModelsByIdsQuery<T, X>(EndPointUser user, params string[] ids) : IQuery<Result<T[]>> where T : BaseDTO<X>, new() where X : BaseModel, new();
