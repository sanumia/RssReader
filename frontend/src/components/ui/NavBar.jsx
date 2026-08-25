import { NavLink } from 'react-router-dom';
import { useSelector } from 'react-redux';
import UserMenu from './UserMenu';

export default function NavBar() {
    const isAuthenticated = useSelector((state) => state.auth.isAuthenticated);

    return (
        <div className="navMenu">
            <ul>
                <li>
                    <NavLink to="/">Home</NavLink>
                </li>
                {
                    isAuthenticated && (
                        <>
                            <li>
                                <NavLink to="/feeds">Feeds</NavLink>
                            </li>
                            <li>
                                <NavLink to="/feed-items">Feed Items List</NavLink>
                            </li>
                        </>
                    )
                }

                <li className="navMenu__spacer" />

                {
                    isAuthenticated 
                        ? (
                            <li>
                                <UserMenu />
                            </li>
                        ) 
                        : (
                            <>
                                <li>
                                    <NavLink to="/login">Login</NavLink>
                                </li>
                                <li>
                                    <NavLink to="/register" className="btn-outline">
                                        Create Account
                                    </NavLink>
                                </li>
                            </>
                        )}
            </ul>
        </div>
    );
}