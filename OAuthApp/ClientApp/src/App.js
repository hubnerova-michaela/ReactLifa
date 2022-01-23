import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { NavMenu} from './components/NavMenu';
import Home from './components/Home';
import NotFound from './components/General/NotFound';
import Dashboard from './components/Dashboard';
import { AuthProvider } from "./providers/AuthProvider";
import SignInCallback from "./components/Auth/SignInCallback";
import SignOutCallback from "./components/Auth/SignOutCallback";
import SilentRenewCallback from "./components/Auth/SilentRenewCallback";

import './custom.css'
import Reality from './components/Reality';
import Profile from './components/Profile';
import Character from './components/Character';
import Location from './components/Location';

export default class App extends Component {
	static displayName = App.name;

	render() {
		return (
			<AuthProvider>
				<NavMenu />
				<Routes>
					<Route path="/oidc-callback" element={<SignInCallback />} />
					<Route path="/oidc-signout-callback" element={<SignOutCallback />} />
					<Route path="/oidc-silent-renew" element={<SilentRenewCallback />} />
					<Route index path='/' element={<Home />} />
					<Route path="/sign-in" element={<Home />} />
					<Route path="/sign-out" element={<Home />} />
					<Route path='/dashboard' element={<Dashboard />} />
					<Route path="*" element={<NotFound />} />

					<Route path="/reality/:id" element={<Reality />} />
					<Route path="/profile" element={<Profile />} />
					<Route path="/reality/:id/character/:id" element={<Character/>} />
					<Route path="/reality/:id/location/:id" element={<Location/>} />
					
				</Routes>

			</AuthProvider>
		);
	}
}
