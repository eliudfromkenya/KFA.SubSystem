﻿using KFA.SubSystem.Core.DataLayer.Types;

namespace KFA.SubSystem.Web.EndPoints.UserAuditTrails;

public record UserAuditTrailRecord(global::System.DateTime? ActivityDate, UserActivities? ActivityEnumNumber, string? AuditId, string? Category, string? CommandId, string? Data, string? Description, string? LoginId, string? Narration, string? OldValues, DateTime? DateInserted___, DateTime? DateUpdated___);
