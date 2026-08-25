import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import api from '../utils/api'
import urls from '../utils/urls'


export const getAllFeeds = createAsyncThunk(
    'feeds/getAllFeeds',
    async (_, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_ALL_FEEDS_URL);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getFeeds = createAsyncThunk(
    'feeds/getFeeds',
    async (_, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_FEEDS_URL);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const addFeed = createAsyncThunk(
    'feeds/addFeeds',
    async(feed, { rejectWithValue }) => {
        try {
            const response = await api.post(urls.ADD_FEED_URL, feed);
    
            return response.data;
        }
        catch(err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const editFeed = createAsyncThunk(
    'feeds/editFeeds',
    async (feed, { rejectWithValue }) => {
        try{
            const response = await api.put(urls.EDIT_FEED_URL(feed.id), feed);
    
            return response.data;
        }
        catch(err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const removeFeed = createAsyncThunk(
    'feeds/removeFeed',
    async (id, { rejectWithValue }) => {
        try {
            await api.delete(urls.REMOVE_FEED_URL(id));
            return id;
        } 
        catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getAllFeedsWithUserInfo = createAsyncThunk(
    'feeds/getAllFeedsWithUserInfo',
    async (_, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_ALL_FEEDS_WITH_USER_INFO_URL);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

const feedsSlice = createSlice({
    name: 'feeds',
    initialState:{
        list: [],
        loading: false,
        error: null,
        currentFeedId: null,
    },
    reducers: {
        selectFeed(state, action) {
            state.currentFeedId = action.payload;
        },
        clearFeedsError(state) {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
        .addCase(getFeeds.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(getFeeds.fulfilled, (state, action) => {
            state.loading = false;
            state.list = action.payload;
        })
        .addCase(getFeeds.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload;
        })
        .addCase(addFeed.fulfilled, (state, action) => {
            state.list.push(action.payload);
        })
        .addCase(addFeed.rejected, (state, action) => {
            state.error = action.payload;
        })
        .addCase(editFeed.fulfilled, (state, action) => {
            const idx = state.list.findIndex((f) => f.id === action.payload.id);
            if (idx !== -1) state.list[idx] = action.payload;
        })
        .addCase(editFeed.rejected, (state, action) => {
            state.error = action.payload;
        })
        .addCase(removeFeed.fulfilled, (state, action) => {
            state.list = state.list.filter((f) => f.id !== action.payload);
            if (state.currentFeedId === action.payload) state.currentFeedId = null;
        })
        .addCase(removeFeed.rejected, (state,action) => {
            state.error = action.payload;
        })
        .addCase(getAllFeeds.pending, (state) => { 
            state.loading = true; state.error = null; 
        })
        .addCase(getAllFeeds.fulfilled, (state, action) => { 
            state.loading = false; state.list = action.payload; 
        })
        .addCase(getAllFeeds.rejected, (state, action) => { 
            state.loading = false; state.error = action.payload; 
        })
        .addCase(getAllFeedsWithUserInfo.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(getAllFeedsWithUserInfo.fulfilled, (state, action) => {
            state.loading = false;
            state.list = action.payload;
        })
        .addCase(getAllFeedsWithUserInfo.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload;
        });
    },
});

export const { selectFeed, clearFeedsError } = feedsSlice.actions;
export default feedsSlice.reducer;