import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import api from '../utils/api';
import urls from '../utils/urls';

export const getGlobalItems = createAsyncThunk(
    'feedItems/getGlobalItems',
    async (filters = {}, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_GLOBAL_ITEMS_URL, { params: filters });
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getPersonalItems = createAsyncThunk(
    'feedItems/getPersonalItems',
    async (filters = {}, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_PERSONAL_ITEMS_URL, { params: filters });
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getPersonalItemsFiltered = createAsyncThunk(
    'feedItems/getPersonalItemsFiltered',
    async (filters = {}, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_PERSONAL_ITEMS_FILTERED_URL, { params: filters });
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const markItemRead = createAsyncThunk(
    'feedItems/markItemRead',
    async ({ itemId, isRead = true }, { rejectWithValue }) => {
        try {
            await api.post(urls.MARK_ITEM_READ_URL(itemId), null, { params: { isRead } });
            return { itemId, isRead };
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const toggleItemFavorite = createAsyncThunk(
    'feedItems/toggleItemFavorite',
    async ({ itemId, isFavorite }, { rejectWithValue }) => {
        try {
            await api.post(urls.TOGGLE_ITEM_FAVORITE_URL(itemId), null, { params: { isFavorite } });
            return { itemId, isFavorite };
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const removeItem = createAsyncThunk(
    'feedItems/removeItem',
    async (itemId, { rejectWithValue }) => {
        try {
            await api.delete(urls.REMOVE_ITEM_URL(itemId));
            return itemId;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getItemsByFeed = createAsyncThunk(
    'feedItems/getItemsByFeed',
    async ({ feedId, pageNumber = 1, pageSize = 20 }, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_ITEMS_BY_FEED_URL(feedId), {
                params: { pageNumber, pageSize },
            });
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const addItemToFeed = createAsyncThunk(
    'feedItems/addItemToFeed',
    async ({ feedId, item }, { rejectWithValue }) => {
        try {
            const response = await api.post(urls.ADD_ITEM_TO_FEED_URL(feedId), item);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getItemsByFeedGrouped = createAsyncThunk(
    'feedItems/getItemsByFeedGrouped',
    async ({ feedId, pageNumber = 1, pageSize = 50 }, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_ITEMS_BY_FEED_GROUPED_URL(feedId), {
                params: { pageNumber, pageSize },
            });
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getItem = createAsyncThunk(
    'feedItems/getItem',
    async(itemId, {rejectWithValue}) => {
        try {
            const response = await api.get(urls.GET_ITEM_URL(itemId));
            return response.data;
        }
        catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
)

export const updateFeedItem = createAsyncThunk(
    'feedItems/updateFeedItem',
    async ({ itemId, updates }, { rejectWithValue }) => {
        try {
            const response = await api.put(urls.UPDATE_ITEM_URL(itemId), updates);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

const feedItemsSlice = createSlice({
    name: 'feedItems',
    initialState: {
        list: [],
        pagination: { pageNumber: 1, pageSize: 20, totalCount: 0 },
        loading: false,
        error: null,
    },
    reducers: {
        clearItemsError(state) {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(getGlobalItems.pending, (state) => { 
                state.loading = true; state.error = null; 
            })
            .addCase(getGlobalItems.fulfilled, (state, action) => { 
                state.loading = false; state.list = action.payload; 
            })
            .addCase(getGlobalItems.rejected, (state, action) => { 
                state.loading = false; state.error = action.payload; 
            })

            .addCase(getPersonalItems.pending, (state) => { 
                state.loading = true; state.error = null; 
            })
            .addCase(getPersonalItems.fulfilled, (state, action) => { 
                state.loading = false; state.list = action.payload; 
            })
            .addCase(getPersonalItems.rejected, (state, action) => { 
                state.loading = false; state.error = action.payload; 
            })

            .addCase(getPersonalItemsFiltered.pending, (state) => { 
                state.loading = true; state.error = null; 
            })
            .addCase(getPersonalItemsFiltered.fulfilled, (state, action) => { 
                state.loading = false; state.list = action.payload; 
            })
            .addCase(getPersonalItemsFiltered.rejected, (state, action) => { 
                state.loading = false; state.error = action.payload; 
            })

            .addCase(markItemRead.fulfilled, (state, action) => {
                const item = state.list.find((i) => i.id === action.payload.itemId);
                if (item) item.isRead = action.payload.isRead;

                if (state.currentItem?.id === action.payload.itemId) {
                    state.currentItem.isRead = action.payload.isRead;
                }
            })
            .addCase(toggleItemFavorite.fulfilled, (state, action) => {
                const item = state.list.find((i) => i.id === action.payload.itemId);
                if (item) item.isFavorite = action.payload.isFavorite;

                if (state.currentItem?.id === action.payload.itemId) {
                    state.currentItem.isFavorite = action.payload.isFavorite;
                }
            })
            .addCase(removeItem.fulfilled, (state, action) => {
                state.list = state.list.filter((i) => i.id !== action.payload);
            })
            .addCase(removeItem.rejected, (state, action) => {
                state.error = action.payload;
            })
            .addCase(getItemsByFeed.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(getItemsByFeed.fulfilled, (state, action) => {
                state.loading = false;
                state.list = action.payload;
            })
            .addCase(getItemsByFeed.rejected, (state, action) => { 
                state.loading = false; state.error = action.payload; 
            })
            .addCase(addItemToFeed.fulfilled, (state, action) => {
                state.list.push(action.payload);
            })
            .addCase(addItemToFeed.rejected, (state, action) => {
                state.error = action.payload;
            })
            .addCase(getItemsByFeedGrouped.fulfilled, (state, action) => {
                state.loading = false;
                state.grouped = action.payload;
            })
            .addCase(getItem.fulfilled, (state, action) => {
                state.loading = false;
                state.currentItem = action.payload;
            })
            .addCase(getItem.pending, (state, action) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(getItem.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload;
            })
            .addCase(updateFeedItem.fulfilled, (state, action) => {
                const updated = action.payload;
                const index = state.list.findIndex((i) => i.id === updated.id);
                if (index !== -1) state.list[index] = updated;

                if (state.grouped) {
                    for (const key of ['today', 'yesterday', 'lastWeek', 'older']) {
                        const group = state.grouped[key];
                        if (group) {
                            const idx = group.findIndex((i) => i.id === updated.id);
                            if (idx !== -1) {
                                group[idx] = updated;
                                break;
                            }
                        }
                    }
                }

                if (state.currentItem?.id === updated.id) {
                    state.currentItem = updated;
                }

                state.loading = false;
                state.error = null;
            })
            .addCase(updateFeedItem.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload;
            })
            .addCase(updateFeedItem.pending, (state) => {
                state.loading = true;
                state.error = null;
            });
    },
});

export const { clearItemsError } = feedItemsSlice.actions;
export default feedItemsSlice.reducer;