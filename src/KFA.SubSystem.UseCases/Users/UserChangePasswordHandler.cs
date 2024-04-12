using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.Services;

namespace KFA.SubSystem.UseCases.Users;

public class UserChangePasswordHandler(IAuthService userService) : ICommandHandler<UserChangePasswordCommand, Result>
{
  public async Task<Result> Handle(UserChangePasswordCommand request,
    CancellationToken cancellationToken)
  {
     return await userService.ChangePasswordAsync(request.userId, request.currentPassword, request.newPassword, request.device,cancellationToken);
  }
}
