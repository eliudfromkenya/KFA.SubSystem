namespace KFA.SubSystem.Web.EndPoints.SystemUsers;

public class UpdateSystemUserResponse
{
  public UpdateSystemUserResponse(SystemUserRecord systemUser)
  {
    SystemUser = systemUser;
  }

  public SystemUserRecord SystemUser { get; set; }
}
