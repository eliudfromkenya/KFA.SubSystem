using FluentValidation;

namespace KFA.SubSystem.Web.Endpoints.CostCentreEndpoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetCostCentreValidator : Validator<GetCostCentreByIdRequest>
{
  public GetCostCentreValidator()
  {
    // RuleFor(x => x.CostCentreCode).NotEmpty();
  }
}
