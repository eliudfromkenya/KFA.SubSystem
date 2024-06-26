﻿namespace KFA.SubSystem.Web.EndPoints.PriceChangeRequests;

public record DeletePriceChangeRequestRequest
{
  public const string Route = "/price_change_requests/{requestID}";
  public static string BuildRoute(string? requestID) => Route.Replace("{requestID}", requestID);
  public string? RequestID { get; set; }
}
