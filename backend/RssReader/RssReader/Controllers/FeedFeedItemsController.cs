using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RssReader.Constants;
using RssReader.Services.Interfaces;

namespace RssReader.Controllers;

[ApiController]
[Authorize]
[Route("api/feed/{feedId}/items")]
public class FeedFeedItemsController(IFeedItemService feedItemService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetItems(
        int feedId,
        [FromQuery] int pageNumber = PaginationConstants.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService.GetFeedItemsByFeedIdAsync(feedId, pageNumber, pageSize, ct);

        return Ok(result);
    }

    [HttpGet("grouped")]
    public async Task<IActionResult> GetItemsGrouped(
        int feedId,
        [FromQuery] int pageNumber = PaginationConstants.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await feedItemService
            .GetFeedItemsGroupedByFeedIdAsync(feedId, pageNumber, pageSize, ct);
        return Ok(result);
    }
}
