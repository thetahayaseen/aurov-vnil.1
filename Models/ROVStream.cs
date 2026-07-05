namespace mark_vnil._1.Models; 

public class ROVStream
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime StartTimeStamp { get; set; }
    public DateTime? EndTimeStamp { get; set; }
    public required string SourceUrl { get; set; }
    public string? RecordedFileUrl { get; set; }
    public bool IsActive { get; set; }
    public ICollection<ROVDetectedItem>? ROVDetectedItems { get; set; }
} 