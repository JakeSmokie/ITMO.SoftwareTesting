import axios from 'axios';
import Vue from 'vue';
import {token} from './token';

export const http = axios.create({
	baseURL: 'http://localhost:5000',
});

export const kudaGo = axios.create({
	baseURL: 'http://localhost:5000/kudago/',
});

http.interceptors.request.use(config => {
	if (token()) {
		config.headers.Authorization = 'Bearer ' + token();
	}

	return config;
});

http.interceptors.response.use(x => x, error => {
	const vm = new Vue();

	vm.$bvToast.toast(error.response.data.detail, {
		title: 'Произошла ошибка',
		autoHideDelay: 5000,
		variant: 'danger',
		toaster: 'b-toaster-bottom-right',
	});

	return Promise.reject(error);
});
