﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.UserRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateUserRightValidator : Validator<CreateUserRightRequest>
{
  public CreateUserRightValidator()
  {
    RuleFor(x => x.Description)
    .NotEmpty()
    .WithMessage("Description is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ObjectName)
         .NotEmpty()
         .WithMessage("Object Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.RightId)
         .NotEmpty()
         .WithMessage("Right Id is required.");

    RuleFor(x => x.RoleId)
         .NotEmpty()
         .WithMessage("Role Id is required.");

    RuleFor(x => x.UserId)
         .NotEmpty()
         .WithMessage("User Id is required.");

    RuleFor(x => x.UserRightId)
         .NotEmpty()
         .WithMessage("User Right Id is required.");
  }
}
