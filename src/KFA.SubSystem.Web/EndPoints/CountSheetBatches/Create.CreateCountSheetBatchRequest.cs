﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.CountSheetBatches;

public class CreateCountSheetBatchRequest
{
  public const string Route = "/count_sheet_batches";

  [Required]
  public string? BatchKey { get; set; }

  public string? BatchNumber { get; set; }
  public string? ClassOfCard { get; set; }
  public short? ComputerNumberOfRecords { get; set; }
  public decimal? ComputerTotalAmount { get; set; }
  public string? CostCentreCode { get; set; }
  public global::System.DateTime? Date { get; set; }
  public string? Month { get; set; }
  public string? Narration { get; set; }
  public short? NoOfRecords { get; set; }
  public decimal? TotalAmount { get; set; }
}
