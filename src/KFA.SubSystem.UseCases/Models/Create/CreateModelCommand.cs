using Ardalis.Result;
using KFA.SubSystem.Core;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.UseCases.Models.Create;

/// <summary>
/// Create a new CostCentre.
/// </summary>
/// <param name="Name"></param>
public record CreateModelCommand<T, X>(EndPointUser user, params T[] models) : Ardalis.SharedKernel.ICommand<Result<T?[]>> where T : BaseDTO<X>, new() where X : BaseModel, new();
