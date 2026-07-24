export default function FeedListItem({ feed, onEdit, onDelete }) {
    return (
        <div className="feed-item">
        {feed.iconUrl && <img src={feed.iconUrl} alt="" width={20} height={20} />}
        <span className="feed-item__title">{feed.title}</span>
        <span className="feed-item__category">{feed.categoryName}</span>
        <span className="feed-item__count">{feed.totalNewsCount} news</span>
        <button onClick={() => onEdit(feed)}>Edit</button>
        <button onClick={() => onDelete(feed.id)}>Delete</button>
        </div>
    );
}