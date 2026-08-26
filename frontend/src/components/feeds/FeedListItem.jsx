import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import React from 'react'
const IMAGE_SIZE = 20;


export default function FeedListItem({ feed, onEdit, onDelete, canManage }) {
    const {
        id,
        title,
        folderNames,
        feedItemCount,
        iconUrl
    } = feed;

    const navigate = useNavigate();

    const folders = folderNames?.join(', ') || 'No folder';

    const handleCardClick = () => {
        navigate(`/feeds/${feed.id}`);
    };

    const handleEditClick = useCallback( (e) => {
        e.stopPropagation();
        onEdit(feed);
    }, [feed, onEdit]);

    const handleDeleteClick = useCallback((e) => {
        e.stopPropagation();
        onDelete(id);
    }, [id, onDelete]);

    return (
        <div 
            className="feed-item" 
            onClick={handleCardClick}
        >
            {
                iconUrl && (
                    <img 
                        className="feed-item__icon"
                        src={iconUrl} 
                        alt="" 
                        width={IMAGE_SIZE} 
                        height={IMAGE_SIZE} 
                    />
                )
            }
            <span className="feed-item__title">{title}</span>
            {
                canManage && (
                    <React.Fragment>
                        <button onClick={handleEditClick}>Edit</button>
                        <button onClick={handleDeleteClick}>Delete</button>
                    </React.Fragment>
                )
            }
        </div>
    );
}