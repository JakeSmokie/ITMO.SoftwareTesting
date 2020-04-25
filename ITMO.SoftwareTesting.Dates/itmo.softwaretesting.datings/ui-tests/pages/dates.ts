import { By, until, WebDriver, WebElement } from 'selenium-webdriver';
import { mapAsync, selectElementAndText, waitAndFindElement } from '../utils/utils';

export const groupDatesGroup = async (browser: WebDriver) => {
	const locator = By.className('group-dates-group');
	await browser.wait(until.elementLocated(locator));

	return browser.findElements(locator);
};
export const groupName = (group: WebElement) => group.findElement(By.className('group-name'));
export const dateEvents = (group: WebElement) =>
	mapAsync(
		group.findElements(By.className('date-event')),
		selectElementAndText,
	);

export const modalGroups = (browser: WebDriver) => browser.findElements(By.className('modal-group'));
export const modalCreateDateButton = (browser: WebDriver) => waitAndFindElement(browser, By.id('modal-create-date-button'));
