namespace KFA.SubSystem.Web.EndPoints.TimsMachines;

public record TimsMachineRecord(string? ClassType, byte? CurrentStatus, string? DomainName, string? ExternalIPAddress, string? ExternalPortNumber, string? InternalIPAddress, string? InternalPortNumber, string? MachineID, string? Narration, bool? ReadyForUse, string? SerialNumber, string? TimsName, DateTime? DateInserted___, DateTime? DateUpdated___);