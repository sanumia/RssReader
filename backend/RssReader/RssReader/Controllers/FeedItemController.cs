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
[Route("api/feeditems")]
public class FeedItemController(FeedItemService feedItemService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("global")]
    public async Task<IActionResult> GetGlobalItems(
        [FromQuery] GlobalFeedItemsFilter globalFeedItemsFilter,
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetGlobalFeedItemsAsync(globalFeedItemsFilter, ct);

        return Ok(result);
    }

    [HttpGet("personal")]
    public async Task<IActionResult> GetPersonalItems(
        [FromQuery] GlobalFeedItemsFilter globalFeedItemsFilter,
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetPersonalFeedItemsAsync(globalFeedItemsFilter, ct);

        return Ok(result);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetPersonalItemsFiltered(
        [FromQuery] PersonalFeedItemsFilter personalFeedItemsFilter,
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetPersonalFeedItemsFilteredAsync(personalFeedItemsFilter, ct);

        return Ok(result);
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetItem(int itemId, CancellationToken ct)
    {
        var result = await feedItemService.GetFeedItemAsync(itemId, ct);

        return Ok(result);
    }

    [HttpPost("{itemId:int}/read")]
    public async Task<IActionResult> MarkAsRead(int itemId, [FromQuery] bool isRead = true, CancellationToken ct = default)
    {
        await feedItemService.MarkAsReadAsync(itemId, isRead, ct);

        return NoContent();
    }

    [HttpPost("{itemId:int}/favorite")]
    public async Task<IActionResult> ChangeFavoriteStatus(int itemId, [FromQuery] bool isFavorite, CancellationToken ct)
    {
        await feedItemService.ChangeFavoriteStatusAsync(itemId, isFavorite, ct);

        return NoContent();
    }

    [HttpDelete("{itemId:int}")]
    public async Task<IActionResult> RemoveItem(int itemId, CancellationToken ct)
    {
        await feedItemService.RemoveFeedItemAsync(itemId, ct);

        return NoContent();
    }
}
