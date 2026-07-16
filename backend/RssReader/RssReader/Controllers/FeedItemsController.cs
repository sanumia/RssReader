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
[Route("api/items")]
public class FeedItemsController(FeedItemService feedItemService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("global")]
    public async Task<IActionResult> GetGlobalItems(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int pageNumber = PaginationConstants.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService
            .GetGlobalFeedItemsAsync(from, to, pageNumber, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("personal")]
    public async Task<IActionResult> GetPersonalItems(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int pageNumber = PaginationConstants.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService
            .GetPersonalFeedItemsAsync(from, to, pageNumber, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetPersonalItemsFiltered(
        [FromQuery] bool? isRead,
        [FromQuery] bool? isFavorite,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int pageNumber = PaginationConstants.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService
            .GetPersonalFeedItemsFilteredAsync(
            isRead, 
            isFavorite, 
            from, 
            to, 
            pageNumber, 
            pageSize, 
            ct);
        return Ok(result);
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetItem(int itemId, CancellationToken ct)
    {
        var result = await feedItemService.GetFeedItemAsync(itemId, ct);
        return Ok(result);
    }

    [HttpPost("{itemId}/read")]
    public async Task<IActionResult> MarkAsRead(int itemId, [FromQuery] bool isRead = true, CancellationToken ct = default)
    {
        await feedItemService.MarkAsReadAsync(itemId, isRead, ct);
        return NoContent();
    }

    [HttpPost("{itemId}/favorite")]
    public async Task<IActionResult> ChangeFavoriteStatus(int itemId, [FromQuery] bool isFavorite, CancellationToken ct)
    {
        await feedItemService.ChangeFavoriteStatusAsync(itemId, isFavorite, ct);
        return NoContent();
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoveItem(int itemId, CancellationToken ct)
    {
        await feedItemService.RemoveFeedItemAsync(itemId, ct);
        return NoContent();
    }
}
