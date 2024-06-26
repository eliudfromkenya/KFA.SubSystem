﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateIssuesSubmissionValidator : Validator<CreateIssuesSubmissionRequest>
{
  public CreateIssuesSubmissionValidator()
  {
    RuleFor(x => x.IssueID)
    .NotEmpty()
    .WithMessage("Issue ID is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);


    RuleFor(x => x.SubmissionID)
         .NotEmpty()
         .WithMessage("Submission ID is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SubmittedTo)
         .NotEmpty()
         .WithMessage("Submitted To is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SubmittingUser)
         .NotEmpty()
         .WithMessage("Submitting User is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Type)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
