import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import AuthPage from './views/AuthPage';
import {token} from './tools/token';
import GroupsPage from './views/GroupsPage';
import UserPage from './views/UserPage';

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
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
        {
          path: 'groups',
          name: 'groups',
          component: GroupsPage
        },
        {
          path: 'user',
          name: 'user',
          component: UserPage
        },
      ]
    },
    {
      path: '/about',
      name: 'about',
      component: () => import(/* webpackChunkName: "about" */ './views/About.vue')
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
      }
    }
  ]
})
