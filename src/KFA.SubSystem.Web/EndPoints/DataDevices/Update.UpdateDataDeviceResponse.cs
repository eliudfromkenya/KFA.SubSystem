namespace KFA.SubSystem.Web.EndPoints.DataDevices;

public class UpdateDataDeviceResponse
{
  public UpdateDataDeviceResponse(DataDeviceRecord dataDevice)
  {
    DataDevice = dataDevice;
  }

  public DataDeviceRecord DataDevice { get; set; }
}
