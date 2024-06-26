﻿using KFA.SubSystem.Core.DataLayer.Types;

namespace KFA.SubSystem.Web.EndPoints.QRCodesRequests;

public record QRCodesRequestRecord(string? CostCentreCode, bool? IsDuplicate, string? Narration, string? QRCodeRequestID, string? RequestData, string? ResponseData, QRResponseType? ResponseStatus, global::System.DateTime? Time, string? TimsMachineused, string? VATClass, DateTime? DateInserted___, DateTime? DateUpdated___);
