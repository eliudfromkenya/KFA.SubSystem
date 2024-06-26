﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Models;
[Table("tbl_dues_payment_details")]
public sealed record class DuesPaymentDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_dues_payment_details";
  // [Required]
  [Encrypted]
  [Column("amount")]
  public string? Amount { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [Column("date")]
  public string? Date { get; init; } = string.Empty;

  [MaxLength(25, ErrorMessage = "Please document no must be 25 characters or less")]
  [Encrypted]
  [Column("document_no")]
  public string? DocumentNo { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [Column("paid_to")]
  public string? PaidTo { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [Column("is_final_payment")]
  public string? IsFinalPayment { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please employee id must be 255 characters or less")]
  [Column("employee_id")]
  public string? EmployeeId { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Encrypted]
  [Column("narration")]
  public string? Narration { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [Column("opening_balance")]
  public string? OpeningBalance { get; init; } = string.Empty;

  // [Required]
  [Column("payment_id")]
  public override string? Id { get; init; }

  // [Required]
  [Encrypted]
  [MaxLength(10, ErrorMessage = "Please payment type must be 10 characters or less")]
  [Column("payment_type")]
  public string? PaymentType { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please processed by must be 255 characters or less")]
  [Encrypted]
  [Column("processed_by")]
  public string? ProcessedBy { get; init; } = string.Empty;

  public override object ToBaseDTO()
  {
    return (DuesPaymentDetailDTO)this;
  }
}
