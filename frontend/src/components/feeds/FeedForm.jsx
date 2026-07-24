import { useState } from 'react';

export default function FeedForm({ initialUrl = '', onSubmit, onCancel }) {
    const [url, setUrl] = useState(initialUrl);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!url.trim()) return;
        onSubmit(url.trim());
    };

    return (
        <form onSubmit={handleSubmit}>
        <input
            type="url"
            placeholder="https://example.com/rss"
            value={url}
            onChange={(e) => setUrl(e.target.value)}
            required
        />
        <button type="submit">Save</button>
        {onCancel && <button type="button" onClick={onCancel}>Cancel</button>}
        </form>
    );
}