namespace KFA.SubSystem.Web.EndPoints.PasswordSafes;

public class UpdatePasswordSafeResponse
{
  public UpdatePasswordSafeResponse(PasswordSafeRecord passwordSafe)
  {
    PasswordSafe = passwordSafe;
  }

  public PasswordSafeRecord PasswordSafe { get; set; }
}
