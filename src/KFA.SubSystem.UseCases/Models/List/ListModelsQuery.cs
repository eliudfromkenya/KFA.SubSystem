using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;
using KFA.SubSystem.UseCases.ModelCommandsAndQueries;

namespace KFA.SubSystem.UseCases.Models.List;

public record ListModelsQuery<T, X>(EndPointUser user, ListParam param) : IQuery<Result<List<T>>> where T : BaseDTO<X>, new() where X : BaseModel, new();
