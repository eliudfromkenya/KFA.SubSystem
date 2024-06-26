﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetExpensesBudgetDetailValidator : Validator<GetExpensesBudgetDetailByIdRequest>
{
  public GetExpensesBudgetDetailValidator()
  {
    RuleFor(x => x.ExpenseBudgetDetailId)
      .NotEmpty()
      .WithMessage("The expense budget detail id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
