using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.DeviceGuids;

public class CreateDeviceGuidRequest
{
  public const string Route = "/device_guids";

  [Required]
  public string? Guid { get; set; }
}
