﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Models;
[Table("tbl_stock_items")]
public sealed record class StockItem : BaseModel
{
  public override object ToBaseDTO()
  {
    return (StockItemDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_stock_items";
  [MaxLength(255, ErrorMessage = "Please barcode must be 255 characters or less")]
  [Column("barcode")]
  public string? Barcode { get; init; }

  [Column("group_id")]
  public string? GroupId { get; init; }

  [ForeignKey(nameof(GroupId))]
  public ItemGroup? Group { get; set; }
  [NotMapped]
  public string? Group_Caption { get; set; }

  // [Required]
  [Column("item_code")]
  public override string? Id { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please item name must be 255 characters or less")]
  [Column("item_name")]
  public string? ItemName { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }
}
