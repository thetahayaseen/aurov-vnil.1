using mark_vnil._1.Models.Requests;
using mark_vnil._1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace mark_vnil._1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ROVDetectedItemController : Controller
{
    private readonly ROVDetectedItemRepository _repository;
    private readonly IHubContext<ROVDetectedItemHub> _hubContext;

    public ROVDetectedItemController(ROVDetectedItemRepository repository, IHubContext<ROVDetectedItemHub> hubContext)
    {
        _repository = repository;
        _hubContext = hubContext;
    }
    

    [HttpPost("createSnapshot")]
    public async Task<IActionResult> createSnapshot([FromBody] CreateSnapshotRequest request)
    {
        var detectedItemId = await _repository.CreateSnapshot(request.StreamId, request.Label, request.Confidence, request.DetectedAtTimeStamp, request.SnapshotFileUrl);
        await _hubContext.Clients.All.SendAsync("NewItemDetected", new { request.StreamId, request.Label, request.Confidence, request.DetectedAtTimeStamp, request.SnapshotFileUrl });
        return Ok( new { detectedItemId } );
    }
    
    [HttpGet("snapshot/{filename}")]
    public IActionResult GetSnapshot(string filename)
    {
        var path = Path.Combine("snapshots", filename);
        if (!System.IO.File.Exists(path))
            return NotFound();
        return PhysicalFile(Path.GetFullPath(path), "image/jpeg");         
    }
}