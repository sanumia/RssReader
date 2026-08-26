import { useState, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { addFeed, editFeed } from 'Actions/feedsActions';
import { getFeeds } from 'Actions/feedsActions';

export default function FeedForm({ editingFeed, onDone }) {
    const dispatch = useDispatch();
    const [url, setUrl] = useState(editingFeed?.url || '');
    const [title, setTitle] = useState(editingFeed?.title || '');
    const [iconUrl, setIconUrl] = useState(editingFeed?.iconUrl || '');
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState(null);
    const isEditing = !!editingFeed;

    const handleSubmit =  useCallback(async (e) => {
        e.preventDefault();
        if (submitting) return;
        setSubmitting(true);
        setError(null);

        const feedData = {
            url,
            title: title.trim(),
            iconUrl: iconUrl.trim() || undefined,
            ...(isEditing && { id: editingFeed.id }),
        };

        const action = isEditing
            ? editFeed(feedData)
            : addFeed(feedData);

        const result = await dispatch(action);

        if (result.meta.requestStatus === 'fulfilled') {
            setUrl('');
            setTitle('');
            setIconUrl('');
            dispatch(getFeeds());
            onDone();
        } else {
            setError(result.payload || 'Something went wrong');
        }
        setSubmitting(false);
    }, [editingFeed, url, title, iconUrl, isEditing, editingFeed, submitting, onDone]);

    return (
        <form className="form-row" onSubmit={handleSubmit}>
            <input
                type="url"
                placeholder="https://example.com/rss"
                value={url}
                onChange={(e) => setUrl(e.target.value)}
                required
            />
            <input
                type="text"
                placeholder="Title"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
            />
            <input
                type="url"
                placeholder="Icon URL (optional)"
                value={iconUrl}
                onChange={(e) => setIconUrl(e.target.value)}
            />
            <button type="submit">{isEditing ? 'Save' : 'Create Feed'}</button>
            <button 
                type="button" 
                className="ghost" 
                onClick={onDone}
            >
                Cancel
            </button>
            {error && <p className="error-text">{error}</p>}
        </form>
    );
}