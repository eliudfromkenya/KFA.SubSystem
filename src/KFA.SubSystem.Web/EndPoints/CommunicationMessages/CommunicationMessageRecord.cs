using KFA.SubSystem.Core.DataLayer.Types;

namespace KFA.SubSystem.Web.EndPoints.CommunicationMessages;

public record CommunicationMessageRecord(byte[]? Attachments, string? Details, string? From, string? Message, string? MessageID, CommunicationMessageType? MessageType, string? Narration, CommunicationMessageStatus? Status, string? Title, string? To, DateTime? DateInserted___, DateTime? DateUpdated___);
