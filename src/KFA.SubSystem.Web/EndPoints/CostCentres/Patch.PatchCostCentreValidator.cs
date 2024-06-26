﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.CostCentres;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchCostCentreValidator : Validator<PatchCostCentreRequest>
{
  public PatchCostCentreValidator()
  {
    RuleFor(x => x.CostCentreCode)
     .NotEmpty()
     .WithMessage("The cost centre code of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
