import {
	groupCreationButton,
	groupDeletionButton,
	groups,
	groupsMainInfo,
	oneGroup,
	tryCreateGroup,
} from '../pages/groups';
import { WebDriver } from 'selenium-webdriver';
import { cleanLocalStorage, createWebDriver } from '../utils/selenium';
import { signUp } from '../pages/auth';
import { ts } from '../utils/utils';

describe('groups page', () => {
	let browser: WebDriver;

	beforeAll(async () => {
		browser = await createWebDriver();
	});

	afterEach(async () => {
		await cleanLocalStorage(browser);
	});

	afterAll(async () => {
		await browser.quit();
	});

	it('should create group', async () => {
		const name = 'The group';
		const purpose = 'The purpose of group';

		await signUp(browser, ts(''), 'Random Password');
		await tryCreateGroup(browser, name, purpose);

		expect(await groups(browser)).toHaveLength(1);
		expect(await groupsMainInfo(browser)).toStrictEqual([{name, purpose}]);
	});

	it('should not allow to create group if no name and purpose of group provided', async () => {
		await signUp(browser, ts(''), 'Random Password');

		await tryCreateGroup(browser, '', '');
		const first = await groupCreationButton(browser).isEnabled();

		await tryCreateGroup(browser, 'abcdef', '');
		const second = await groupCreationButton(browser).isEnabled();

		await tryCreateGroup(browser, '', 'abcdef');
		const third = await groupCreationButton(browser).isEnabled();

		expect(first).toBe(false);
		expect(second).toBe(false);
		expect(third).toBe(false);
	});

	it('should delete group', async () => {
		await signUp(browser, ts(''), 'Random Password');
		await tryCreateGroup(browser, '123', '3456');

		const group = await oneGroup(browser);
		await groupDeletionButton(group).click();

		expect(await groups(browser, false)).toHaveLength(0);
	});
});
