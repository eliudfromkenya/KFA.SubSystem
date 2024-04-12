using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.UserRights;

public record UpdateUserRightRequest
{
  public const string Route = "/user_rights/{userRightId}";
  [Required]
  public string? Description { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? ObjectName { get; set; }
  [Required]
  public string? RightId { get; set; }
  [Required]
  public string? RoleId { get; set; }
  [Required]
  public string? UserId { get; set; }
  [Required]
  public string? UserRightId { get; set; }
}
