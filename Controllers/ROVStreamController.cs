using mark_vnil._1.Models.RepositoryModels;
using mark_vnil._1.Models.Requests;
using mark_vnil._1.Models.ViewModels;
using mark_vnil._1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace mark_vnil._1.Controllers;

public class ROVStreamController : Controller
{
    private readonly ROVStreamRepository _repository;
    private readonly IHubContext<ROVStreamHub> _hubContext;

    public ROVStreamController(ROVStreamRepository repository, IHubContext<ROVStreamHub> hubContext)
    {
        _repository = repository;
        _hubContext = hubContext;
    }
    
    public async Task<IActionResult> Dashboard()
    {
        var streams = await _repository.GetAllInactiveStreams();
        var viewModel = new DashboardViewModel
        {
            AllStreams = streams,
        };

        return View(viewModel);
    }

    [HttpPost("api/rovstream/start")]
    public async Task<IActionResult> StartStream ([FromBody] StartStreamRequest request)
    {
        StartStreamRepositoryModel repositoryModel = await _repository.StartStream(request.Title!, request.StartTimeStamp, request.SourceUrl);

        int streamId = repositoryModel.StreamId;
        List<string> detectedItemsUniqueLabels = repositoryModel.DetectedItemsUniqueLabels;

        await _hubContext.Clients.All.SendAsync("StreamStarted", new { streamId, request.Title, request.SourceUrl, detectedItemsUniqueLabels } );
        return Ok( new { streamId } );
    }


    [HttpPut("api/rovstream/end")]
    public async Task<IActionResult> EndStream ([FromBody] EndStreamRequest request)
    {
        await _repository.EndStream(request.StreamId, request.EndTimeStamp, request.RecordedFileUrl); 
        await _hubContext.Clients.All.SendAsync("StreamEnded");
        return Ok();
    }

    [HttpGet("api/rovstream/recording/{filename}")]
    public IActionResult GetRecording(string filename)
    {
        var path = Path.Combine("recordings", filename);
        if (!System.IO.File.Exists(path))
            return NotFound();
        return PhysicalFile(Path.GetFullPath(path), "video/mp4", enableRangeProcessing: true);
    }
}