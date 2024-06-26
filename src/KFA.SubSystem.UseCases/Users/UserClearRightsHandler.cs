﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SubSystem.Core.Services;

namespace KFA.SubSystem.UseCases.Users;

public class UserClearRightsHandler(IUserManagementService userService) : ICommandHandler<UserClearRightsCommand, Result<string[]>>
{
  public async Task<Result<string[]>> Handle(UserClearRightsCommand request,
    CancellationToken cancellationToken)
  {
    return await userService.ClearUserRightsAsync(request.userId, request.device, cancellationToken, request.userRightsIds);
  }
}

