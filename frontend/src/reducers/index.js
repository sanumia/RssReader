import { combineReducers } from 'redux'
import feedsReducer from './feedsSlice'

const rootReducer = combineReducers({
    feeds: feedsReducer
});

export default rootReducer;