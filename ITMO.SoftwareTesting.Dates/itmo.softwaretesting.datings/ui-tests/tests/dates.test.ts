import { WebDriver } from 'selenium-webdriver';
import { cleanLocalStorage, createWebDriver, url } from '../utils/selenium';
import { signUp } from '../pages/auth';
import { createGroup } from '../pages/groups';
import { eventCreateDateButton, eventTitle } from '../pages/events';
import { dateEvents, groupDatesGroup, groupName, modalCreateDateButton, modalGroups } from '../pages/dates';

describe('dates', () => {
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

	it('should allow to create a date', async () => {
		const expectedGroupName = '1';

		await signUp(browser);
		await createGroup(browser, expectedGroupName, '1');

		await browser.get(url('events'));
		const expectedEventTitle = await eventTitle(browser);

		await eventCreateDateButton(browser).then(x => x.click());
		await browser.sleep(200);

		await modalGroups(browser).then(([x]) => x.click());
		await modalCreateDateButton(browser).then(x => x.click());

		await browser.get(url('dates'));
		await browser.navigate().refresh();

		const [group] = await groupDatesGroup(browser);
		const actualGroupName = await groupName(group).getText();
		const [event] = await dateEvents(group);

		expect(actualGroupName).toBe(expectedGroupName);
		expect(event.text).toBe(expectedEventTitle);
	});
});
