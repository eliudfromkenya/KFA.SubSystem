using Ardalis.Result;
using KFA.SubSystem.Globals.Models;

namespace KFA.SubSystem.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserLoginCommand(string username, string password, string? device) : Ardalis.SharedKernel.ICommand<Result<LoginResult>>;
