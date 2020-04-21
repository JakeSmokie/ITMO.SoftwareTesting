import {Builder} from 'selenium-webdriver';

export const createBrowser = () =>
	new Builder()
		.forBrowser('chrome')
		.usingServer('http://localhost:4444/wd/hub')
		.build();
