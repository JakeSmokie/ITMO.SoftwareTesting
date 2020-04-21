import {createBrowser} from '../utils/selenium';
import { By, Key, until, WebDriver } from 'selenium-webdriver';

describe('webdriver test', () => {
	let browser: WebDriver;

	beforeAll(async () => {
		browser = await createBrowser();
	});

	afterAll(async () => {
		await browser.quit();
	});

	it('should work', async () => {
		await browser.get('http://www.google.com/ncr');
		await browser.findElement(By.name('q')).sendKeys('webdriver', Key.RETURN);
		await browser.wait(until.titleIs('webdriver - Google Search'), 1000);
	});
});
