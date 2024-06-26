﻿using Ardalis.Result;

namespace KFA.SubSystem.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserChangeRoleCommand(string userId, string newRoleId, string? device) : Ardalis.SharedKernel.ICommand<Result>;
