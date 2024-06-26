﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.Verifications;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetVerificationValidator : Validator<GetVerificationByIdRequest>
{
  public GetVerificationValidator()
  {
    RuleFor(x => x.VerificationId)
      .NotEmpty()
      .WithMessage("The verification id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
