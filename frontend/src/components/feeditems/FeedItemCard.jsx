import { Link, useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { markItemRead, toggleItemFavorite, removeItem } from 'Actions/feedItemsActions';
import { useState, useCallback } from 'react';
import FeedItemForm from 'Components/feeditems/FeedItemForm';

export default function FeedItemCard({ item }) {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [editing, setEditing] = useState(false);
    
    const {
        id,
        title,
        description,
        iconUrl,
        isRead,
        isFavorite
    } = item;

    const handleDelete = useCallback(async () => {
        if (!window.confirm('Remove this item?')) return;
        try {
            await dispatch(removeItem(id)).unwrap();
            navigate('/'); // only after successful deletion
        } catch (err) {
            console.error('Delete failed:', err);
        }
    }, [dispatch, id, navigate]);

    const onMarkReadClick = useCallback(() => {
        dispatch(markItemRead({
            itemId: id, 
            isRead: !isRead
        }))
    },[dispatch, id, isRead]);
    
    const onMarkFavoriteClick = useCallback(() => {
        dispatch(toggleItemFavorite({
            itemId: id, 
            isFavorite: !isFavorite
        }))
    },[dispatch, id, isFavorite]);

    const handleEditDone = () => {
        setEditing(false);
    };

    return (
        <div className="item-card">
            {iconUrl && (
                <img
                    className="item-card__icon"
                    src={iconUrl}
                    alt=""
                    onError={(e) => { e.target.style.display = 'none'; }}
                />
            )}
            <Link to={`/feed-items/${id}`}><h3>{title}</h3></Link>
            <p>{description}</p>
            {
                editing && (
                    <FeedItemForm
                        feedId={item.feedId}
                        initialData={item}
                        onDone={handleEditDone}
                    />
                )
            }
            <div className="item-card__actions">
                <button
                    className="ghost"
                    onClick={onMarkReadClick}
                >
                    {isRead ? 'Mark unread' : 'Mark read'}
                </button>
                <button
                    className="ghost"
                    onClick={onMarkFavoriteClick}
                >
                    {isFavorite ? '★ Favorited' : '☆ Favorite'}
                </button>
                <button className="ghost" onClick={() => setEditing(true)}>
                    Edit
                </button>
                <button className="danger" onClick={handleDelete}>Delete</button>
            </div>
        </div>
    );
}