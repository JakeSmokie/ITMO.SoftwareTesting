import { By, until, WebDriver } from 'selenium-webdriver';

export const ts = (string: string) => string + '_' + +new Date();
export const token = (browser: WebDriver) => browser.executeScript(() => localStorage.getItem('datings-token'));

export const toastText = async (browser: WebDriver) => {
	await browser.wait(until.elementsLocated(By.className('toast-body')));
	return browser.findElement(By.className('toast-body')).getText();
};
