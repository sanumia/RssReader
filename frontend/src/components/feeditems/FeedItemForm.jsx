import { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { addItemToFeed, updateFeedItem } from 'Actions/feedItemsActions';
import { useForm } from 'Hooks/useForm';


const INITIAL = {
    title: '',
    description: '',
    link: '',
    publishDate: '',
    iconUrl: '',
};

export default function FeedItemForm({ feedId, initialData = null, onDone }) {
    const dispatch = useDispatch();
    const { 
        values: {
            title,
            description,
            link,
            publishDate,
            iconUrl,
        }, 
        handleChange, 
        reset, 
        setValues
    } = useForm(INITIAL);

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const isEditing = !!initialData;

    useEffect(() => {
        if (initialData) {
            setValues({
                title: initialData.title || '',
                description: initialData.description || '',
                link: initialData.link || '',
                publishDate: initialData.publishDate 
                    ? new Date(initialData.publishDate).toISOString().slice(0, 16)
                    : '',
                iconUrl: initialData.iconUrl || '',
            });
        }
    }, [initialData, setValues]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setLoading(true); 

        const item = {
            Title: title,
            Description: description || null,
            Link: link,
            PublishDate: publishDate 
                ? new Date(publishDate).toISOString() 
                : new Date().toISOString(),
            IconUrl: iconUrl || null,
        };

        try {
            let result;
            if (isEditing) {
                result = await dispatch(updateFeedItem({ 
                    itemId: initialData.id, 
                    updates: item 
                })).unwrap();
            } else {
                result = await dispatch(addItemToFeed({ feedId, item })).unwrap();
            }

            reset();
            onDone();
        } catch (err) {
            setError(err || 'Something went wrong');
        } finally {
            setLoading(false);
        }
    };

    return (
        <form 
            className="form-row" 
            onSubmit={handleSubmit}
        >
            <input
                name="title"
                placeholder="Title"
                value={title}
                onChange={handleChange}
                required
            />
            <input
                name="description"
                placeholder="Description (optional)"
                value={description}
                onChange={handleChange}
            />
            <input
                name="link"
                type="url"
                placeholder="Link"
                value={link}
                onChange={handleChange}
                required
            />
            <input
                name="publishDate"
                type="datetime-local"
                value={publishDate}
                onChange={handleChange}
            />
            <input
                name="iconUrl"
                type="url"
                placeholder="Icon URL (optional)"
                value={iconUrl}
                onChange={handleChange}
            />

            <div className="form-row">
                <button type="submit" disabled={loading}>
                    {loading ? 'Saving...' : isEditing ? 'Update Item' : 'Add Item'}
                </button>
                <button 
                    type="button" 
                    className="ghost" 
                    onClick={onDone}
                    disabled={loading}
                >
                    Cancel
                </button>
            </div>
            {error && <p className="error-text">{error}</p>}
        </form>
    );
}