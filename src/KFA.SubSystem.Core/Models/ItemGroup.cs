﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Models;
[Table("tbl_item_groups")]
public sealed record class ItemGroup : BaseModel
{
  public override object ToBaseDTO()
  {
    return (ItemGroupDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_item_groups";
  // [Required]
  [Column("group_id")]
  public override string? Id { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please name must be 255 characters or less")]
  [Column("name")]
  public string? Name { get; init; }

  [Column("parent_group_id")]
  public string? ParentGroupId { get; init; }

  [ForeignKey(nameof(ParentGroupId))]
  public ItemGroup? ParentGroup { get; set; }
  [NotMapped]
  public string? ParentGroup_Caption { get; set; }

  public ICollection<ItemGroup>? ItemGroups { get; set; }
  public ICollection<StockItem>? StockItems { get; set; }
}
