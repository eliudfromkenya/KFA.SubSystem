﻿namespace KFA.SubSystem.Web.EndPoints.Suppliers;

public class UpdateSupplierResponse
{
  public UpdateSupplierResponse(SupplierRecord supplier)
  {
    Supplier = supplier;
  }

  public SupplierRecord Supplier { get; set; }
}
