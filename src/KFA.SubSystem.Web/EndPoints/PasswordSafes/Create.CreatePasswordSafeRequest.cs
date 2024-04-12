using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.PasswordSafes;

public class CreatePasswordSafeRequest
{
  public const string Route = "/password_safes";

  [Required]
  public string? Details { get; set; }

  [Required]
  public string? Name { get; set; }

  [Required]
  public string? Password { get; set; }

  [Required]
  public string? PasswordId { get; set; }

  public string? Reminder { get; set; }
  public string? UsersVisibleTo { get; set; }
}
