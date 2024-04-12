namespace KFA.SubSystem.Web.EndPoints.UserLogins;

public class UpdateUserLoginResponse
{
  public UpdateUserLoginResponse(UserLoginRecord userLogin)
  {
    UserLogin = userLogin;
  }

  public UserLoginRecord UserLogin { get; set; }
}
