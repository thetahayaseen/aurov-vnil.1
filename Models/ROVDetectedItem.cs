namespace mark_vnil._1.Models; 

public class ROVDetectedItem 
{
    public int Id { get; set; }
    public int StreamId { get; set; }
    public required string Label { get; set; }
    public float Confidence { get; set; }
    public DateTime DetectedAtTimeStamp { get; set; }
    public required string SnapshotFileUrl { get; set; }
    public ROVStream? Stream { get; set; }
}