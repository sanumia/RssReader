namespace RssReader.DTOs.Folder;

public class ResponseFolderDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int FeedCount { get; set; }
}
