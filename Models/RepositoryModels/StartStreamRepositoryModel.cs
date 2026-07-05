namespace mark_vnil._1.Models.RepositoryModels;

public class StartStreamRepositoryModel
{
    public required int StreamId { get; set; }
    public required List<string> DetectedItemsUniqueLabels { get; set; }
}