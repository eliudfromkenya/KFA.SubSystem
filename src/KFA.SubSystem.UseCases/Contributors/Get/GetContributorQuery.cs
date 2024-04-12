using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SubSystem.UseCases.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
