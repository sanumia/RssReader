using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RssReader.DTOs.Feed;
using RssReader.Repositories.Interfaces;
using RssReader.Services.Interfaces;
using System.Security.Claims;

namespace RssReader.Controllers;

[ApiController]
[Authorize]
[Route("api/feeds")]
public class FeedsController(IFeedService feedService) : ControllerBase
{
    protected int CurrentUserId
        => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    [HttpGet("all")]
    public async Task<IActionResult> GetAllFeedsAsync(CancellationToken ct)
    {
        var result = await feedService.GetAllFeedsAsync(ct);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetFeedsAsync(CancellationToken ct)
    {
        var result = await feedService.GetFeedsForDashboardAsync(CurrentUserId, ct);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeedAsync(CreateFeedDto createFeedDto, CancellationToken ct)
    {
        var result = await feedService.CreateFeedAsync(CurrentUserId, createFeedDto, ct);
        
        return Ok(result);
    }

    [HttpPut("{feedId}")]
    public async Task<IActionResult> UpdateFeedAsync(int feedId, UpdateFeedDto updateFeedDto, CancellationToken ct)
    {
        await feedService.UpdateFeedAsync(CurrentUserId, feedId, updateFeedDto, ct);
        
        return NoContent();
    }

    [HttpDelete("{feedId}")]
    public async Task<IActionResult> DeleteFeedAsync(int feedId, CancellationToken ct)
    {
        await feedService.RemoveFeedAsync(CurrentUserId, feedId, ct);
        
        return NoContent();
    }
}
