﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchExpenseBudgetBatchHeaderValidator : Validator<PatchExpenseBudgetBatchHeaderRequest>
{
  public PatchExpenseBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
     .NotEmpty()
     .WithMessage("The batch key of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
