﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Models;
[Table("tbl_let_properties_accounts")]
public sealed record class LetPropertiesAccount : BaseModel
{
  public override object ToBaseDTO()
  {
    return (LetPropertiesAccountDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_let_properties_accounts";
  //[Column("account_number")]
  //public string? AccountNumber { get; init; }

  [Column("commencement_rent")]
  public decimal CommencementRent { get; init; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Column("current_rent")]
  public decimal CurrentRent { get; init; }

  [Column("last_review_date")]
  public global::System.DateTime LastReviewDate { get; init; }

  [MaxLength(25, ErrorMessage = "Please ledger account id must be 25 characters or less")]
  [Column("ledger_account_code")]
  public string? LedgerAccountCode { get; init; }
  [ForeignKey(nameof(LedgerAccountCode))]
  public LedgerAccount? LedgerAccount { get; set; }

  [Column("let_on")]
  public global::System.DateTime LetOn { get; init; }

  // [Required]
  [Column("let_property_account_id")]
  public override string? Id { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please tenant address must be 255 characters or less")]
  [Column("tenant_address")]
  public string? TenantAddress { get; init; }
  [NotMapped]
  public string? LedgerAccount_Caption { get; set; }
}
