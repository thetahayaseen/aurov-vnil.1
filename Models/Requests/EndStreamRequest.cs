namespace mark_vnil._1.Models.Requests;

public class EndStreamRequest
{
    public int StreamId { get; set; }
    public DateTime EndTimeStamp { get; set; }
    public required string RecordedFileUrl { get; set; }
}