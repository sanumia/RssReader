<<<<<<< HEAD
import { configureStore } from '@reduxjs/toolkit'

export default configureStore({
    reducer: {
    },
});

=======
import { configureStore } from '@reduxjs/toolkit';
import rootReducer from '../reducers';

const store = configureStore({
    reducer: {
        rootReducer
    },
});

export default store;
>>>>>>> 296ef54 (setup initial redux toolkit)
