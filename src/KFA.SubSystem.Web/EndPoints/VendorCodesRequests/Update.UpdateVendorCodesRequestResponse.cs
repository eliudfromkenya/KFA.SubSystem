﻿namespace KFA.SubSystem.Web.EndPoints.VendorCodesRequests;

public class UpdateVendorCodesRequestResponse
{
  public UpdateVendorCodesRequestResponse(VendorCodesRequestRecord vendorCodesRequest)
  {
    VendorCodesRequest = vendorCodesRequest;
  }

  public VendorCodesRequestRecord VendorCodesRequest { get; set; }
}
