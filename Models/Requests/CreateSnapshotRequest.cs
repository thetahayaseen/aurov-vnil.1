namespace mark_vnil._1.Models.Requests;

public class CreateSnapshotRequest
{
    public int StreamId { get; set; }
    public required string Label { get; set; }
    public float Confidence { get; set; }
    public DateTime DetectedAtTimeStamp { get; set; }
    public required string SnapshotFileUrl { get; set; }
}