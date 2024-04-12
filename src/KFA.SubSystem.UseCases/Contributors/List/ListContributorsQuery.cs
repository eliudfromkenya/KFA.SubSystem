using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SubSystem.UseCases.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
