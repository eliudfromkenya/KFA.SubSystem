﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.SalesBudgetDetails;

public record UpdateSalesBudgetDetailRequest
{
  public const string Route = "/sales_budget_details/{salesBudgetDetailId}";
  public string? BatchKey { get; set; }
  public string? ItemCode { get; set; }
  public byte? Month { get; set; }
  public decimal? Month01Quantity { get; set; }
  public decimal? Month02Quantity { get; set; }
  public decimal? Month03Quantity { get; set; }
  public decimal? Month04Quantity { get; set; }
  public decimal? Month05Quantity { get; set; }
  public decimal? Month06Quantity { get; set; }
  public decimal? Month07Quantity { get; set; }
  public decimal? Month08Quantity { get; set; }
  public decimal? Month09Quantity { get; set; }
  public decimal? Month10Quantity { get; set; }
  public decimal? Month11Quantity { get; set; }
  public decimal? Month12Quantity { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? SalesBudgetDetailId { get; set; }
  public decimal? SellingPrice { get; set; }
  public decimal? UnitCostPrice { get; set; }
}
