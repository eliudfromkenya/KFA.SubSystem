using Ardalis.Result;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals.Models;

namespace KFA.SubSystem.Core.Services;

public interface IAuthService
{
  Task<LoginResult?> LoginAsync(string username, string password, string? device, CancellationToken cancellationToken);
   
  Task<Result> ChangePasswordAsync(string userIdOrUsername, string oldPassword, string newPassword, string? device, CancellationToken cancellationToken);
  Task<DataDeviceDTO> RegisterDeviceAsync(DataDeviceDTO dataDevice, CancellationToken cancellationToken);

  Task<SystemUserDTO> RegisterUserAsync(SystemUserDTO usr, string password, string? device, CancellationToken cancellationToken);
}
