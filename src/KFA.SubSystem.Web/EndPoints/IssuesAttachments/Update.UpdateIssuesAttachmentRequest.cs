﻿using System.ComponentModel.DataAnnotations;

namespace KFA.SubSystem.Web.EndPoints.IssuesAttachments;

public record UpdateIssuesAttachmentRequest
{
  public const string Route = "/issues_attachments/{attachmentID}";
  [Required]
  public string? AttachmentID { get; set; }
  public string? AttachmentType { get; set; }
  public byte[]? Data { get; set; }
  public string? Description { get; set; }
  public string? File { get; set; }
  public string? IssueID { get; set; }
  public string? Narration { get; set; }
  public global::System.DateTime? Time { get; set; }
}
