﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.StockCountSheets;

public class CreateStockCountSheetRequest
{
  public const string Route = "/stock_count_sheets";
  public decimal? Actual { get; set; }
  public decimal? AverageAgeMonths { get; set; }
  public string? BatchKey { get; set; }

  [Required]
  public string? CountSheetDocumentId { get; set; }

  [Required]
  public string? CountSheetId { get; set; }

  public string? DocumentNumber { get; set; }

  [Required]
  public string? ItemCode { get; set; }

  public string? Narration { get; set; }
  public decimal? QuantityOnHand { get; set; }
  public decimal? QuantitySoldLast12Months { get; set; }
  public decimal? SellingPrice { get; set; }
  public decimal? StocksOver { get; set; }
  public decimal? StocksShort { get; set; }
  public decimal? UnitCostPrice { get; set; }
}
