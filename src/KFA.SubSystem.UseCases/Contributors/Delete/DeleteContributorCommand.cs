using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SubSystem.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
