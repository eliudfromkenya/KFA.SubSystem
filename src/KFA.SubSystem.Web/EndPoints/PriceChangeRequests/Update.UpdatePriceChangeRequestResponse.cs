﻿namespace KFA.SubSystem.Web.EndPoints.PriceChangeRequests;

public class UpdatePriceChangeRequestResponse
{
  public UpdatePriceChangeRequestResponse(PriceChangeRequestRecord priceChangeRequest)
  {
    PriceChangeRequest = priceChangeRequest;
  }

  public PriceChangeRequestRecord PriceChangeRequest { get; set; }
}
