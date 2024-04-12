using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.DeviceGuids;

public record UpdateDeviceGuidRequest
{
  public const string Route = "/device_guids/{guid}";
  [Required]
  public string? Guid { get; set; }
}
