namespace KFA.SubSystem.Web.EndPoints.Verifications;

public class UpdateVerificationResponse
{
  public UpdateVerificationResponse(VerificationRecord verification)
  {
    Verification = verification;
  }

  public VerificationRecord Verification { get; set; }
}
