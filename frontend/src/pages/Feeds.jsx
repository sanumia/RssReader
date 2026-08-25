import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { getAllFeeds, removeFeed } from 'Actions/feedsActions';
import FeedForm from 'Components/feeds/FeedForm';
import { getFolders } from 'Reducers/foldersReducer';
import AddToFolderButton from 'Components/feeds/AddToFolderButton';

export default function Feeds() {
    const dispatch = useDispatch();
    const { list: feeds, loading, error } = useSelector((state) => state.feeds);
    const [editingFeed, setEditingFeed] = useState(null);

    useEffect(() => {
        dispatch(getAllFeeds());
        dispatch(getFolders());
    }, [dispatch]);

    const handleDelete = (id) => {
        if (window.confirm('Delete this feed and all its items?')) {
            dispatch(removeFeed(id));
        }
    };

    return (
        <div className="page">
            <section className="section">
                <div className="section-header">
                    <h2>Feeds</h2>
                    {editingFeed !== 'new' && (
                        <button onClick={() => setEditingFeed('new')}>+ Create Feed</button>
                    )}
                </div>

                {(editingFeed === 'new' || (editingFeed && editingFeed !== 'new')) && (
                    <FeedForm
                        editingFeed={editingFeed === 'new' ? null : editingFeed}
                        onDone={() => setEditingFeed(null)}
                    />
                )}

                {loading && <p className="empty-text">Loading feeds...</p>}
                {error && <p className="error-text">{error}</p>}
                {!loading && feeds.length === 0 && (
                    <p className="empty-text">No feeds yet — create your first one above.</p>
                )}

                {feeds.map((feed) => (
                    <div 
                        className="feed-item" 
                        key={feed.id}
                    >
                        {
                            feed.iconUrl && (
                                <img
                                    className="feed-item__icon"
                                    src={feed.iconUrl}
                                    alt=""
                                    onError={(e) => 
                                        { e.target.style.display = 'none'; }
                                    }
                                />
                            )
                        }
                        <Link to={`/feeds/${feed.id}`} className="feed-item__title">
                            {feed.title || feed.url}
                        </Link>
                        <span className="feed-item__count">{feed.totalNewsCount ?? 0} news</span>
                        <div className="feed-item__actions">
                            <AddToFolderButton feedId={feed.id} />
                            <button className="ghost" onClick={() => setEditingFeed(feed)}>Edit</button>
                            <button className="danger" onClick={() => handleDelete(feed.id)}>Delete</button>
                        </div>
                    </div>
                ))}
            </section>
        </div>
    );
}