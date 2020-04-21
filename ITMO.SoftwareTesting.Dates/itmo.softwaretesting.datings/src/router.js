import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import AuthPage from './views/AuthPage';
import {token} from './tools/token';
import GroupsPage from './views/GroupsPage';
import UserPage from './views/UserPage';
import AllEventsPage from './views/events/AllEventsPage';
import FavoritesEventsPage from './views/events/FavoritesEventsPage';
import MainEventsPage from './views/events/MainEventsPage';
import MainHomePage from './views/MainHomePage';
import DatesPage from './views/DatesPage';

Vue.use(Router);

const route = (path, component) => ({path, component, name: path});

export default new Router({
	mode: 'history',
	base: process.env.BASE_URL,
	routes: [
		{
			path: '',
			name: 'home',
			component: Home,
			beforeEnter: (from, to, next) => {
				if (token()) {
					next();
				} else {
					next({name: 'auth'});
				}
			},
			children: [
				{path: '', component: MainHomePage, name: 'main-home'},
				route('groups', GroupsPage),
				route('user', UserPage),
				route('dates', DatesPage),
				{
					path: 'events', component: MainEventsPage,
					children: [
						{path: '', component: AllEventsPage, name: 'events'},
						{path: 'favorites', component: FavoritesEventsPage, name: 'favorites'},
					]
				},
			],
		},
		{
			path: '/auth',
			name: 'auth',
			component: AuthPage,
			beforeEnter: (from, to, next) => {
				if (!token()) {
					next();
				} else {
					next({name: '/'});
				}
			},
		},
	],
});
