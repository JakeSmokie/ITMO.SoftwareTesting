import { WebDriver } from 'selenium-webdriver';
import { cleanLocalStorage, createWebDriver, url } from '../utils/selenium';
import { signUp } from '../pages/auth';
import {
	eventCategories,
	eventCreateDateButtonIsPresent,
	eventFavButton, eventLocation,
	events,
	eventTitle,
	secondCategoryInFilter, secondLocationInFilter,
} from '../pages/events';
import { mapAsync } from '../utils/utils';

describe('favorite events', () => {
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

	it('should show the list of available events', async () => {
		await signUp(browser);
		await browser.get(url('events'));

		expect(await events(browser)).toHaveLength(20);
	});

	it('should show details of event', async () => {
		await signUp(browser);
		await browser.get(url('events'));

		const [{element, text}] = await events(browser);
		await element.click();

		expect(await eventTitle(browser)).toBe(text);
	});

	it('should not show date creation button if no groups are created', async () => {
		await signUp(browser);
		await browser.get(url('events'));

		const [{element}] = await events(browser);
		await element.click();

		expect(await eventCreateDateButtonIsPresent(browser)).toBe(false);
	});

	it('should allow to add and remove event in favorites', async () => {
		await signUp(browser);
		await browser.get(url('events'));

		const [{element, text}] = await events(browser);
		await element.click();

		await eventFavButton(browser).then(x => x.click());
		await browser.sleep(200);

		await browser.get(url('events/favorites'));
		await browser.navigate().refresh();

		const afterAdd = await events(browser).then(x => x.map(x => x.text));
		await eventFavButton(browser).then(x => x.click());

		await browser.navigate().refresh();
		const afterRemove = await events(browser, false).then(x => x.map(x => x.text));

		expect(afterAdd).toContain(text);
		expect(afterRemove).not.toContain(text);
	});

	describe('filters', () => {
		it('should filter out events by categories', async () => {
			await signUp(browser);
			await browser.get(url('events'));
			await events(browser);

			const option = await secondCategoryInFilter(browser);
			await option.click();
			const category = await option.getAttribute('value');

			const categories = await mapAsync(
				await events(browser),
				async x => {
					await x.element.click();
					return eventCategories(browser);
				},
			);

			for (const actual of categories) {
				expect(actual).toContain(category);
			}
		}, 120000);

		it('should filter out events by location', async () => {
			await signUp(browser);
			await browser.get(url('events'));
			await events(browser);

			const option = await secondLocationInFilter(browser);
			await option.click();
			const location = await option.getAttribute('value');

			const locations = await mapAsync(
				await events(browser),
				async x => {
					await x.element.click();
					return eventLocation(browser);
				},
			);

			expect(new Set(locations.filter(x => x !== 'online'))).toStrictEqual(new Set([location]));
		}, 120000);
	});
});
