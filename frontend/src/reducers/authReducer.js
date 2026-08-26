import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import api from '../utils/api';
import urls from '../utils/urls';
import { isTokenValid } from '../utils/tokenUtils';

const storedToken = localStorage.getItem('token');
const tokenIsValid = isTokenValid(storedToken);

if (storedToken && !tokenIsValid) {
    localStorage.removeItem('token');
}

export const login = createAsyncThunk(
    'auth/login',
    async (credentials, { rejectWithValue }) => {
        try {
            const response = await api.post(urls.LOGIN_URL, credentials);
            localStorage.setItem('token', response.data.token);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.detail || err.message);
        }
    }
);

export const register = createAsyncThunk(
    'auth/register',
    async(payload, { rejectWithValue }) => {
        try{
            const response = await api.post(urls.REGISTER_URL, payload);
            localStorage.setItem('token', response.data.token);

            return response.data;
        }
        catch (err) {
            return rejectWithValue(err.response?.data?.detail || err.message);
        }
    }
);

export const changePassword = createAsyncThunk(
    'auth/changePassword',
    async ({ currentPassword, newPassword }, { rejectWithValue }) => {
        try {
            await api.put(urls.CHANGE_PASSWORD_URL, { currentPassword, newPassword });
            return true;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const getProfile = createAsyncThunk(
    'auth/getProfile',
    async (_, { rejectWithValue }) => {
        try {
            const response = await api.get(urls.GET_PROFILE_URL);
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

export const updateProfile = createAsyncThunk(
    'auth/updateProfile',
    async ({ userName, email }, { rejectWithValue }) => {
        try {
            const response = await api.put(urls.UPDATE_PROFILE_URL, { userName, email });
            return response.data;
        } catch (err) {
            return rejectWithValue(err.response?.data?.message || err.message);
        }
    }
);

const authSlice = createSlice({
    name: 'auth',
    initialState: {
        user: null,
        token: tokenIsValid ? storedToken : null,
        isAuthenticated: tokenIsValid,
        status: 'idle',
        error: null,
    },
    reducers: {
        logout(state) {
            state.user = null;
            state.token = null;
            state.isAuthenticated = false;
            localStorage.removeItem('token');
        },
        clearAuthError(state) {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.status = 'loading';
                state.error = null;
            })
            .addCase(login.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.token = action.payload.token;
                state.user = action.payload.user ?? null;
                state.isAuthenticated = true;
            })
            .addCase(login.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.payload;
            })
            .addCase(register.pending, (state) => {
                state.status = 'loading';
                state.error = null;
            })
            .addCase(register.fulfilled, (state, action) =>{
                state.status = 'succeeded';
                state.token = action.payload.token;
                state.user = action.payload.user ?? null;
                state.isAuthenticated = true;
            })
            .addCase(register.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.payload;
            })
            .addCase(changePassword.rejected, (state, action) => {
                state.error = action.payload;
            })
            .addCase(getProfile.fulfilled, (state, action) => {
                state.user = action.payload;
            })
            .addCase(updateProfile.fulfilled, (state, action) => {
                state.user = action.payload;
            })
            .addCase(updateProfile.rejected, (state, action) => {
                state.error = action.payload;
            });
    },
});

export const { logout, clearAuthError } = authSlice.actions;
export default authSlice.reducer;