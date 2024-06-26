﻿using Ardalis.Result;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Core.Models;
using KFA.SubSystem.Core.Services;
using KFA.SubSystem.Globals.DataLayer;
using KFA.SubSystem.Globals.Models;
using KFA.SubSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KFA.SubSystem.Infrastructure.Services;

internal class AuthService(AppDbContext context, IIdGenerator idGenerator) : IAuthService
{
  public async Task<LoginResult?> LoginAsync(string username, string password, string? device, CancellationToken cancellationToken)
  {
      using (context)
      {
        var users = context.SystemUsers
        .Where(b => b.Username == username || b.Id == username || b.EmailAddress == username)
        .AsNoTracking()
          //.Select(b => new { b.Id, b.ExpirationDate, b.IsActive, b.MaturityDate, b.NameOfTheUser, b.PasswordHash, b.PasswordSalt, b.RoleId, b.Username, })
          .ToArray();

        users = users.Where(user => VerifyUser(password, user.PasswordHash, user.PasswordSalt)).ToArray();
        if (users.Length == 0) throw new UnauthorizedAccessException("Invalid user login credentials\r\nPlease check username or password");

        users = users.Where(user => user?.IsActive ?? false).ToArray();
        if (users.Length == 0) throw new UnauthorizedAccessException("User is inactive, please contact system administrator");

        users = users.Where(user => user.MaturityDate < DateTime.UtcNow).ToArray();
        if (users.Length == 0) throw new UnauthorizedAccessException($"Your time to use the system is not yet ready, please contact system administrator");

        users = users.Where(user => user.ExpirationDate > DateTime.UtcNow).ToArray();
        if (users.Length == 0) throw new UnauthorizedAccessException($"You are no longer allowed to use the system (time expired), please contact system administrator");

        var user = users.First();
        var allUserRights = context
          .UserRights
          .Where(c => c.UserId == user.Id || c.RoleId == user.RoleId)
          .Select(v => new { v.RightId, v.CommandId })
          .Distinct()
          .ToArray();

        var userRights = allUserRights
          .Where(c => c.CommandId != null && c.CommandId?.Trim()?.Length > 0)
          .Select(c => $"C-{c.CommandId}").Concat(allUserRights
          .Where(c => c.RightId != null && c.RightId?.Trim()?.Length > 0)
          .Select(c => $"R-{c.RightId}")).ToList();

        userRights.Add($"U-{user.RoleId}");

        var dataDevice = context.DataDevices.FirstOrDefault(c => c.DeviceCode == device || c.DeviceNumber == device);

        var id = new IdGenerator().GetNextId<UserLogin>();
        var login = new UserLogin
        {
          DeviceId = dataDevice?.Id,
          FromDate = DateTime.UtcNow,
          Narration = "Web Api Login",
          UserId = user.Id,
          Id = id,
          ___DateInserted___ = DateTime.UtcNow.FromDateTime(),
          ___DateUpdated___ = DateTime.UtcNow.FromDateTime(),
          ___ModificationStatus___ = 1,
          UptoDate = DateTime.UtcNow
        };
        await context.AddAsync(login, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new LoginResult
        {
          DeviceId = dataDevice?.Id,
          ExpiryDate = DateTime.Now,
          LoginId = login.Id,
          Prefix = dataDevice?.DeviceNumber,
          UserId = user.Id,
          User = (SystemUserDTO)user,
          UserRole = user.RoleId,
          UserRights = [.. userRights]
        };
      }
  }

  private static bool VerifyUser(string password, byte[]? passwordHash, byte[]? passwordSalt)
  {
    try
    {
      if (passwordHash != null && passwordSalt != null)
      {
        using var hmac = new global::System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(global::System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
          if (computedHash[i] != passwordHash[i]) return false;
        return true;
      }
    }
    catch
    {
    }
    return false;
  }

  public async Task<SystemUserDTO> RegisterUserAsync(SystemUserDTO mUser, string password, string? device, CancellationToken cancellationToken)
  {
    //using (context)
    {
      var usr = (SystemUser)mUser;
      byte[] passwordHash, passwordSalt = [];
      passwordHash = password.CreatePasswordHash(out passwordSalt);

      SystemUser user = usr with { PasswordHash = passwordHash, Username = usr.Username?.ToLower(), PasswordSalt = passwordSalt, RoleId = "USR006" };

      if (string.IsNullOrWhiteSpace(user.Id))
        user = user with { Id = idGenerator.GetNextId<SystemUser>() };

      await context.SystemUsers.AddAsync(user, cancellationToken);
      await context.SaveChangesAsync(cancellationToken);
      return user;
    }
  }

  public async Task<Result> ChangePasswordAsync(string userIdOrUsername, string oldPassword, string newPassword, string? device, CancellationToken cancellationToken)
  {
    using (context)
    {
      if (string.IsNullOrWhiteSpace(newPassword?.Trim()) || newPassword?.Trim()?.Length < 4)
        throw new Exception("New password must be atleast 4 characters");

      using var db = context;
      var user = context.SystemUsers
      .Where(b => b.Username == userIdOrUsername || b.Id == userIdOrUsername)
      .AsNoTracking()
      .FirstOrDefault() ?? throw new Exception("Can't find the user to change the password");
      byte[] passwordHash, passwordSalt = [];
      passwordHash = newPassword!.CreatePasswordHash(out passwordSalt);

      if (!VerifyUser(oldPassword, user.PasswordHash, user.PasswordSalt))
        throw new Exception("Your current(old) password is not valid");

      user = user with { IsActive = true, PasswordHash = passwordHash, PasswordSalt = passwordSalt };

      context.SystemUsers.Update(user);
      await context.SaveChangesAsync(cancellationToken);
      return Result.Success();
    }
  }

  public async Task<DataDeviceDTO> RegisterDeviceAsync(DataDeviceDTO dataDevice, CancellationToken cancellationToken)
  {
    using (context)
    {
      using var db = context;
      DataDevice device = (DataDevice)dataDevice;
      if (string.IsNullOrWhiteSpace(device.Id))
        device = device with { Id = idGenerator.GetNextId<DataDevice>() };

      await db.DataDevices.AddAsync(device, cancellationToken);
      await db.SaveChangesAsync(cancellationToken);
      return device;
    }
  }
}
