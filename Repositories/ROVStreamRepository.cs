using mark_vnil._1.Data;
using mark_vnil._1.Models;
using Microsoft.EntityFrameworkCore;

namespace mark_vnil._1.Repositories;

public class ROVStreamRepository
{
    private readonly AppDbContext _dbContext;
    
    public ROVStreamRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> StartStream(string title, DateTime startTimeStamp, string sourceUrl)
    {
        var stream = new ROVStream
        {
            Title = title,
            StartTimeStamp = startTimeStamp,
            SourceUrl = sourceUrl,
            IsActive = true
        };

        _dbContext.ROVStreams.Add(stream);
        await _dbContext.SaveChangesAsync();

        return stream.Id;
    } 

  public async Task EndStream(int streamId, DateTime endTimeStamp, string recordedFileUrl)
    {
        var stream = await _dbContext.ROVStreams.FindAsync(streamId);
        if (stream == null) return;

        stream.EndTimeStamp = endTimeStamp;
        stream.RecordedFileUrl = recordedFileUrl;
        stream.IsActive = false;
        
        await _dbContext.SaveChangesAsync();
    } 

    public async Task<List<ROVStream>> GetAllInactiveStreams()
    {
        return await _dbContext.ROVStreams.Include(o => o.ROVDetectedItems).Where(o => !o.IsActive).OrderByDescending(o => o.EndTimeStamp).ToListAsync();
    }
}