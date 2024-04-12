using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Services;

namespace KFA.SubSystem.UseCases.Users;

public class UserAddRightsHandler(IUserManagementService userService) : ICommandHandler<UserAddRightsCommand, Result<UserRightDTO[]>>
{
  public async Task<Result<UserRightDTO[]>> Handle(UserAddRightsCommand request,
    CancellationToken cancellationToken)
  {
    return await userService.AddUserRightsAsync(request.userId, request.rightIds, request.commandIds, cancellationToken);
  }
}
