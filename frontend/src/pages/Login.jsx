import { login, clearAuthError } from 'Actions/authActions';
import { useDispatch, useSelector } from 'react-redux';
import { useState, useCallback } from 'react';


const FormStatus = Object.freeze({
    Loading: 'loading',
});

export default function Login() {
    const dispatch = useDispatch();
    const { status, error, isAuthenticated } = useSelector((state) => state.auth);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = useCallback((e) => {
        e.preventDefault();
        dispatch(login({ email, password }));
    }, [dispatch, email, password]);

    const handleEmailChange = useCallback((e) => {
        setEmail(e.target.value);
        if (error) dispatch(clearAuthError());
    }, [error, dispatch]);

    const handlePasswordChange = useCallback((e) => {
        setPassword(e.target.value);
        if (error) dispatch(clearAuthError());
    }, [error, dispatch]);

    if(isAuthenticated) {
        return (
            <div className="auth-page">
                <p className="empty-text">Logged in!</p>
            </div>
        );
    }
    
    return (
        <div className="auth-page">
            <h1>Log In</h1>
            <form onSubmit={handleSubmit}>
                <input 
                    type="email"
                    value={email}
                    onChange={handleEmailChange}
                    placeholder="Email"
                    required 
                />
                <input 
                    type="password"
                    value={password}
                    onChange={handlePasswordChange}
                    placeholder="Password"
                    required 
                />
                <button
                    type="submit"
                    disabled={status === FormStatus.Loading}    
                >
                    {status === FormStatus.Loading ? 'Logging in...' : 'Login'}
                </button>
                {error && <p>{error}</p>}
            </form>
        </div>
    );
};