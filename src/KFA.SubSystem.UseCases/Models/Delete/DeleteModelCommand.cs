using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Delete;

public record DeleteModelCommand<T>(EndPointUser user, params string[] id) : ICommand<Result> where T : BaseModel, new();
