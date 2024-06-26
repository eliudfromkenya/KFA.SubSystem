﻿namespace KFA.SubSystem.Web.EndPoints.LeasedPropertiesAccounts;

public class UpdateLeasedPropertiesAccountResponse
{
  public UpdateLeasedPropertiesAccountResponse(LeasedPropertiesAccountRecord leasedPropertiesAccount)
  {
    LeasedPropertiesAccount = leasedPropertiesAccount;
  }

  public LeasedPropertiesAccountRecord LeasedPropertiesAccount { get; set; }
}
