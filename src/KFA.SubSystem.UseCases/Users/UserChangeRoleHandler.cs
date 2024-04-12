using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.Services;

namespace KFA.SubSystem.UseCases.Users;

public class UserChangeRoleHandler(IUserManagementService userService) : ICommandHandler<UserChangeRoleCommand, Result>
{
  public async Task<Result> Handle(UserChangeRoleCommand request,
    CancellationToken cancellationToken)
  {
    return await userService.ChangeUserRoleAsync(request.userId, request.newRoleId, request.device, cancellationToken);
  }
}
