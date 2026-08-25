import { useState, useEffect, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom'; 
import { getGlobalItems, removeItem, toggleItemFavorite, markItemRead } from 'Actions/feedItemsActions';
import FeedItemFilters from 'Components/feeditems/FeedItemFilters';

export default function AllFeedItems() {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { list: items, loading, error } = useSelector((state) => state.feedItems);

    const [filters, setFilters] = useState({
        isRead: undefined,
        isFavorite: undefined,
        from: undefined,
        to: undefined,
    });

    useEffect(() => {
        dispatch(getGlobalItems(filters));
    }, [dispatch, filters]);

    const createMarkReadHandler = useCallback((item) => () => {
        dispatch(markItemRead({ itemId: item.id, isRead: !item.isRead }));
    }, [dispatch]);

    const createFavoriteHandler = useCallback((item) => () => {
        dispatch(toggleItemFavorite({ itemId: item.id, isFavorite: !item.isFavorite }));
    }, [dispatch]);

    const createDeleteHandler = useCallback((item) => () => {
        if (window.confirm('Remove this item?')) {
            dispatch(removeItem(item.id));
        }
    }, [dispatch]);

    const handleCardClick = (itemId) => {
        navigate(`/feed-items/${itemId}`);
    };

    if (loading) return <p className="empty-text">Loading items...</p>;
    if (error) return <p className="error-text">{error}</p>;

    return (
        <div className="page">
            <section className="section">
                <div className="section-header">
                    <h2>Feed Items</h2>
                </div>
                <FeedItemFilters filters={filters} onChange={setFilters} />
                {items.length === 0 && <p className="empty-text">No items yet.</p>}

                {items.map((item) => (
                    <div 
                        className="item-card" 
                        key={item.id}
                        onClick={() => handleCardClick(item.id)}
                    >
                        <h3>{item.title}</h3>
                        <p>{item.description}</p>
                        <div>
                            <button
                                className="ghost"
                                onClick={createMarkReadHandler(item)}
                            >
                                {item.isRead ? 'Mark unread' : 'Mark read'}
                            </button>
                            <button
                                className="ghost"
                                onClick={createFavoriteHandler(item)}
                            >
                                {item.isFavorite ? '★ Favorited' : '☆ Favorite'}
                            </button>
                            <button 
                                className="danger" 
                                onClick={createDeleteHandler(item)}
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                ))}
            </section>
        </div>
    );
}