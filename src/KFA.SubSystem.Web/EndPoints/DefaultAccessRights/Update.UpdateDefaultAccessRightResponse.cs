﻿namespace KFA.SubSystem.Web.EndPoints.DefaultAccessRights;

public class UpdateDefaultAccessRightResponse
{
  public UpdateDefaultAccessRightResponse(DefaultAccessRightRecord defaultAccessRight)
  {
    DefaultAccessRight = defaultAccessRight;
  }

  public DefaultAccessRightRecord DefaultAccessRight { get; set; }
}
