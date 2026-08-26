import { useDispatch } from 'react-redux';
import { removeFeed } from 'Actions/feedsActions';


export function useDeleteFeed() {
    const dispatch = useDispatch();
    
    return (id) => {
        if (window.confirm('Remove this feed?')) {
            dispatch(removeFeed(id));
        }
    };
}