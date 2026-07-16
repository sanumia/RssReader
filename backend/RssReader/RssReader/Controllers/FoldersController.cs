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
    [HttpGet]
    public async Task<IActionResult> GetFoldersAsync(CancellationToken ct)
    {
        var result = await folderService.GetFoldersWithFeedCountsAsync(ct);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFolderAsync(FolderNameDto folderNameDto, CancellationToken ct)
    {
        var result = await folderService.CreateFolderAsync(folderNameDto, ct);
        
        return Ok(result);
    }

    [HttpPut("{folderId}")]
    public async Task<IActionResult> UpdateFolderAsync(int folderId, FolderNameDto folderNameDto, CancellationToken ct)
    {
        await folderService.RenameFolderAsync(folderId, folderNameDto, ct);
        
        return NoContent();
    }

    [HttpDelete("{folderId}")]
    public async Task<IActionResult> DeleteFolderAsync(int folderId, CancellationToken ct)
    {
        await folderService.DeleteFolderAsync(folderId, ct);

        return NoContent();
    }

    [HttpPost("{folderId}/feeds/{feedId}")]
    public async Task<IActionResult> AddFeedToFolderAsync(int folderId, int feedId, CancellationToken ct)
    {
        await folderService.AddFeedToFolderAsync(folderId, feedId, ct);

        return NoContent();
    }

    [HttpDelete("{folderId}/feeds/{feedId}")]
    public async Task<IActionResult> RemoveFeedFromFolderAsync(int folderId, int feedId, CancellationToken ct)
    {
        await folderService.RemoveFeedFromFolderAsync(folderId, feedId, ct);

        return NoContent();
    }
}
