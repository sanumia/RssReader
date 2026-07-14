using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RssReader.DTOs.Folder;
using RssReader.Services.Interfaces;
using System.Security.Claims;

namespace RssReader.Controllers;


[ApiController]
[Authorize]
[Route("api/folders")]
public class FoldersController(IFolderService folderService) : ControllerBase
{
    protected int CurrentUserId
        => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public async Task<IActionResult> GetFoldersAsync(CancellationToken ct)
    {
        var result = await folderService.GetFoldersWithFeedCountsAsync(CurrentUserId, ct);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFolderAsync(FolderNameDto folderNameDto, CancellationToken ct)
    {
        var result = await folderService.CreateFolderAsync(CurrentUserId, folderNameDto, ct);
        
        return Ok(result);
    }

    [HttpPut("{folderId}")]
    public async Task<IActionResult> UpdateFolderAsync(int folderId, FolderNameDto folderNameDto, CancellationToken ct)
    {
        await folderService.RenameFolderAsync(CurrentUserId, folderId, folderNameDto, ct);
        
        return NoContent();
    }

    [HttpDelete("{folderId}")]
    public async Task<IActionResult> DeleteFolderAsync(int folderId, CancellationToken ct)
    {
        await folderService.DeleteFolderAsync(CurrentUserId, folderId, ct);

        return NoContent();
    }

    [HttpPost("{folderId}/feeds/{feedId}")]
    public async Task<IActionResult> AddFeedToFolderAsync(int folderId, int feedId, CancellationToken ct)
    {
        await folderService.AddFeedToFolderAsync(CurrentUserId, folderId, feedId, ct);

        return NoContent();
    }

    [HttpDelete("{folderId}/feeds/{feedId}")]
    public async Task<IActionResult> RemoveFeedFromFolderAsync(int folderId, int feedId, CancellationToken ct)
    {
        await folderService.RemoveFeedFromFolderAsync(CurrentUserId, folderId, feedId, ct);

        return NoContent();
    }
}
