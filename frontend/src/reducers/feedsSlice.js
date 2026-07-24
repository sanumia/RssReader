import {createSlice, createAsyncThunk} from '@reduxjs/toolkit'
import api from '../utils/api'

export const getFeeds = createAsyncThunk(
    'feeds/getFeeds',
    async () => {
        const response = await api.get('/feeds'); 
        return response.data;
    }
); 

export const addFeed = createAsyncThunk(
    'feeds/addFeeds',
    async(feed) => {
        const response = await api.post('/feeds', feed);
        return response.data;
    }
);

export const editFeed = createAsyncThunk(
    'feeds/editFeeds',
    async (feed) => {
        const response = await api.put(`/feeds/${feed.id}`, feed);
        return response.data;
    }
);

export const removeFeed = createAsyncThunk(
    'feeds/removeFeed',
    async (id) => {
        await api.delete(`/feeds/${id}`);
        return id;
    }
)

const feedsSlice = createSlice({
    name: 'feeds',
    initialState:{
        list:[],
        loading:false,
        error:null,
        currenFeedId:null,
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
        .addCase(editFeed.fulfilled, (state, action) => {
            const idx = state.list.findIndex((f) => f.id === action.payload.id);
            if (idx !== -1) state.list[idx] = action.payload;
        })
        .addCase(removeFeed.fulfilled, (state, action) => {
            state.list = state.list.filter((f) => f.id !== action.payload);
            if (state.currentFeedId === action.payload) state.currentFeedId = null;
        });
    },
});

export const { selectFeed, clearFeedsError } = feedsSlice.actions;
export default feedsSlice.reducer;