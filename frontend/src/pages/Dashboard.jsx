import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getAllFeeds, addFeed, editFeed, removeFeed } from 'Actions/feedsActions.js';
import FeedListItem from 'Components/feeds/FeedListItem.jsx';
import { Link } from 'react-router-dom';
import FeedForm from 'Components/feeds/FeedForm.jsx';
import Loader from 'Components/ui/Loader.jsx';
import { getGlobalItems, removeItem } from 'Actions/feedItemsActions';
import { useDeleteFeed } from 'Hooks/useDeleteFeed';



export default function Dashboard() {
    const dispatch = useDispatch();
    const { list: feeds, loading: feedsLoading, error: feedsError } = useSelector((state) => state.feeds);
    const { list: items, loading: itemsLoading } = useSelector((state) => state.feedItems);
    const firstFiveItems = items.slice(0, 5);
    
    const isAuthenticated = useSelector((state) => state.auth.isAuthenticated);
    const [editingFeed, setEditingFeed] = useState(null); 
    const isEditing = !!editingFeed;
    const deleteFeed = useDeleteFeed();

    useEffect(() => {
        dispatch(getAllFeeds());
        dispatch(getGlobalItems());
    }, [dispatch]);

    const handleAdd = (url) => {
        dispatch(addFeed({ url }));
        dispatch(getAllFeeds());
        setEditingFeed(null);
    };

    const handleEditSubmit = (url) => {
        dispatch(editFeed({ id: editingFeed.id, url }));
        setEditingFeed(null);
    };

    const handleDeleteItem = (id) => {
        if (window.confirm('Remove this item?')) {
            dispatch(removeItem(id));
        }
    };

    return (
        <div className="dashboard">
            {!isAuthenticated && (
                    <p className="dashboard__empty">
                        <Link to="/login">Log in</Link> or{' '}
                        <Link to="/register">create an account</Link> to add and manage your own feeds.
                    </p>
                )}
            <section className="dashboard__section">
                <div className="dashboard__section-header">
                    <h2 className="dashboard__title">Feeds</h2>
                    {
                        isAuthenticated && editingFeed !== 'new' && (
                            <button 
                                className="btn btn--primary" 
                                onClick={() => setEditingFeed('new')}>
                                + Add Feed
                            </button>
                        )
                    }
                </div>

                {
                    isAuthenticated && isEditing
                    && (
                        <FeedForm
                            editingFeed={editingFeed === 'new' ? null : editingFeed}
                            onDone={() => 
                                setEditingFeed(null)}
                        />
                    )
                }

                {
                    isAuthenticated && feedsLoading && <p className="dashboard__empty">Loading feeds...</p>
                }
                {
                    isAuthenticated && feedsError && <p className="control-form__error">{feedsError}</p>
                }
                {
                    isAuthenticated && !feedsLoading && feeds.length === 0 
                    && (
                        <p className="dashboard__empty">No feeds yet — add your first one above.</p>
                    )
                }

                {
                    feeds.map((feed) => (
                        <FeedListItem
                            key={feed.id}
                            feed={feed}
                            onEdit={setEditingFeed}
                            onDelete={deleteFeed}
                            canManage={isAuthenticated}
                        />
                ))}
            </section>

            <section className="dashboard__section">
                <div className="dashboard__section-header">
                    <h2 className="dashboard__title">Recent Feed Items</h2>
                    <Link 
                        to="/feed-items" 
                        className="dashboard__view-all"
                    >
                        View all 
                    </Link>
                </div>

                {
                    itemsLoading && <p className="dashboard__empty">Loading items...</p>
                }
                {
                    !itemsLoading && items.length === 0 && (<p className="dashboard__empty">No items yet.</p>)
                }

                {
                    firstFiveItems.map((item) => (
                        <div className="item-card" key={item.id}>
                            <h3 className="item-card__title">{item.title}</h3>
                            <p className="item-card__description">{item.description}</p>
                            {
                                isAuthenticated && (
                                    <div className="item-card__actions">
                                        <button 
                                            className="btn btn--danger" 
                                            onClick={() => handleDeleteItem(item.id)}>
                                            Delete
                                        </button>
                                    </div>
                                )
                            }
                        </div>
                ))}
            </section>
        </div>
    );
}

