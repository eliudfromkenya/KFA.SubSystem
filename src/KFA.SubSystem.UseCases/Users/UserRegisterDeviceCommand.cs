﻿using Ardalis.Result;
using KFA.SubSystem.Core.DTOs;

namespace KFA.SubSystem.Web.UserEndPoints;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserRegisterDeviceCommand(DataDeviceDTO device) : Ardalis.SharedKernel.ICommand<Result<DataDeviceDTO>>;
