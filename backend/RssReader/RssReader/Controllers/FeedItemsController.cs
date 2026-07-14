using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RssReader.Constants;
using RssReader.DTOs.FeedItem;
using RssReader.Services;
using System.Security.Claims;

namespace RssReader.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FeedItemsController(FeedItemService feedItemService) : ControllerBase
{
    protected int CurrentUserId
        => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);


    [HttpGet("items/all")]
    public async Task<IActionResult> GetAllItemsAsync(
        [FromQuery] FeedItemFilterQuery filter,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetAllFeedItemsFilteredAsync(
            CurrentUserId,
            filter,
            pageNumber,
            pageSize,
            ct);

        return Ok(result);
    }

    [HttpGet("feeds/{feedId}/items/all")]
    public async Task<IActionResult> GetAllItemsGroupedAsync(
        int feedId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetAllFeedItemsGroupedAsync(
            CurrentUserId, 
            feedId, 
            pageNumber, 
            pageSize, 
            ct);

        return Ok(result);
    }

    [HttpGet("feeds/{feedId}/items")]
    public async Task<IActionResult> GetItemsGroupedAsync(
        int feedId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize, 
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetFeedItemsGroupedAsync(CurrentUserId, feedId, pageNumber, pageSize, ct);

        return Ok(result);
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetItemsFilteredAsync(
        [FromQuery] FeedItemFilterQuery filter, 
        CancellationToken ct)
    {
        var result = await feedItemService.GetFeedItemFilteredAsync(CurrentUserId, filter, ct);

        return Ok(result);
    }

    [HttpGet("items/{itemId}")]
    public async Task<IActionResult> GetItemAsync(int itemId, CancellationToken ct)
    {
        var result = await feedItemService.GetFeedItemAsync(CurrentUserId, itemId, ct);

        return Ok(result);
    }

    [HttpPost("items/{itemId}/read")]
    public async Task<IActionResult> MarkFeedAsReadAsync(int itemId, bool isRead = true, CancellationToken ct = default)
    {
        await feedItemService.MarkAsReadAsync(CurrentUserId, itemId, isRead, ct);

        return NoContent();
    }

    [HttpPost("items/{itemId}/favorite")]
    public async Task<IActionResult> ChangeFavoriteStatusAsync(int itemId, bool isFavorite, CancellationToken ct)
    {
        await feedItemService.ChangeFavoriteStatusAsync(CurrentUserId, itemId, isFavorite, ct);

        return NoContent();
    }

    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> RemoveFeedItemAsync(int itemId, CancellationToken ct)
    {
        await feedItemService.RemoveFeedItemAsync(CurrentUserId, itemId, ct);

        return NoContent();
    }
}
