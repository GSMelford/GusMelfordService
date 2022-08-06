﻿using GusMelfordBot.Api.Dto.ContentCollector;
using GusMelfordBot.Api.Services.Applications.ContentCollector;
using GusMelfordBot.Domain.Application.ContentCollector;
using Microsoft.AspNetCore.Mvc;

namespace GusMelfordBot.Api.Controllers;

[ApiController]
[Route("api/content-collector")]
public class ContentCollectorController : Controller
{
    private readonly IContentCollectorService _contentCollectorService;
    private readonly IContentCollectorRoomFactory _contentCollectorRoomFactory;
    
    public ContentCollectorController(
        IContentCollectorService contentCollectorService,
        IContentCollectorRoomFactory contentCollectorRoomFactory)
    {
        _contentCollectorService = contentCollectorService;
        _contentCollectorRoomFactory = contentCollectorRoomFactory;
    }
    
    [HttpGet("content")]
    public async Task<FileStreamResult?> GetContent([FromQuery] Guid contentId)
    {
        MemoryStream memoryStream = await _contentCollectorService.GetContentStreamAsync(contentId);
        FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "video/mp4");
        
        HttpContext.Response.Headers.Add("Content-Length", memoryStream.Length.ToString());
        HttpContext.Response.Headers.Add("Accept-Ranges", "bytes");
        HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        HttpContext.Response.Headers.Add("Pragma", "no-cache");
        HttpContext.Response.Headers.Add("Expires", "0");
        
        return fileStreamResult;
    }

    [HttpGet("room/content/info")]
    public ContentDto GetContentInfo([FromQuery] string roomCode)
    {
        return _contentCollectorRoomFactory.GetContentCollectorRoom(roomCode).GetContentInfo().ToDto();
    }
    
    [HttpPost("room")]
    public string CreateRoom([FromQuery] ContentFilterDto contentFilterDto)
    {
        return _contentCollectorRoomFactory.Create(
            _contentCollectorService.GetContents(contentFilterDto.ToDomain()).ToList());
    }
}