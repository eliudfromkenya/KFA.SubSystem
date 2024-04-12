namespace KFA.SubSystem.Web.EndPoints.VerificationTypes;

public class UpdateVerificationTypeResponse
{
  public UpdateVerificationTypeResponse(VerificationTypeRecord verificationType)
  {
    VerificationType = verificationType;
  }

  public VerificationTypeRecord VerificationType { get; set; }
}
