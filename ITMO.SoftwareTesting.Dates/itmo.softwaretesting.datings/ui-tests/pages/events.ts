import { By, until, WebDriver } from 'selenium-webdriver';
import { mapAsync, selectElementAndText, waitAndFindElement, waitAndFindElements } from '../utils/utils';

export const events = async (browser: WebDriver, wait = true) => {
	if (wait) {
		await browser.wait(until.elementLocated(By.className('event-list-item')));
	}

	return mapAsync(
		browser.findElements(By.className('event-list-item')),
		selectElementAndText,
	);
};

export const eventCreateDateButtonIsPresent = (browser: WebDriver) =>
	browser.findElements(By.id('event-create-date-button')).then(x => x.length > 0);

export const eventCreateDateButton = (browser: WebDriver) =>
	waitAndFindElement(browser, By.id('event-create-date-button'));

export const eventFavButton = (browser: WebDriver) =>
	waitAndFindElement(browser, By.id('event-fav-button'));

export const eventTitle = (browser: WebDriver) =>
	waitAndFindElement(browser, By.id('event-details-card'))
		.then(x => x.findElement(By.className('card-title')).getText());

export const eventCategories = (browser: WebDriver) =>
	waitAndFindElements(browser, By.className('event-category'))
		.then(x => mapAsync(x, x => x.getAttribute('text')));

export const eventLocation = (browser: WebDriver) =>
	waitAndFindElement(browser, By.id('event-location')).then(x => x.getAttribute('text'));

export const secondCategoryInFilter = async (browser: WebDriver) => {
	const locator = By.xpath('//*[@id="category-filter"]/option');

	await browser.wait(until.elementsLocated(locator));
	return browser.findElements(locator).then(([, x]) => x);
};

export const secondLocationInFilter = async (browser: WebDriver) => {
	const locator = By.xpath('//*[@id="location-filter"]/option');

	await browser.wait(until.elementsLocated(locator));
	return browser.findElements(locator).then(([, x]) => x);
};

