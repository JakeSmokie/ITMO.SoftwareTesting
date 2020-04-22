import { By, Key, until, WebDriver, WebElement } from 'selenium-webdriver';
import { url } from '../utils/selenium';
import { mapAsync } from '../utils/utils';

export const groupsPageButton = (browser: WebDriver) => browser.findElement(By.id('page-nav/groups'));
export const groupCreationName = (browser: WebDriver) => browser.findElement(By.id('group-creation-name'));
export const groupCreationPurpose = (browser: WebDriver) => browser.findElement(By.id('group-creation-purpose'));
export const groupCreationButton = (browser: WebDriver) => browser.findElement(By.id('group-creation-button'));

export const groups = async (browser: WebDriver, waitForList = true) => {
	if (waitForList) {
		await browser.wait(until.elementLocated(By.id('groups-list')));
	}

	return await browser.findElements(By.className('groups-list-item'));
};

export const oneGroup = async (browser: WebDriver) => {
	const group = await groups(browser);
	expect(group).toHaveLength(1);

	return group[0];
};

export const groupDeletionButton = (group: WebElement) => group.findElement(By.className('group-deletion-button'));

export const groupsMainInfo = async (browser: WebDriver) => {
	const items = await groups(browser);
	return await mapAsync(items, async x => ({
		name: await x.findElement(By.className('group-name')).getText(),
		purpose: await x.findElement(By.className('group-purpose')).getText(),
	}));
};

export const tryCreateGroup = async (browser: WebDriver, name: string, purpose: string) => {
	await groupsPageButton(browser).click();
	await browser.wait(until.urlIs(url('groups')));

	await groupCreationName(browser).sendKeys(Key.CONTROL + 'a', Key.DELETE, name);
	await groupCreationPurpose(browser).sendKeys(Key.CONTROL + 'a', Key.DELETE, purpose);
	await groupCreationButton(browser).click();
};
