const GET_ALL_FEEDS_URL = '/feeds/all';
const GET_FEEDS_URL = '/feeds';
const ADD_FEED_URL = '/feeds';
const EDIT_FEED_URL = (id) => `/feeds/${id}`;
const REMOVE_FEED_URL = (id) => `/feeds/${id}`;
const GET_ALL_FEEDS_WITH_USER_INFO_URL = '/feeds/all-user';

const GET_PROFILE_URL = '/user/admin';
const UPDATE_PROFILE_URL = '/user/admin';
const DELETE_ACCOUNT_URL = '/user/admin';

const LOGIN_URL = '/auth/login';
const REGISTER_URL = '/auth/register';
const CHANGE_PASSWORD_URL = '/auth/change-password';

const GET_GLOBAL_ITEMS_URL = '/feed-items/global';
const GET_PERSONAL_ITEMS_URL = '/feed-items/personal';
const GET_PERSONAL_ITEMS_FILTERED_URL = '/feed-items/filtered';
const GET_ITEM_URL = (itemId) => `/feed-items/${itemId}`;
const UPDATE_ITEM_URL = (itemId) => `/feed-items/${itemId}`;
const MARK_ITEM_READ_URL = (itemId) => `/feed-items/${itemId}/read`;
const TOGGLE_ITEM_FAVORITE_URL = (itemId) => `/feed-items/${itemId}/favorite`;
const REMOVE_ITEM_URL = (itemId) => `/feed-items/${itemId}`;

const GET_ITEMS_BY_FEED_URL = (feedId) => `/feed/${feedId}/feed-items`;
const GET_ITEMS_BY_FEED_GROUPED_URL = (feedId) => `/feed/${feedId}/feed-items/grouped`;
const ADD_ITEM_TO_FEED_URL = (feedId) => `/feed/${feedId}/feed-items`;

const GET_FOLDERS_URL = '/folders';
const ADD_FOLDER_URL = '/folders';
const EDIT_FOLDER_URL = (id) => `/folders/${id}`;
const REMOVE_FOLDER_URL = (id) => `/folders/${id}`;
const ADD_FEED_TO_FOLDER_URL = (folderId, feedId) => `/folders/${folderId}/feeds/${feedId}`;
const REMOVE_FEED_FROM_FOLDER_URL = (folderId, feedId) => `/folders/${folderId}/feeds/${feedId}`;
const GET_FEEDS_IN_FOLDER_URL = (folderId) => `/folders/${folderId}/feeds`;

export default {
    GET_ALL_FEEDS_URL,
    GET_FEEDS_URL,
    ADD_FEED_URL,
    EDIT_FEED_URL,
    REMOVE_FEED_URL,
    GET_ALL_FEEDS_WITH_USER_INFO_URL,

    GET_PROFILE_URL,
    DELETE_ACCOUNT_URL,
    UPDATE_PROFILE_URL,

    LOGIN_URL,
    REGISTER_URL,
    CHANGE_PASSWORD_URL,

    GET_GLOBAL_ITEMS_URL, 
    GET_PERSONAL_ITEMS_URL, 
    GET_PERSONAL_ITEMS_FILTERED_URL,
    GET_ITEM_URL, 
    UPDATE_ITEM_URL,
    MARK_ITEM_READ_URL, 
    TOGGLE_ITEM_FAVORITE_URL, 
    REMOVE_ITEM_URL,

    GET_ITEMS_BY_FEED_URL, 
    GET_ITEMS_BY_FEED_GROUPED_URL,
    ADD_ITEM_TO_FEED_URL,

    GET_FOLDERS_URL,
    ADD_FOLDER_URL,
    EDIT_FOLDER_URL,
    REMOVE_FOLDER_URL,
    ADD_FEED_TO_FOLDER_URL,
    REMOVE_FEED_FROM_FOLDER_URL,
    GET_FEEDS_IN_FOLDER_URL,
};
