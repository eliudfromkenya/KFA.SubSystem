﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.TimsMachines;

public class CreateTimsMachineRequest
{
  public const string Route = "/tims_machines";
  public string? ClassType { get; set; }
  public byte? CurrentStatus { get; set; }
  public string? DomainName { get; set; }
  public string? ExternalIPAddress { get; set; }
  public string? ExternalPortNumber { get; set; }

  [Required]
  public string? InternalIPAddress { get; set; }

  public string? InternalPortNumber { get; set; }

  [Required]
  public string? MachineID { get; set; }

  public string? Narration { get; set; }

  [Required]
  public bool? ReadyForUse { get; set; }

  public string? SerialNumber { get; set; }
  public string? TimsName { get; set; }
}
