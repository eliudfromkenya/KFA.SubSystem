using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Services;
using KFA.SubSystem.Web.UserEndPoints;

namespace KFA.SubSystem.UseCases.Users;

public class UserRegisterDeviceHandler(IAuthService userService) : ICommandHandler<UserRegisterDeviceCommand, Result<DataDeviceDTO>>
{
  public async Task<Result<DataDeviceDTO>> Handle(UserRegisterDeviceCommand request,
    CancellationToken cancellationToken)
  {
    return await userService.RegisterDeviceAsync(request.device, cancellationToken);
  }
}

