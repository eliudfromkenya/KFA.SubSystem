﻿namespace KFA.SubSystem.Web.EndPoints.UserRoles;

public class UpdateUserRoleResponse
{
  public UpdateUserRoleResponse(UserRoleRecord userRole)
  {
    UserRole = userRole;
  }

  public UserRoleRecord UserRole { get; set; }
}
