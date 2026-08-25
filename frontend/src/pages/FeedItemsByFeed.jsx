import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { getItemsByFeedGrouped, removeItem, markItemRead, toggleItemFavorite } from 'Actions/feedItemsActions';
import FeedItemForm from 'Components/feeditems/FeedItemForm';
import FeedItemCard from 'Components/feeditems/FeedItemCard';
import { Link } from 'react-router-dom'

export default function FeedItemsByFeed() {
    const { feedId } = useParams();
    const dispatch = useDispatch();
    const [showForm, setShowForm] = useState(false);
    const { grouped, loading, error } = useSelector((state) => state.feedItems);
    const feeds = useSelector((state) => state.feeds.list);
    const feed = feeds.find((f) => f.id === Number(feedId));

    useEffect(() => {
        dispatch(getItemsByFeedGrouped({ feedId: Number(feedId) }));
    }, [dispatch, feedId]);

    const sections = [
        ['Today', grouped?.Today],
        ['Yesterday', grouped?.Yesterday],
        ['Last 7 Days', grouped?.LastWeek],
        ['Older', grouped?.Older],
    ];

    const handleFormDone = () => {
        setShowForm(false);
        dispatch(getItemsByFeedGrouped({ feedId: Number(feedId) }));
    };

    return (
        <div className="page">
            <section className="section">
                <div className="section-header">
                    <div>
                        <Link to="/feeds" className="empty-text">← Back to Feeds</Link>
                        <h2>{feed?.title || feed?.url || 'Feed'} — Items</h2>
                    </div>
                    <button onClick={() => setShowForm(true)}>Add Item</button>
                </div>
                {
                    showForm && (
                        <FeedItemForm 
                            feedId={feedId} 
                            onDone={handleFormDone}
                        />
                    )
                }
                {loading && <p className="empty-text">Loading...</p>}
                {error && <p className="error-text">{error}</p>}
                {
                    sections.map(([label, list]) =>
                        list && list.length > 0 && (
                            <div key={label}>
                                <div className="group-heading">{label}</div>
                                {list.map((item) => 
                                    <FeedItemCard 
                                        key={item.id} 
                                        item={item} />
                                    )
                                }
                            </div>
                        )
                    )
                }
            </section>
        </div>
    );
}