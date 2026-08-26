import { useDispatch, useSelector } from 'react-redux';
import { useParams, useNavigate } from 'react-router-dom';
import { getItem, removeItem, toggleItemFavorite } from 'Actions/feedItemsActions';
import { useEffect, useCallback } from 'react';


export default function FeedItemDetails() {
    const { itemId } = useParams();
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const { currentItem: item, loading, error } = useSelector((state) => state.feedItems)


    useEffect(() => {
        dispatch(getItem(itemId));
    }, [dispatch, itemId]);

    const handleDelete = () => {
        if (window.confirm('Remove this item?')){
            dispatch(removeItem(Number(itemId)));
            navigate(-1);
        }
    }

    const onMarkFavoriteClick = useCallback(() => {
        dispatch(toggleItemFavorite({
            itemId: item.id,
            isFavorite: !item.isFavorite
        }))
    }, [dispatch, item])

    if(loading) return <p className="empty-text">Loading...</p>
    if(error) return <p className="error-text">{error}</p>
    if(!item) return null

    return (
        <div className="page">
            <section className="section">
                <div className="section-header">
                    <h2>{item.title}</h2>
                </div>
                <p>{item.description}</p>
                {item.iconUrl && <img src={item.iconUrl} alt="" style={{ maxWidth: '100%' }} />}

                <div className="item-card__actions">
                    <button
                        className="ghost"
                        onClick={onMarkFavoriteClick}
                    >
                        {item.isFavorite ? '★ Favorited' : '☆ Favorite'}
                    </button>
                    <a href={item.link} target="_blank" rel="noopener noreferrer">
                        <button className="ghost">Open Original</button>
                    </a>
                    <button className="danger" onClick={handleDelete}>Delete</button>
                </div>
            </section>
        </div>
    )
}