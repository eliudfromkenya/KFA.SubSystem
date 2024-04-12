using Ardalis.Result;
using KFA.SubSystem.Core.DTOs;

namespace KFA.SubSystem.UseCases.Users;
public record UserAddRightsCommand(string userId, string[] commandIds, string[] rightIds) : Ardalis.SharedKernel.ICommand<Result<UserRightDTO[]>>;
