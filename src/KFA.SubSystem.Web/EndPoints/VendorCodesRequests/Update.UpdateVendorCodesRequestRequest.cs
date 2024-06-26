﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.VendorCodesRequests;

public record UpdateVendorCodesRequestRequest
{
  public const string Route = "/vendor_codes_requests/{vendorCodeRequestID}";
  public string? AttandedBy { get; set; }
  [Required]
  public string? CostCentreCode { get; set; }
  [Required]
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public string? RequestingUser { get; set; }
  public string? Status { get; set; }
  public string? TimeAttended { get; set; }
  public string? TimeOfRequest { get; set; }
  public string? VendorCode { get; set; }
  [Required]
  public string? VendorCodeRequestID { get; set; }
  public string? VendorType { get; set; }
}
