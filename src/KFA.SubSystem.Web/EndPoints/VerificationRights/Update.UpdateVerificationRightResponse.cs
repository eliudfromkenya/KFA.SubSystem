﻿namespace KFA.SubSystem.Web.EndPoints.VerificationRights;

public class UpdateVerificationRightResponse
{
  public UpdateVerificationRightResponse(VerificationRightRecord verificationRight)
  {
    VerificationRight = verificationRight;
  }

  public VerificationRightRecord VerificationRight { get; set; }
}
