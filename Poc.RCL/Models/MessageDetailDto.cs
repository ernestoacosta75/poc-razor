namespace Poc.RCL.Models
{
    public record MessageDetailDto(int MessageId, string VesselName, string Voyage, string MessageType,
        string OriginalContent, List<AttachmentDto> Attachments, List<CommentDto> Comments,
        List<SyncHistoryEntryDto> SyncHistory)
    {
    }

    public record AttachmentDto(string FileName, string SizeLabel, DateTime UploadedDate)
    {

    }

    public record CommentDto(string Author, string Text, DateTime CreatedDate)
    {
    }

    public record SyncHistoryEntryDto(DateTime Date, List<FieldChangeDto> Changes)
    {

    }

    public record FieldChangeDto(string FieldName, string OldValue, string NewValue)
    {

    }

    public record AttachmentFileDto(byte[] Content, string ContentType, string FileName)
    {

    }
}
