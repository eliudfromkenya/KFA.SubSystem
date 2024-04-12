using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.UserEndPoints;

public class LoginRequest
{
  public const string Route = "/users/login";

  [Required]
  public string? Username { get; set; }

  [Required]
  public string? Password { get; set; }

  public string? Device { get; set; }
}
