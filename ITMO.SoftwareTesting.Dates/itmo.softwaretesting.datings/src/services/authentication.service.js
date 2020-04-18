import {http} from '../tools/axios';

export const signUp = (nickname, password) =>
	http.post('api/authentication/sign-up', {nickname, password});

export const signIn = (nickname, password) =>
	http.post('api/authentication/sign-in', {nickname, password});

export const deleteAccount = (password) =>
	http.post('api/authentication/delete-account', {password});
