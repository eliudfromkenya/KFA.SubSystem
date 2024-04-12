﻿namespace KFA.SubSystem.Web.EndPoints.TimsMachines;

public class UpdateTimsMachineResponse
{
  public UpdateTimsMachineResponse(TimsMachineRecord timsMachine)
  {
    TimsMachine = timsMachine;
  }

  public TimsMachineRecord TimsMachine { get; set; }
}
