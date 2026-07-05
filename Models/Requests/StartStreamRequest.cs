namespace mark_vnil._1.Models.Requests;

public class StartStreamRequest
{
    public string? Title { get; set; }
    public DateTime StartTimeStamp { get; set; }
    public required string SourceUrl { get; set; }   
}