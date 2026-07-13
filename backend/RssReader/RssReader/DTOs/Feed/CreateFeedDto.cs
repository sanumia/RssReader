namespace RssReader.DTOs.Feed;

public class CreateFeedDto
{
    public string Url { get; set; } = string.Empty;
    public List<int>? FolderIds { get; set; }
}
