﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.DataDevices;

public record UpdateDataDeviceRequest
{
  public const string Route = "/data_devices/{deviceId}";
  public string? DeviceCaption { get; set; }
  [Required]
  public string? DeviceCode { get; set; }
  [Required]
  public string? DeviceId { get; set; }
  [Required]
  public string? DeviceName { get; set; }
  public string? DeviceNumber { get; set; }
  public string? DeviceRight { get; set; }
  [Required]
  public string? StationID { get; set; }
  public string? TypeOfDevice { get; set; }
}
