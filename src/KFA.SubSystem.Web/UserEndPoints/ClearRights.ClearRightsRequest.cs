using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.UserEndPoints;

public class ClearRightsRequest
{
  public const string Route = "/users/clear_rights";

  [Required]
  public string? UserId { get; set; }

  [Required]
  public string[] UserRightIds { get; set; } = [];

  public string? Device { get; set; }
}
