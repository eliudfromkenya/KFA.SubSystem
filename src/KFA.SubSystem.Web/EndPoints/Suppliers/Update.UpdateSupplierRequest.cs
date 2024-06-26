﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.Suppliers;

public record UpdateSupplierRequest
{
  public const string Route = "/suppliers/{supplierId}";
  public string? Address { get; set; }
  public string? ContactPerson { get; set; }
  public string? CostCentreCode { get; set; }
  [Required]
  public string? Description { get; set; }
  public string? Email { get; set; }
  public string? Narration { get; set; }
  public string? PostalCode { get; set; }
  [Required]
  public string? SupplierCode { get; set; }
  [Required]
  public string? SupplierId { get; set; }
  public string? Telephone { get; set; }
  public string? Town { get; set; }
}
