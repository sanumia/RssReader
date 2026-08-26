using System.ComponentModel.DataAnnotations;

namespace RssReader.Models;

public class FeedItem
{
    public int Id { get; set; }
    public int FeedId { get; set; }
    public Feed Feed { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string Link { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public string? IconUrl { get; set; }
    public string? Attachments { get; set; }
    public ICollection<UserFeedItem> UserFeedItems { get; set; } = new List<UserFeedItem>();
}
