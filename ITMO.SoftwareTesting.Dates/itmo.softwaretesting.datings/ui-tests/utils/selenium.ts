import { Builder, By, until, WebDriver } from 'selenium-webdriver';

export const url = (url = '') => 'http://localhost:8080/' + url;

export const createWebDriver = () =>
	new Builder()
		.forBrowser('chrome')
		.usingServer('http://localhost:4444/wd/hub')
		.build();
