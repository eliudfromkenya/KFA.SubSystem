namespace KFA.SubSystem.Web.EndPoints.LetPropertiesAccounts;

public class UpdateLetPropertiesAccountResponse
{
  public UpdateLetPropertiesAccountResponse(LetPropertiesAccountRecord letPropertiesAccount)
  {
    LetPropertiesAccount = letPropertiesAccount;
  }

  public LetPropertiesAccountRecord LetPropertiesAccount { get; set; }
}
