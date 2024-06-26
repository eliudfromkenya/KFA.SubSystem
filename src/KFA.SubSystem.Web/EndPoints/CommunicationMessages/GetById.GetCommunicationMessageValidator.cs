﻿using FluentValidation;

namespace KFA.SubSystem.Web.EndPoints.CommunicationMessages;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetCommunicationMessageValidator : Validator<GetCommunicationMessageByIdRequest>
{
  public GetCommunicationMessageValidator()
  {
    RuleFor(x => x.MessageID)
      .NotEmpty()
      .WithMessage("The message id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
