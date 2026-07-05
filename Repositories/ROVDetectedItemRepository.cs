using mark_vnil._1.Data;
using mark_vnil._1.Models;

namespace mark_vnil._1.Repositories;

public class ROVDetectedItemRepository
{
    private readonly AppDbContext _dbContext;
    
    public ROVDetectedItemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> CreateSnapshot(int streamId, string label, float confidence, DateTime detectedAtTimeStamp, string snapshotFileUrl)
    {
        var stream = await _dbContext.ROVStreams.FindAsync(streamId);
        
        var detectedItem = new ROVDetectedItem
        {
            StreamId = streamId,
            Label = label,
            Confidence = confidence,
            DetectedAtTimeStamp = detectedAtTimeStamp,
            SnapshotFileUrl = snapshotFileUrl,
            Stream = stream,
        };
        
        _dbContext.ROVDetectedItems.Add(detectedItem);
        await _dbContext.SaveChangesAsync();
        
        return detectedItem.Id;
    } 
}