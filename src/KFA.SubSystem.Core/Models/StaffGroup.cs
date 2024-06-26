﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SubSystem.Core.DTOs;
using KFA.SubSystem.Globals;

namespace KFA.SubSystem.Core.Models;
[Table("tbl_staff_groups")]
public sealed record class StaffGroup : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_staff_groups";
  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  [Encrypted]
  public string? Description { get; init; } = string.Empty;

  // [Required]
  [Column("group_number")]
 [Encrypted]
  public override string? Id { get; init; } = string.Empty;

  // [Required]
  [Column("is_active")]
  public bool IsActive { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  [Encrypted]
  public string? Narration { get; init; } = string.Empty;

  public ICollection<EmployeeDetail>? EmployeeDetails { get; set; }
  public override object ToBaseDTO()
  {
    return (StaffGroupDTO)this;
  }
}
