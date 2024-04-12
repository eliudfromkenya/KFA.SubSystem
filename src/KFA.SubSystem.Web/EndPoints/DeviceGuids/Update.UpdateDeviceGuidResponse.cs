namespace KFA.SubSystem.Web.EndPoints.DeviceGuids;

public class UpdateDeviceGuidResponse
{
  public UpdateDeviceGuidResponse(DeviceGuidRecord deviceGuid)
  {
    DeviceGuid = deviceGuid;
  }

  public DeviceGuidRecord DeviceGuid { get; set; }
}
