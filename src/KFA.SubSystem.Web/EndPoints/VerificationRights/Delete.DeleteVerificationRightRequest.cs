﻿namespace KFA.SubSystem.Web.EndPoints.VerificationRights;

public record DeleteVerificationRightRequest
{
  public const string Route = "/verification_rights/{verificationRightId}";
  public static string BuildRoute(string? verificationRightId) => Route.Replace("{verificationRightId}", verificationRightId);
  public string? VerificationRightId { get; set; }
}
