import { By, until, WebDriver, WebElement } from 'selenium-webdriver';
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

const namePurposeSelector = async (x: WebElement) => ({
	name: await x.findElement(By.className('group-name')).getText(),
	purpose: await x.findElement(By.className('group-purpose')).getText(),
});

export const groupsMainInfo = async (browser: WebDriver) =>
	mapAsync(groups(browser), namePurposeSelector);

export const tryCreateGroup = async (browser: WebDriver, name: string, purpose: string) => {
	await groupsPageButton(browser).click();
	await browser.wait(until.urlIs(url('groups')));

	await groupCreationName(browser).sendKeys(name);
	await groupCreationPurpose(browser).sendKeys(purpose);
	await groupCreationButton(browser).click();
};

export const createGroup = async (browser: WebDriver, name: string, purpose: string) => {
	await tryCreateGroup(browser, name, purpose);
	return oneGroup(browser);
};

const groupSpoiler = (group: WebElement) => group.findElement(By.className('group-spoiler'));
const groupDetails = (group: WebElement) => group.findElement(By.className('group-details'));

export const openGroup = async (group: WebElement) => {
	const detailsLocated = await group.findElements(By.className('group-details')).then(x => x.length === 1);

	if (detailsLocated) {
		return;
	}

	await groupSpoiler(group).click();
	await group.getDriver().wait(until.elementLocated(By.className('group-details')));
};

export const groupMembers = (group: WebElement) =>
	mapAsync(
		group.findElements(By.className('group-member')),
		x => x.getText(),
	);

const groupInvitationNickname = (group: WebElement) => group.findElement(By.className('group-invitation-nickname'));
export const groupInvitationButton = (group: WebElement) => group.findElement(By.className('group-invitation-button'));

export const inviteUserInGroup = async (group: WebElement, userNickname: string) => {
	await openGroup(group);

	await groupInvitationNickname(group).sendKeys(userNickname);
	await groupInvitationButton(group).click();
};

export const groupInvitations = async (group: WebElement, wait = false) => {
	if (wait) {
		await group.getDriver().wait(until.elementLocated(By.className('group-invitation')));
	}

	return mapAsync(
		group.findElements(By.className('group-invitation')),
		x => x.getText(),
	);
};

export const groupForeignInvitations = async (browser: WebDriver, wait: boolean) => {
	const locator = By.className('group-foreign-invitation');

	if (wait) {
		await browser.wait(until.elementLocated(locator));
	}

	return mapAsync(browser.findElements(locator), async x => ({
		...await namePurposeSelector(x),
		button: await x.findElement(By.className('group-invitation-acceptance-button'))
	}));
};



