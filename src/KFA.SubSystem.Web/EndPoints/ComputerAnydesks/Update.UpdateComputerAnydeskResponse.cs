﻿namespace KFA.SubSystem.Web.EndPoints.ComputerAnydesks;

public class UpdateComputerAnydeskResponse
{
  public UpdateComputerAnydeskResponse(ComputerAnydeskRecord computerAnydesk)
  {
    ComputerAnydesk = computerAnydesk;
  }

  public ComputerAnydeskRecord ComputerAnydesk { get; set; }
}
