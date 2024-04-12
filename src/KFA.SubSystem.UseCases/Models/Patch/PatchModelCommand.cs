using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Patch;

public record PatchModelCommand<T, X>(EndPointUser user, string id, Func<T,T> applyChanges) : ICommand<Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new();
