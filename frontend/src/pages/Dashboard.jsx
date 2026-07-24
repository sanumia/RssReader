import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchFeeds, addFeed, editFeed, removeFeed } from '../actions/feedActions';
import FeedListItem from '../components/feeds/FeedListItem';
import FeedForm from '../components/feeds/FeedForm';
import Loader from '../components/ui/Loader';

export default function Dashboard() {
    const dispatch = useDispatch();
    const { list: feeds, loading, error } = useSelector((state) => state.feeds);
    const [editingFeed, setEditingFeed] = useState(null); // null | feed object | 'new'

    useEffect(() => {
        dispatch(fetchFeeds());
    }, [dispatch]);

    const handleAdd = (url) => {
        dispatch(addFeed({ url }));
        setEditingFeed(null);
    };

    const handleEditSubmit = (url) => {
        dispatch(editFeed({ id: editingFeed.id, url }));
        setEditingFeed(null);
    };

    const handleDelete = (id) => {
        if (window.confirm('Remove this feed?')) dispatch(removeFeed(id));
    };

    if (loading) return <Loader />;

    return (
        <div>
        <h1>My Feeds</h1>
        {error && <p className="error">{error}</p>}

        {feeds.map((feed) => (
            <FeedListItem
            key={feed.id}
            feed={feed}
            onEdit={setEditingFeed}
            onDelete={handleDelete}
            />
        ))}

        {editingFeed === 'new' && (
            <FeedForm onSubmit={handleAdd} onCancel={() => setEditingFeed(null)} />
        )}
        {editingFeed && editingFeed !== 'new' && (
            <FeedForm
            initialUrl={editingFeed.url}
            onSubmit={handleEditSubmit}
            onCancel={() => setEditingFeed(null)}
            />
        )}

        {!editingFeed && <button onClick={() => setEditingFeed('new')}>Add Feed</button>}
        </div>
    );
}