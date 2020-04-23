import { By, until, WebDriver } from 'selenium-webdriver';

export const ts = (string: string) => string + '_' + +new Date();
export const token = (browser: WebDriver) => browser.executeScript(() => localStorage.getItem('datings-token'));

export const assertToastText = async (browser: WebDriver, text: string) => {
	const locator = By.className('toast-body');

	await browser.wait(until.elementsLocated(locator));
	await browser.wait(until.elementTextIs(await browser.findElement(locator), text));
};

export async function mapAsync<T, U>(items: Promise<T[]> | T[], selector: ((x: T) => Promise<U>)): Promise<U[]> {
	const newItems: U[] = [];

	for (const item of await items) {
		newItems.push(await selector(item));
	}

	return newItems;
}
