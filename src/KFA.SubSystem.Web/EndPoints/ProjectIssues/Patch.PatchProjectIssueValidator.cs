﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.ProjectIssues;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchProjectIssueValidator : Validator<PatchProjectIssueRequest>
{
  public PatchProjectIssueValidator()
  {
    RuleFor(x => x.ProjectIssueID)
     .NotEmpty()
     .WithMessage("The project issue id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
