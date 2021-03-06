import {
	createGroup,
	groupCreationButton,
	groupDeletionButton,
	groupDeletionButtonIsPresent,
	groupEditFormIsPresent,
	groupEditFormName,
	groupEditFormPurpose,
	groupEditFormSaveButton,
	groupForeignInvitations,
	groupInvitationButton,
	groupInvitations,
	groupMemberDeletionButtons,
	groupMembers,
	groups,
	groupsMainInfo,
	inviteInGroupAndAccept,
	inviteUserInGroup,
	namePurposeSelector,
	oneGroup,
	openGroup,
	tryCreateGroup,
} from '../pages/groups';
import { WebDriver } from 'selenium-webdriver';
import { cleanLocalStorage, createWebDriver, url } from '../utils/selenium';
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
		expect(await groupCreationButton(browser).isEnabled()).toBe(false);
	});

	it('should delete group', async () => {
		await signUp(browser, ts(''), 'Random Password');
		const group = await createGroup(browser, '123', '3456');
		await groupDeletionButton(group).click();

		await browser.navigate().refresh();
		await browser.sleep(200);

		expect(await groups(browser, false)).toHaveLength(0);
	});

	it('should provide list of members', async () => {
		const nickname = ts('');

		await signUp(browser, nickname, 'Random Password');
		const group = await createGroup(browser, '123', '3456');
		await openGroup(group);

		const [actualNickname, ...rest] = await groupMembers(group);

		expect(actualNickname).toBe(nickname);
		expect(rest).toHaveLength(0);
	});

	it('should not allow to invite nonexistent user', async () => {
		await signUp(browser, ts('first'));

		const group = await createGroup(browser, 'name', 'purpose');
		await inviteUserInGroup(group, ts('random_nick') + Math.random());

		expect(await groupInvitationButton(group).isEnabled()).toBe(false);
		expect(await groupInvitations(group, false)).toHaveLength(0);
	});

	it('should allow to edit group', async () => {
		const newName = 'new name';
		const newPurpose = 'new group';

		await signUp(browser, ts('first'));

		const group = await createGroup(browser, 'name', 'purpose');
		await openGroup(group);

		await groupEditFormName(group).clear();
		await groupEditFormName(group).sendKeys(newName);
		await groupEditFormPurpose(group).clear();
		await groupEditFormPurpose(group).sendKeys(newPurpose);
		await groupEditFormSaveButton(group).click();

		await browser.navigate().refresh();
		const {name, purpose} = await oneGroup(browser).then(x => namePurposeSelector(x));

		expect(name).toBe(newName);
		expect(purpose).toBe(newPurpose);
	});

	describe('two users groups page', () => {
		let secondBrowser: WebDriver;

		beforeAll(async () => {
			secondBrowser = await createWebDriver();
		});

		afterEach(async () => {
			await cleanLocalStorage(secondBrowser);
		});

		afterAll(async () => {
			await secondBrowser.quit();
		});

		it('should provide users invitation', async () => {
			const expectedName = 'name';
			const expectedPurpose = 'purpose';

			const first = ts('first');
			const second = ts('second');

			await Promise.all([signUp(browser, first), signUp(secondBrowser, second)]);

			const group = await createGroup(browser, expectedName, expectedPurpose);
			await inviteUserInGroup(group, second);
			const invitations = await groupInvitations(group, true);

			await secondBrowser.get(url('groups'));
			const [{name, purpose, button}] = await groupForeignInvitations(secondBrowser, true);

			await button.click();
			await secondBrowser.navigate().refresh();
			const [{name: secondName, purpose: secondPurpose}] = await groupsMainInfo(secondBrowser);

			await browser.navigate().refresh();
			const members = await oneGroup(browser).then(async x => {
				await openGroup(x);
				return groupMembers(x);
			});

			expect(invitations).toStrictEqual([second]);
			expect(name).toBe(expectedName);
			expect(purpose).toBe(expectedPurpose);
			expect(secondName).toBe(expectedName);
			expect(secondPurpose).toBe(expectedPurpose);
			expect(new Set(members)).toStrictEqual(new Set([first, second]));
		}, 1000000);

		it('should not allow second user to delete or edit a group', async () => {
			const first = ts('first');
			const second = ts('second');

			await Promise.all([signUp(browser, first), signUp(secondBrowser, second)]);

			const group = await createGroup(browser, 'F', 'F');
			await inviteInGroupAndAccept(group, second, browser, secondBrowser);

			expect(await oneGroup(secondBrowser).then(x => groupDeletionButtonIsPresent(x))).toBe(false);
			expect(await oneGroup(secondBrowser).then(x => groupEditFormIsPresent(x))).toBe(false);
		}, 100000);

		it('should allow to delete user from group', async () => {
			const first = ts('first');
			const second = ts('second');

			await Promise.all([signUp(browser, first), signUp(secondBrowser, second)]);

			let group = await createGroup(browser, 'F', 'F');
			await inviteInGroupAndAccept(group, second, browser, secondBrowser);

			await browser.navigate().refresh();
			group = await oneGroup(browser);
			await openGroup(group);

			await groupMemberDeletionButtons(group).then(x => x[0].click());

			await browser.navigate().refresh();
			const members = await oneGroup(browser).then(async x => {
				await openGroup(x);
				return groupMembers(x);
			});

			expect(members).toStrictEqual([first]);
		}, 100000);
	});
});
